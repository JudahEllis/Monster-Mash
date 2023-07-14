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

    private int xRot;
    private int yRot;
    private int zRot;

    private GameObject currTorso;
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

    public void NewRotObj(GameObject obj)
    {
        hasTarget = true;
        sliderParent.SetActive(true);

        currTorso = obj;

        xRot = Mathf.RoundToInt(obj.transform.rotation.x);
        yRot = Mathf.RoundToInt(obj.transform.rotation.y);
        zRot = Mathf.RoundToInt(obj.transform.rotation.z);

        xSlid.value = xRot;
        ySlid.value = yRot;
        zSlid.value = zRot;
    }

    public void RotateX()
    {
        currTorso.transform.rotation = Quaternion.Euler(xSlid.value, currTorso.transform.rotation.y, currTorso.transform.rotation.z);
    }

    public void RotateY()
    {
        currTorso.transform.rotation = Quaternion.Euler(currTorso.transform.rotation.x, ySlid.value, currTorso.transform.rotation.z);
    }

    public void RotateZ()
    {
        currTorso.transform.rotation = Quaternion.Euler(currTorso.transform.rotation.x, currTorso.transform.rotation.y, zSlid.value);
    }
}
