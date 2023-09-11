using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnConveyor : MonoBehaviour
{
    Rigidbody rb;

    Vector3 myVelocity;

    bool onBelt = false;

    float thineOwnSpeed = 750f;

    [SerializeField] private ConveyorBelt belt;

    private Vector3 currentVelocity;

    private void Awake()
    {
        onBelt = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //inputArea = targetCamera.rect;
    }

    // Update is called once per frame
    void Update()
    {
        if (onBelt)
        {
            //print("the pee whoo pooed");
            rb.velocity = myVelocity * Time.deltaTime * thineOwnSpeed;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<ConveyorBelt>())
        {
            onBelt = true;
            //print("convey!");

            //ConveyorBelt belt = collision.gameObject.GetComponent<ConveyorBelt>();

            currentVelocity = belt.GetDirection() * belt.GetSpeed();

            Move(currentVelocity);
        }
        else if (collision.gameObject.GetComponent<OnConveyor>())
        {
            //OnConveyor convey = collision.gameObject.GetComponent<OnConveyor>();

            //onBelt = true;

            //currentVelocity = convey.GetCurrentVelocity();
            //Move(currentVelocity);
        }
    }

    /*private void OnMouseDown()
    {
        belt.SetSelection(this.gameObject);
    }*/

    public void ClickedOn(CursorConveyorSelection myCursor)
    {
        myCursor.SetSelection(this.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<ConveyorBelt>())
        {
            onBelt = false;
        }
    }

    private void Move(Vector3 dir)
    {
        //rb.AddForce(dir * speed, ForceMode.VelocityChange);

        myVelocity = dir;
    }

    public Vector3 GetCurrentVelocity()
    {
        return currentVelocity;
    }

    public void NotOnBelt()
    {
        onBelt = false;
    }
}
