using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformManager : MonoBehaviour
{
    [SerializeField] private GameObject uiParent;
    //[SerializeField] private GameObject RotateUIParent;
    //[SerializeField] private GameObject MoveUIParent;

    private RotateManager rotateManager;
    private MoveManager moveManager;

    [SerializeField] private Slider choose; //value 0 = move, 1 = rotate

    private GameObject currObj;

    // Start is called before the first frame update
    void Start()
    {
        uiParent.SetActive(false);
        //RotateUIParent.SetActive(false);
        //MoveUIParent.SetActive(false);

        rotateManager = FindObjectOfType<RotateManager>();
        moveManager = FindObjectOfType<MoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NewRotObj(GameObject obj)
    {
        uiParent.SetActive(true);

        currObj = obj;

        Change();
    }

    public void Change() //can call from slider change value
    {
        if (choose.value < 1) //move object
        {
            Move();
        }
        else //rot object
        {
            Rotate();
        }
    }

    private void Move()
    {
        rotateManager.Stop();
        moveManager.Initiate(currObj);
    }

    private void Rotate()
    {
        moveManager.Stop();
        rotateManager.Initiate(currObj);
    }

    public void TurnOffUI()
    {
        rotateManager.Stop();
        moveManager.Stop();
        uiParent.SetActive(false);
    }
}
