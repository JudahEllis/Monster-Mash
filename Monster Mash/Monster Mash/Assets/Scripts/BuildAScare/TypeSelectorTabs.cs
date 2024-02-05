using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class TypeSelectorTabs : MonoBehaviour
{
    [SerializeField] Button _arms, _legs, _heads, _other;

    [SerializeField] GameObject _armsScroll, _legsScroll, _headScroll, _otherScroll;

    public enum Type
    {
        Arms,
        Legs,
        Heads,
        Other
    }

    // Start is called before the first frame update
    void Start()
    {
        _arms.onClick.AddListener(() => ChangeScrollView(Type.Arms));
        _legs.onClick.AddListener(() => ChangeScrollView(Type.Legs));
        _heads.onClick.AddListener(() => ChangeScrollView(Type.Heads));
        _other.onClick.AddListener(() => ChangeScrollView(Type.Other));
    }

    void ChangeScrollView(Type type)
    {
        switch (type)
        {
            case Type.Arms:
                _legsScroll.gameObject.SetActive(false);
                _headScroll.gameObject.SetActive(false);
                _otherScroll.gameObject.SetActive(false);
                _armsScroll.gameObject.SetActive(true);
                break;
            case Type.Legs:
                _armsScroll.gameObject.SetActive(false);
                _headScroll.gameObject.SetActive(false);
                _otherScroll.gameObject.SetActive(false);
                _legsScroll.gameObject.SetActive(true);
                break;
            case Type.Heads:
                _armsScroll.gameObject.SetActive(false);
                _legsScroll.gameObject.SetActive(false);
                _otherScroll.gameObject.SetActive(false);
                _headScroll.gameObject.SetActive(true);
                break;
            case Type.Other:
                _armsScroll.gameObject.SetActive(false);
                _legsScroll.gameObject.SetActive(false);
                _headScroll.gameObject.SetActive(false);
                _otherScroll.gameObject.SetActive(true);
                break;
        }
    }


}
