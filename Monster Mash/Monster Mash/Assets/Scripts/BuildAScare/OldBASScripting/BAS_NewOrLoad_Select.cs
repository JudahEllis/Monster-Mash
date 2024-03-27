using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BAS_NewOrLoad_Select : MonoBehaviour
{
    [SerializeField] Button _new_monster;


    // Start is called before the first frame update
    void Start()
    {
        _new_monster.onClick.AddListener(LoadBAS);
    }

    private void LoadBAS()
    {
        //ScenesManager.Instance.LoadScene(ScenesManager.Scene.BuildAScare);
        ScenesManager.Instance.LoadNewGame();
    }
}
