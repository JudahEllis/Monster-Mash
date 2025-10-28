using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer heartbeatUI;

    [SerializeField] private GameObject[] stocks;
    private List<GameObject> activeStocks = new();
    private int stockIndex;
    private int lastSegmeantIndex = -1;
    private monsterAttackSystem playerRef;
    private Gradient healthGradient;
    private Coroutine colorCoroutine;

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void Setup(monsterAttackSystem playerRef, Gradient healthGradient)
    {
        this.playerRef = playerRef;
        this.healthGradient = healthGradient;

        float startPercent = 0;
        heartbeatUI.color = healthGradient.Evaluate(startPercent);

        SubscribeEvents();
        SetupStocks();
    }

    private void SetupStocks()
    {
        for (int i = 0; i < playerRef.GetActiveAttackSlots().Count; i++)
        {
            activeStocks.Add(stocks[i]);
            activeStocks[i].SetActive(true);
        }
    }

    private void UpdateHealthUI(object sender, HealthEventArgs healthEventArgs)
    {
        int start = healthEventArgs.MaxHealth;
        int end = healthEventArgs.MaxHealth - healthEventArgs.SegmentHealth;
        int segmeantIndex = GetCurrentSegmentIndex(healthEventArgs);

        float healthPercentage = Mathf.InverseLerp(start, end, healthEventArgs.CurrentHealth);
        Color newColor = healthGradient.Evaluate(healthPercentage);

        // flashes red if an entire segment was lost in one hit
        if (segmeantIndex != lastSegmeantIndex)
        {
            if (colorCoroutine  != null)
            {
                StopCoroutine(colorCoroutine);
            }

            colorCoroutine = StartCoroutine(ColorFlash());
        }
        else if (heartbeatUI.color != newColor)
        {
            if (colorCoroutine != null)
            {
                StopCoroutine(colorCoroutine);
            }

            float lerpTime = 0.2f;
            colorCoroutine = StartCoroutine(LerpColor(heartbeatUI.color, newColor, lerpTime));
        }

        lastSegmeantIndex = segmeantIndex;
    }

    private int GetCurrentSegmentIndex(HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.SegmentHealth <= 0) return 0;
        return (healthEventArgs.MaxHealth - healthEventArgs.CurrentHealth) / healthEventArgs.SegmentHealth;
    }

    IEnumerator LerpColor(Color startingColor, Color newColor, float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / time;
            heartbeatUI.color = Color.Lerp(startingColor, newColor, t);

            yield return null;
        }

        heartbeatUI.color = newColor;
        yield return null;
    }

    IEnumerator ColorFlash()
    {
        Color red = healthGradient.Evaluate(1);
        Color green = healthGradient.Evaluate(0);
        float colorLerpTime = 0.2f;
        float transitionTime = 0.2f;

        yield return StartCoroutine(LerpColor(heartbeatUI.color, red, colorLerpTime));
        yield return new WaitForSeconds(transitionTime);
        yield return StartCoroutine(LerpColor(heartbeatUI.color, green, colorLerpTime));
    }

    private void SubscribeEvents()
    {
        playerRef.OnMonsterPartRemoved += RemoveStock;
        playerRef.OnHealthUpdated += UpdateHealthUI;
        playerRef.OnMonsterDeath += DisableHealthBar;
    }

    private void UnsubscribeEvents()
    {
        playerRef.OnMonsterPartRemoved -= RemoveStock;
        playerRef.OnHealthUpdated += UpdateHealthUI;
        playerRef.OnMonsterDeath -= DisableHealthBar;
    }

    private void RemoveStock(object sender, EventArgs e)
    {
        activeStocks[stockIndex].SetActive(false);
        stockIndex += 1;

        stockIndex = Mathf.Clamp(stockIndex, 0, activeStocks.Count);
    }

    private void DisableHealthBar(object sender, EventArgs e)
    {
        heartbeatUI.gameObject.SetActive(false);
        UnsubscribeEvents();
    }
}