using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartSelector : MonoBehaviour
{
    public int defaultActiveCategory;

    public ScrollRect scrollRect;
    public RectTransform divider_top;
    public RectTransform divider_bottom;
    public Transform conveyor;
    public Transform partsParent;
    public GameObject[] partsGrids;
    int activeCategory;
    public Animation switchTabAnimation;
    public Transform switchTabAnimationParent;
    public Transform tabButtonParent;
    public List<Toggle> tabButtons;

    void Start()
    {
    }


    public void EnableCategory(int categoryNumber)
    {
        if (!partsGrids[categoryNumber].transform.parent.gameObject.activeSelf)
        {
            activeCategory = categoryNumber;
            partsGrids[categoryNumber].transform.parent.gameObject.SetActive(true);
            switchTabAnimationParent.position = conveyor.position;
            partsGrids[categoryNumber].transform.parent.transform.SetParent(switchTabAnimationParent);
            
            switchTabAnimation.Play();
        }
    }

    public void AnimationDone()
    {
        foreach (Transform category in partsParent.GetComponentInChildren<Transform>())
        {
            category.gameObject.SetActive(false);
        }
        switchTabAnimationParent.GetChild(0).SetParent(partsParent);
        scrollRect.content = partsGrids[activeCategory].transform.parent.GetComponent<RectTransform>();

        conveyor.SetParent(scrollRect.content);
        conveyor.SetAsFirstSibling();

        // reposition and reparent dividers
        divider_top.SetParent(scrollRect.content);
        divider_top.localPosition = new Vector2 (divider_top.localPosition.x, 0f);
        divider_bottom.SetParent(scrollRect.content);
        float new_y = 0 - scrollRect.content.sizeDelta.y + 60f;
        if (new_y > Screen.height * -1f) { new_y = Screen.height * -1f;}
        divider_bottom.localPosition = new Vector2(divider_bottom.localPosition.x, new_y);

        partsGrids[activeCategory].GetComponent<CanvasGroup>().interactable = true;
    }
}
