using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer heartbeatUI;
    private monsterAttackSystem playerRef;
    private Color startColor;
    private Color endColor;

    private bool setupComplete;


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

        setupComplete = true;
    }

    private void UpdateHealthUI()
    {
        if (!setupComplete) { return; }

        int start = playerRef.MaxHealth;
        int end = playerRef.MaxHealth - playerRef.SegmentHealth;

        float healthPercentage = Mathf.InverseLerp(start, end, playerRef.CurrentHealth);
        heartbeatUI.color = Color.Lerp(startColor, endColor, healthPercentage);
    }
}
