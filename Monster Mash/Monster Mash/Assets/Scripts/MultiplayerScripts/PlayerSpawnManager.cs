using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawnLocations;

    [SerializeField]
    private Transform players;
    void Awake()
    {
       SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnPlayers()
    {
        CharacterSelectManager transfer = FindObjectOfType<CharacterSelectManager>();


        GameObject playerPrefab = Resources.Load("Fun-Test Parts/Monster/Player") as GameObject;

        GameObject monsterEmpty = Resources.Load("Fun-Test Parts/Monster/MonsterEmpty") as GameObject;

        for (int i = 0; i < transfer.storedPlayerInformation.Count; i++)
        {
            GameObject spawnedPlayer = Instantiate(playerPrefab, playerSpawnLocations[i].position, Quaternion.identity);

            spawnedPlayer.name = ("Player_" + (transfer.storedPlayerInformation[i].playerIndex + 1));

            spawnedPlayer.GetComponent<input_handler>().playerInput = transfer.storedPlayerInformation[i].playerInput.GetComponent<PlayerInput>();

            spawnedPlayer.transform.parent = players;

            Transform monsterPartStorage = spawnedPlayer.transform.GetChild(1);

            GameObject emptyMonster = Instantiate(monsterEmpty, monsterPartStorage);

            Transform monsterPartVisualsSpawn = emptyMonster.transform.GetChild(0).GetChild(0);

            monsterAttackSystem monsterControl = emptyMonster.GetComponent<monsterAttackSystem>();

            foreach (MonsterPartData partData in transfer.storedPlayerInformation[i].monster.monsterParts)
            {
                GameObject partPrefab = Resources.Load(partData.partPrefabPath) as GameObject;

                GameObject spawnedPart = Instantiate(partPrefab, monsterPartVisualsSpawn);

                spawnedPart.transform.localPosition = partData.partPosition;

                spawnedPart.transform.localRotation = partData.partRotation;

                spawnedPart.transform.localScale = partData.partScale;
            }

            monsterControl.turnOnLimbConnectors();

            StartCoroutine(Delay(monsterControl));
        }
    }

    IEnumerator Delay(monsterAttackSystem monsterControl)
    {
        yield return new WaitForSeconds(0.1f);

        monsterControl.turnOffLimbConnectors();

        monsterControl.connectCurrentLimbs();

        monsterControl.awakenTheBeast();
    }
}
