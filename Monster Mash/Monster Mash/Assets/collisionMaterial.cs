using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionMaterial : MonoBehaviour
{
    public ParticleSystem[] VFX;
    public float yAxisAdjustment = 0;
    private int VFXCount = 0;
    public void spawnVFX(Vector3 spawnCooridinates)
    {
        VFX[VFXCount].Stop();
        if (VFXCount < VFX.Length - 1)
        {
            VFXCount++;
        }
        else
        {
            VFXCount = 0;
        }
        VFX[VFXCount].gameObject.transform.position = new Vector3(spawnCooridinates.x, spawnCooridinates.y + yAxisAdjustment, spawnCooridinates.z);
        VFX[VFXCount].Play();
    }
}
