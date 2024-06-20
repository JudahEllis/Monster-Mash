using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunTestManager : MonoBehaviour
{
    public List<MonsterData> playerMonsters;

    public Transform[] playerPartParents;

    MonsterTransfer transferData;

    void Start()
    {
        CompleteMonsters();

        transferData = FindObjectOfType<MonsterTransfer>();
    }

    void CompleteMonsters()
    {
        for(int i = 0; i < playerPartParents.Length; i++)
        {
            foreach(Transform child in playerPartParents[i])
            {
                child.GetComponent<TempPartData>().SavePartData();

                playerMonsters[i].monsterParts.Add(child.GetComponent<TempPartData>().monsterPart);
            }
        }
    }

    public void StartMatch()
    {
        transferData.selectedMonsters = playerMonsters;

        SceneManager.LoadScene(0);
    }

}
