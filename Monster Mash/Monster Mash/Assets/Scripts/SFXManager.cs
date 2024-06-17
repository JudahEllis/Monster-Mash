using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioSource[] clips;

    public void footstepSFX(monsterPart part)
    {
        if (part.isLeg)
        {
            Debug.Log("Play Footstep SFX");
            RaycastHit hit;
            if (Physics.Raycast(part.transform.position, Vector3.down, out hit))
            {
                MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();
                string material = meshFilter.ToString();
                if (material.Contains("table"))
                {
                    Debug.Log("wood");
                    clips[3].Play();
                }
                else if (material.Contains("pot"))
                {
                    Debug.Log("pot");
                    clips[2].Play();
                }
                else if (material.Contains("planter") || material.Contains("dirt"))
                {
                    Debug.Log("dirt");
                    clips[0].Play();
                }
                else if (material.Contains("can"))
                {
                    Debug.Log("metal");
                    clips[1].Play();
                }
                else
                {
                    clips[3].Play();
                    Debug.Log("wood");
                }

            }
        }
    }
}
