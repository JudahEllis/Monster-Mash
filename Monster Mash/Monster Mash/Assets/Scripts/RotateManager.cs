using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateManager : MonoBehaviour
{
    [SerializeField] private Slider xSlid;
    [SerializeField] private Slider ySlid;
    [SerializeField] private Slider zSlid;

    [SerializeField] private GameObject sliderParent;

    private GameObject currObj;
    private bool hasTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        sliderParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initiate(GameObject obj)
    {
        hasTarget = true;
        sliderParent.SetActive(true);

        currObj = obj;

        xSlid.value = obj.transform.rotation.x;
        ySlid.value = obj.transform.rotation.y;
        zSlid.value = obj.transform.rotation.z;
    }

    public void Stop()
    {
        sliderParent.SetActive(false);
    }

    public void RotateX()
    {
        //ResetRotSliders();
        currObj.transform.rotation = Quaternion.Euler(xSlid.value, currObj.transform.rotation.y, currObj.transform.rotation.z);
    }

    public void RotateY()
    {
        //ResetRotSliders();
        currObj.transform.rotation = Quaternion.Euler(currObj.transform.rotation.x, ySlid.value, currObj.transform.rotation.z);
    }

    public void RotateZ()
    {
        //ResetRotSliders();
        currObj.transform.rotation = Quaternion.Euler(currObj.transform.rotation.x, currObj.transform.rotation.y, zSlid.value);
    }

    private void ResetRotSliders()
    {
        xSlid.value = currObj.transform.rotation.x;
        ySlid.value = currObj.transform.rotation.y;
        zSlid.value = currObj.transform.rotation.z;
    }
}
