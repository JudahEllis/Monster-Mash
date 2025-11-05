using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerSpawnsParent;
    [SerializeField] private HealthUI[] healthUI;

    [SerializeField] private Gradient healthGradient;

    private monsterAttackSystem[] players;

    void Start()
    {
        players = playerSpawnsParent.GetComponentsInChildren<monsterAttackSystem>();

        for (int i = 0; i < players.Length; i++)
        {
            healthUI[i].gameObject.SetActive(true);
            healthUI[i].Setup(players[i], healthGradient);
        }
    }
}
