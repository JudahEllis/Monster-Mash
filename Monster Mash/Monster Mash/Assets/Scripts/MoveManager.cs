using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveManager : MonoBehaviour
{
    [SerializeField] private GameObject sliderParent;

    [SerializeField] private Slider xSlid;
    [SerializeField] private Slider ySlid;
    [SerializeField] private Slider zSlid;

    private float maxMag = 1f;
    private float minMag = -1;

    private GameObject currObj;

    // Start is called before the first frame update
    void Start()
    {
        sliderParent.SetActive(false);
    }

    public void Initiate(GameObject obj)
    {
        sliderParent.SetActive(true);

        currObj = obj;
    }

    public void Stop()
    {
        sliderParent.SetActive(false);
    }

    public void XMove()
    {
        currObj.transform.position = new Vector3(xSlid.value, currObj.transform.position.y, currObj.transform.position.z);
    }

    public void YMove()
    {
        currObj.transform.position = new Vector3(currObj.transform.position.x, ySlid.value, currObj.transform.position.z);
    }

    public void ZMove()
    {
        currObj.transform.position = new Vector3(currObj.transform.position.x, currObj.transform.position.y, zSlid.value);
    }

    private void ResetmoveSliders()
    {
        xSlid.value = currObj.transform.position.x;
        ySlid.value = currObj.transform.position.y;
        zSlid.value = currObj.transform.position.z;
    }
}
