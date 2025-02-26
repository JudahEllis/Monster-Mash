using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveToFront : MonoBehaviour
{

    public void Move()
    {
        Transform p = transform.parent;
        transform.SetParent(null, false);
        transform.SetParent(p, false);
    }
}
