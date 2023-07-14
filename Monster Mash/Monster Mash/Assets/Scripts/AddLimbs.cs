using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLimbs : MonoBehaviour
{
    [SerializeField] private GameObject TorsoPrefab;

    private GameObject torso;

    private GameObject limb;

    int index = 0;

    string myTag = "LimbSlot";

    GameObject[] limbSlots = new GameObject[0];

    GameObject[] limbs;

    bool nestedChildDone = false;

    // Start is called before the first frame update
    void Start()
    {
        torso = GameObject.FindGameObjectWithTag("Torso");

        limb = GameObject.FindGameObjectWithTag("Limb");

        if (torso.transform.childCount > 0)
        {
            NestedChildLoop(torso.transform, myTag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nestedChildDone)
        {
            PrintArray(limbSlots);

            limbs = new GameObject[limbSlots.Length];

            AddThoseLimbsBaby();

            nestedChildDone = false;
        }
    }

    private void NestedChildLoop(Transform tran, string tag)
    {
        foreach(Transform child in tran)
        {
            if (child.gameObject.CompareTag(tag))
            {
                ResizeLimbSlots(child);
                index++;
            }

            if (child.childCount > 0)
            {
                NestedChildLoop(child, tag);
            }
            else
            {
                nestedChildDone = true;
            }
        }
    }

    private void ResizeLimbSlots(Transform c)
    {
        print("array length: " + limbSlots.Length + ", int: " + index);

        System.Array.Resize<GameObject>(ref limbSlots, limbSlots.Length + 1);
        limbSlots[index] = c.gameObject;
    }

    void PrintArray(GameObject[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            print(slots[i] + ": " + slots[i].transform.position);
        }
    }

    void AddThoseLimbsBaby()
    {
        for (int i = 0; i < limbSlots.Length; i++)
        {
            if (i == 0)
            {
                limbs[i] = Instantiate(TorsoPrefab, torso.transform);
                limbs[i].transform.parent = limbSlots[i].transform;
                limbs[i].transform.localPosition = Vector3.zero;
                limbs[i].transform.localRotation = new Quaternion(-90, 90, 0, 0);
            }
            else
            {
                limbs[i] = Instantiate(limb, limb.transform);

                limbs[i].transform.parent = limbSlots[i].transform;
                limbs[i].transform.localPosition = Vector3.zero;
            }
        }
    }
}
