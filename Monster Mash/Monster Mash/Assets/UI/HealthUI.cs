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
    private Color startColor;
    private Color endColor;

    private bool allowUpdate;

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    public void Setup(monsterAttackSystem playerRef, Color startColor, Color endColor)
    {
        this.playerRef = playerRef;
        this.startColor = startColor;
        this.endColor = endColor;

        heartbeatUI.color = startColor;

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

    private void UpdateHealthUI()
    {
        if (!allowUpdate) { return; }

        int start = playerRef.MaxHealth;
        int end = playerRef.MaxHealth - playerRef.SegmentHealth;

        float healthPercentage = Mathf.InverseLerp(start, end, playerRef.CurrentHealth);
        heartbeatUI.color = Color.Lerp(startColor, endColor, healthPercentage);
    }

    private void SubscribeEvents()
    {
        playerRef.OnMonsterPartRemoved += RemoveStock;
        playerRef.OnMonsterDeath += DisableHealthBar;
    }

    private void UnsubscribeEvents()
    {
        playerRef.OnMonsterPartRemoved -= RemoveStock;
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