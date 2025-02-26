using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPartGrid : MonoBehaviour
{
    bool setupDone;
    public RectTransform rect;
    List<GameObject> parts;

    private void Awake()
    {
        parts = new List<GameObject>();
        foreach (Transform part in transform.GetComponentInChildren<Transform>())
        {
            parts.Add(part.gameObject);
        }
    }

    IEnumerator Start()
    {
        if (!setupDone)
        {
            yield return new WaitForEndOfFrame();
            Resize();
            setupDone = true;
        }
    }

    private void OnEnable()
    {
        JostlePartIcons();
    }
    void JostlePartIcons()
    {
        foreach(GameObject part in parts)
        {
            part.GetComponent<Animator>().SetTrigger("Jostle");
        }
    }
    

   

    // adjust Rect height according to grid contents
    void Resize()
    {
        // calculate and set new height by getting the
        // difference between first and last child y positions
        float first = parts[0].GetComponent<RectTransform>().position.y;
        float last = parts[parts.Count - 1].GetComponent<RectTransform>().position.y;
        float difference = Mathf.Abs(last - first) + GetComponent<GridLayoutGroup>().cellSize.y;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, difference + 60f);
    }

}
