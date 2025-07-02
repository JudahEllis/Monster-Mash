using System.Collections;
using System.Collections.Generic;
using GameAudioScriptingEssentials;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioClipRandomizer[] groundTypeWalk;
    [SerializeField] AudioClipRandomizer[] groundTypeRun;
    [SerializeField] AudioClipRandomizer[] walkWeight;
    [SerializeField] AudioClipRandomizer[] runWeight;
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

    public void footstepSFX(NewMonsterPart part)
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
                    toPlay.Add(groundTypeWalk[3]);
                }
                else if (material.Contains("pot"))
                {
                    toPlay.Add(groundTypeWalk[2]);
                }
                else if (material.Contains("planter") || material.Contains("dirt"))
                {
                    toPlay.Add(groundTypeWalk[0]);
                }
                else if (material.Contains("can"))
                {
                    toPlay.Add(groundTypeWalk[1]);
                }
                else
                {
                    toPlay.Add(groundTypeWalk[3]);
                }
            }
            else
            {
                toPlay.Add(groundTypeWalk[3]);
            }

            // adjust later when monster weight is added
            toPlay.Add(walkWeight[0]);

            playSFX();

        }
    }

    public void runSFX(NewMonsterPart part)
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
                    toPlay.Add(groundTypeRun[3]);
                }
                else if (material.Contains("pot"))
                {
                    toPlay.Add(groundTypeRun[2]);
                }
                else if (material.Contains("planter") || material.Contains("dirt"))
                {
                    toPlay.Add(groundTypeRun[0]);
                }
                else if (material.Contains("can"))
                {
                    toPlay.Add(groundTypeRun[1]);
                }
                else
                {
                    toPlay.Add(groundTypeRun[3]);
                }
            }
            else
            {
                toPlay.Add(groundTypeRun[3]);
            }

            // adjust later when monster weight is added
            toPlay.Add(runWeight[0]);

            playSFX();

        }
    }

    public void JumpSFX(NewMonsterPart part)
    {
        if (part.isLeg)
        {
            footstepSFX(part);
        }
        toPlay.Add(baseSounds[0]);
        playSFX();
    }

    public void DoubleJumpSFX(NewMonsterPart part)
    {
        toPlay.Add(baseSounds[2]);
        playSFX();
    }

    public void DoubleJumpWingedSFX()
    {
        toPlay.Add(baseSounds[4]);
        playSFX();
    }

    public void LandSFX(NewMonsterPart part)
    {
        if (part.isLeg)
        {
            footstepSFX(part);
        }
        toPlay.Add(landWeight[0]);
        toPlay.Add(baseSounds[1]);
        playSFX();
    }

    public void DashSFX()
    {
        toPlay.Add(baseSounds[3]);
        playSFX();
    }
}
