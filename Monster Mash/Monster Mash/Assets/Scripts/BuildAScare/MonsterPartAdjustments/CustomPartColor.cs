using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPartColor : MonoBehaviour, IPartAdjustable
{
    Color partColor;

    [SerializeField]
    SkinnedMeshRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
    }

    void IPartAdjustable.PartAdjustment(MonsterPartData partRef)
    { 
        ColorUtility.TryParseHtmlString(partRef.partHexCode, out partColor);

        rend.material.SetColor("_BaseColor", partColor);
    }
}
