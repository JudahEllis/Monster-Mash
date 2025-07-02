using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartFlipped : MonoBehaviour, IPartAdjustable
{
    [SerializeField]
    Material flippedMaterial;

    [SerializeField]
    MeshRenderer rend;

    public enum FlipType { Material, Collider, ParticleEffect}

    [SerializeField]
    FlipType[] partFlip;

    [SerializeField]
    Collider[] flipColliders;

    [SerializeField]
    ParticleSystem[] flipParticle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPartAdjustable.PartAdjustment(MonsterPartData partRef)
    {
        //Very temp. may need to be an array
        if(partRef.isFlipped)
        {
           foreach(FlipType type in partFlip)
            {
                switch(type)
                {
                    case FlipType.Material:

                        rend.material = flippedMaterial;

                        break;

                    case FlipType.Collider:

                        foreach (BoxCollider col in flipColliders)
                        {
                            col.gameObject.transform.localScale = new Vector3(col.gameObject.transform.localScale.x * -1,
                                col.gameObject.transform.localScale.y,
                                col.gameObject.transform.localScale.z);
                        }

                        break;

                    case FlipType.ParticleEffect:

                        foreach(ParticleSystem system in flipParticle)
                        {
                            system.gameObject.transform.localScale = new Vector3(system.gameObject.transform.localScale.x * -1,
                                system.gameObject.transform.localScale.y,
                                system.gameObject.transform.localScale.z);
                        }

                        break;
                }
            }
        }
    }
}
