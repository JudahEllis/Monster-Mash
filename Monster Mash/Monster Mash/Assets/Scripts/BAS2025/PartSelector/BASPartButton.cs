using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BASPartButton : MonoBehaviour
{
    public string monsterPartReference;

    public bool isTorso;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPart()
    {

        GameObject prefabRef = Resources.Load(monsterPartReference) as GameObject;

        BuildAScareManager.instance.SpawnPart(isTorso, prefabRef);
    }    

    public void CheckIndex()
    {
        //grid.CheckScrollPage(gameObject);
    }
}
