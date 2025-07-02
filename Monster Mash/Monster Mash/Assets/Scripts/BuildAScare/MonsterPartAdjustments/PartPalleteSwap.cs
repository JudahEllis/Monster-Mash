using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPalleteSwap : MonoBehaviour, IPartAdjustable
{
    [System.Serializable]
    public class PalleteSwap
    {
        public SkinnedMeshRenderer rend;

        public Material[] partMaterials;
    }

    [SerializeField]
    PalleteSwap[] partSwaps;
    
    void Start()
    {
        
    }

    void IPartAdjustable.PartAdjustment(MonsterPartData partRef)
    {
        if(partRef.palleteSwapIndex.Count > 0)
        {
            for(int i = 0; i < partRef.palleteSwapIndex.Count; i++)
            {
                partSwaps[i].rend.material = partSwaps[i].partMaterials[partRef.palleteSwapIndex[i]];
            }
        }
    }
}
