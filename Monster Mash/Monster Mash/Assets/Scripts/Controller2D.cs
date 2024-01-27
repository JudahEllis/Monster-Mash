using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller2D : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float playerSpeed = 8.0f;
    [SerializeField] private float playerRunSpeed = 9.33f;
    [SerializeField] private float decelerate = 10f;
    [SerializeField] private float jumpForce = 8.0f;
    private bool grounded;
    private CapsuleCollider2D cap;
    private bool wall;
    [SerializeField] private float move;
    private float fallMultiplier = -1f;

    private Vector3 xMovement;
    private Vector3 yMovement;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private bool isRun = false;
    [SerializeField] private bool isSkid = false;
    private float skidTime = 0.25f;


    public KeyCode keyToDetect1 = KeyCode.A;
    public KeyCode keyToDetect2 = KeyCode.D;
    public float doubleTapTimeThreshold = 0.3f; // Adjust as needed

    private bool key1WasPressed;
    private bool key2WasPressed;
    private float timeSinceLastPress1;
    private float timeSinceLastPress2;
    [SerializeField] private int key1result;
    [SerializeField] private int key2result;

    //ui
    [SerializeField] Slider walkButton;
    [SerializeField] TMP_Text walkText;
    [SerializeField] Slider runButton;
    [SerializeField] TMP_Text runText;
    [SerializeField] Slider skidButton;
    [SerializeField] TMP_Text skidText;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cap = GetComponent<CapsuleCollider2D>();

        playerRunSpeed = playerSpeed * (13f / 10f);

        walkButton.value = playerSpeed;
        runButton.value = playerRunSpeed;
        skidButton.value = skidTime;
    }

    private void Update()
    {
        playerSpeed = walkButton.value;
        playerRunSpeed = runButton.value;
        skidTime = skidButton.value;

        walkText.text = playerSpeed.ToString();
        runText.text = playerRunSpeed.ToString();
        skidText.text = skidTime.ToString();

        if (Input.GetKeyDown(keyToDetect1))
        {
            if (key1WasPressed && Time.time - timeSinceLastPress1 < doubleTapTimeThreshold)
            {
                // Key was double-tapped
                Debug.Log("Key double-tapped");
                key1result = 2;

                key1WasPressed = false;
            }
            else
            {
                // Key was single-tapped
                Debug.Log("Key single-tapped");
                key1result = 1;

                key1WasPressed = true;
                timeSinceLastPress1 = Time.time;
            }
        }

        if (Input.GetKeyDown(keyToDetect2))
        {
            if (key2WasPressed && Time.time - timeSinceLastPress2 < doubleTapTimeThreshold)
            {
                // Key was double-tapped
                Debug.Log("Key double-tapped");
                key2result = 2;

                key2WasPressed = false;
            }
            else
            {
                // Key was single-tapped
                Debug.Log("Key single-tapped");
                key2result = 1;

                key2WasPressed = true;
                timeSinceLastPress2 = Time.time;
            }
        }

        MoveX();

        if (!grounded)
        {
            //rb.velocity += new Vector2(0, rb.gravityScale * fallMultiplier);
        }
    }

    private void FixedUpdate()
    {
        // Check for grounded state (you can use raycasting or other methods).
        //IsGrounded();

        // Jumping.
        if (Input.GetKeyDown(KeyCode.Space))// && grounded)
        {
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        ApplyMove();
    }

    private void IsGrounded()
    {
        if (Physics2D.CapsuleCast(transform.position, cap.size * 0.75f, cap.direction, 0, Vector2.down, 0.1f, groundLayerMask))
        {
            grounded = true;
        }

        //print("NOO");
        grounded = false;
    }

    private bool HitWall(float direction)
    {
        if (Physics2D.CapsuleCast(transform.position, new Vector2(cap.size.x, cap.size.y * 0.5f), cap.direction, 0, Vector2.right * direction, 0.1f, groundLayerMask))
        {
            return true;
        }

        return false;
    }

    private void MoveX()
    {
        // Horizontal movement.
        move = Input.GetAxis("Horizontal");

        if (!HitWall(move))
        {
            if ((key1result == 2 && move < 0) || (key2result == 2 && move > 0))//(Input.GetKey(KeyCode.LeftShift))
            {
                isRun = true;
                xMovement = new Vector3(move * playerRunSpeed, 0.0f, 0.0f);
            }
            else
            {
                isRun = false;
                xMovement = new Vector3(move * playerSpeed, 0.0f, 0.0f);
            }
        }
    }

    private void MoveY()
    {

    }

    private void ApplyMove()
    {
        if (!isSkid)
        {
            if ((isRun && (move < 0 && rb.velocity.x > 0) || (move > 0 && rb.velocity.x < 0)))
            {
                //skid
                //rb.AddForce(new Vector2(-rb.velocity.x * decelerate, 0f), ForceMode2D.Force);
                StartCoroutine("SkidToStop");
            }
            else
            {
                rb.velocity = xMovement + new Vector3(0.0f, rb.velocity.y, 0.0f);//yMovement;
            }
        }
    }

    private IEnumerator SkidToStop()
    {
        isSkid = true;

        rb.velocity = new Vector2(rb.velocity.x * 0.75f, rb.velocity.y);

        yield return new WaitForSeconds(skidTime/4f);

        rb.velocity = new Vector2(rb.velocity.x * 0.75f, rb.velocity.y);

        yield return new WaitForSeconds(skidTime / 4f);

        rb.velocity = new Vector2(rb.velocity.x * 0.75f, rb.velocity.y);

        yield return new WaitForSeconds(skidTime / 4f);

        rb.velocity = new Vector2(0, rb.velocity.y);

        yield return new WaitForSeconds(skidTime / 4f);

        isSkid = false;

        yield break;
    }
}