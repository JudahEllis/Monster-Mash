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
    private float jumpTime = 1f;
    private int midAirJumps = 1;
    private int jumpsLeft = 0;
    private bool cannotJumpCuzMidJump = false;
    [SerializeField] private bool grounded;
    private CapsuleCollider2D cap;
    private bool wall;
    [SerializeField] private float move;
    [SerializeField] private float fallMultiplier = 3f;

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
    [SerializeField] Slider forceButton;
    [SerializeField] TMP_Text forceText;
    [SerializeField] Slider gravityButton;
    [SerializeField] TMP_Text gravityText;
    [SerializeField] Slider fallButton;
    [SerializeField] TMP_Text fallText;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cap = GetComponent<CapsuleCollider2D>();

        playerRunSpeed = playerSpeed * (13f / 10f);

        jumpForce = 21.6f;
        rb.gravityScale = 6.28f;
        fallMultiplier = 4f;

        forceButton.value = jumpForce;
        gravityButton.value = rb.gravityScale;
        fallButton.value = fallMultiplier;
    }

    private void Update()
    {
        jumpForce = forceButton.value;
        rb.gravityScale = gravityButton.value;
        fallMultiplier = fallButton.value;

        forceText.text = jumpForce.ToString();
        gravityText.text = rb.gravityScale.ToString();
        fallText.text = fallMultiplier.ToString();

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

        if (cannotJumpCuzMidJump && Input.GetKeyUp(KeyCode.Space))
        {
            cannotJumpCuzMidJump = false;
        }

        //MoveX();

        //MoveY();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsLeft = midAirJumps;
            } else if (jumpsLeft > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsLeft--;
            }
        }

        if (rb.velocity.y < 0.0f)
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                rb.velocity -= -Physics2D.gravity * fallMultiplier * 5 * Time.deltaTime;
            }
            else
            {
                rb.velocity -= -Physics2D.gravity * fallMultiplier * Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        // Check for grounded state (you can use raycasting or other methods).
        //IsGrounded();

        // Jumping.
        /*if (Input.GetKeyDown(KeyCode.Space))// && grounded)
        {
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }*/

        IsGrounded();

        ApplyMove();
    }

    private void IsGrounded()
    {
        //if (Physics2D.CapsuleCast(transform.position, cap.size * 0.75f, cap.direction, 0, Vector2.down, 0.1f, groundLayerMask))
        if (Physics2D.BoxCast(new Vector2(transform.position.x, cap.bounds.min.y), new Vector2(cap.bounds.size.x, 0.25f), 0, Vector2.down, 0.1f, groundLayerMask))
        {
            grounded = true;
        } else
        {
            grounded = false;
        }
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

        //disable move
        //move = 0;

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

        /*if (Input.GetKey(KeyCode.Space) && !cannotJumpCuzMidJump)
        {
            if (grounded)
            {
                yMovement = Vector3.up * jumpForce;
                jumpsLeft = midAirJumps;
                cannotJumpCuzMidJump = true;
            }
            else if (jumpsLeft > 0)
            {
                yMovement = Vector3.up * jumpForce;
                jumpsLeft--;
                cannotJumpCuzMidJump = true;
            }
        }*/
        //else
        {
            yMovement = Vector3.zero;
        }
    }

    private void ApplyMove()
    {
        if (!isSkid) //x movement
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

        if (yMovement.y > 0.0f)
        {
            //rb.velocity = new Vector3(rb.velocity.x, 0.0f, 0.0f) + yMovement;
        }

        if (rb.velocity.y < 0.0f)
        {
            //rb.velocity -= Physics2D.gravity * fallMultiplier * Time.deltaTime;
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