using System.Collections;
using System.Collections.Generic;
using GameAudioScriptingEssentials;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioClipRandomizer[] groundType;
    [SerializeField] AudioClipRandomizer[] weight;
    [SerializeField] AudioClipRandomizer[] landWeight;
    [SerializeField] AudioClipRandomizer[] baseSounds;
    private List<AudioClipRandomizer> toPlay = new List<AudioClipRandomizer>();

    private void playSFX()
    {
        foreach (AudioClipRandomizer clip in toPlay)
        {
            clip.PlaySFX();
        }
        toPlay.Clear();
    }

    public void footstepSFX(monsterPart part)
    {
        if (part.isLeg)
        {
            RaycastHit hit;
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

            playSFX();

        }
    }

    public void JumpSFX(monsterPart part)
    {
        if (part.isLeg)
        {
            footstepSFX(part);
        }
        toPlay.Add(baseSounds[0]);
        playSFX();
    }

    public void DoubleJumpSFX(monsterPart part)
    {
        toPlay.Add(baseSounds[2]);
        playSFX();
    }

    public void LandSFX(monsterPart part)
    {
        if (part.isLeg)
        {
            footstepSFX(part);
        }
        toPlay.Add(landWeight[0]);
        toPlay.Add(baseSounds[1]);
        playSFX();
    }
}
