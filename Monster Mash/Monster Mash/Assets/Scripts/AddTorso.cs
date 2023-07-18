using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddTorso : MonoBehaviour
{
    private GameObject limbPrefab;
    private GameObject torsoPrefab;

    private bool canDo = true;

    // Start is called before the first frame update
    void Start()
    {
        limbPrefab = GameObject.FindGameObjectWithTag("LimbPrefab");

        torsoPrefab = GameObject.FindGameObjectWithTag("TorsoPrefab");

        //canDo = true;
    }

    private void OnMouseOver()
    {
        if (canDo)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
                //MakeNewLimb();
                //canDo = false;
            //}
            //else if (Input.GetMouseButtonDown(1))
            if (Input.GetMouseButtonDown(0))
            {
                //print("the time has come");
                MakeNewTorso();
                canDo = false;
            }
        }
    }

    void MakeNewLimb()
    {
        GameObject newLimb = Instantiate(limbPrefab);
        newLimb.transform.parent = transform;
        newLimb.transform.localPosition = Vector3.zero;
        newLimb.transform.localRotation = Quaternion.identity;
        newLimb.tag = "Untagged";
    }

    void MakeNewTorso()
    {
        GameObject newTorso = Instantiate(torsoPrefab);
        newTorso.transform.parent = transform;
        newTorso.transform.localPosition = Vector3.zero;
        newTorso.transform.rotation = Quaternion.identity;
        newTorso.tag = "Untagged";
    }
}
