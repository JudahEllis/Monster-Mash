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

    private monsterAttackSystem playerRef;
    private Gradient healthGradient;

    private bool allowUpdate;

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateHealthUI();
    }

    public void Setup(monsterAttackSystem playerRef, Gradient healthGradient)
    {
        this.playerRef = playerRef;
        this.healthGradient = healthGradient;

        float startPercent = 0;
        heartbeatUI.color = healthGradient.Evaluate(startPercent);

        SubscribeEvents();
        SetupStocks();

        allowUpdate = true;
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

        float healthPercentage = Mathf.InverseLerp(start, end, healthEventArgs.CurrentHealth);
        Color newColor = healthGradient.Evaluate(healthPercentage);

        if (heartbeatUI.color != newColor) 
        {
            float lerpTime = 0.2f;
            StartCoroutine(LerpColor(heartbeatUI.color, newColor, lerpTime));
        }
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
        allowUpdate = false;
        UnsubscribeEvents();
    }
}