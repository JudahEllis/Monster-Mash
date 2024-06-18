using System.Collections;
using System.Collections.Generic;
using GameAudioScriptingEssentials;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioClipRandomizer[] groundType;
    [SerializeField] AudioClipRandomizer[] weight;

    public void footstepSFX(monsterPart part)
    {
        if (part.isLeg)
        {
            RaycastHit hit;
            List<AudioClipRandomizer> toPlay = new List<AudioClipRandomizer>();
            if (Physics.Raycast(part.transform.position, Vector3.down, out hit))
            {
                MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();
                string material = meshFilter.ToString();
                if (material.Contains("table"))
                {
                    toPlay.Add(groundType[3]);
                }
                else if (material.Contains("pot"))
                {
                    toPlay.Add(groundType[2]);
                }
                else if (material.Contains("planter") || material.Contains("dirt"))
                {
                    toPlay.Add(groundType[0]);
                }
                else if (material.Contains("can"))
                {
                    toPlay.Add(groundType[1]);
                }
                else
                {
                    toPlay.Add(groundType[3]);
                }
            }
            else
            {
                toPlay.Add(groundType[3]);
            }

            // adjust later when monster weight is added
            toPlay.Add(weight[0]);

            foreach (AudioClipRandomizer clip in toPlay)
            {
                clip.PlaySFX();
            }

        }
    }
}
