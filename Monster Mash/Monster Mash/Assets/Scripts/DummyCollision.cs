using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCollision : MonoBehaviour
{
    [SerializeField] private bool isCollide = false;

    [SerializeField] private LayerMask mask;

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * transform.localScale.y / 2 + new Vector3(0, -1, 0), Color.red);
        Collide();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collide();
    }

    private void Collide()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, transform.localScale.y / 2 + 1f, mask))
        {
            isCollide = true;
        } else
        {
            isCollide = false;
        }
    }

    public bool GetCollide()
    {
        return isCollide;
    }

    public void ResetCollide()
    {
        isCollide = false;
    }
}
