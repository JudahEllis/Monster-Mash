using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public List<MultiplayerJoinManager.PlayerInformation> storedPlayerInformation;

    public static CharacterSelectManager Instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);


        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
}
