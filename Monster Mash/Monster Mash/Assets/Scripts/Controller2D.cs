using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller2D : MonoBehaviour
{
    private bool facingRight = true;
    private Rigidbody2D rb;
    private float playerSpeed = 12.0f;
    private float playerRunSpeed;
    private float playerAirSpeed = 11.0f;
    private float playerAirBackwardSpeed = 4.0f;
    private float decelerate = 1f;
    private float jumpForce = 10f;
    private float midAirJumpForce = 18f;
    private float jumpCounter;
    private float jumpTime = 0.2f;
    private bool isJumping = false; //for grounded jump, longer hold = higher jump
    private float jumpMultiplier = 14f;
    private float fastFallSpeed = 3f;
    private float airDrag;
    private float fastAirDrag = 0.95f;
    private float slowAirDrag = 0.65f;
    private int midAirJumps = 1;
    private int jumpsLeft = 0;
    private bool cannotJumpCuzMidJump = false;
    private bool grounded;
    private CapsuleCollider2D cap;
    private bool wall;
    private int move;
    private float fallMultiplier = 3f;

    private Vector3 xMovement;
    private Vector3 yMovement;

    [SerializeField] private LayerMask groundLayerMask;

    private bool isRun = false;
    private bool isSkid = false;
    private float skidTime = 0f;


    public KeyCode keyToDetect1 = KeyCode.A;
    public KeyCode keyToDetect2 = KeyCode.D;
    public float doubleTapTimeThreshold = 0.3f; // Adjust as needed

    private bool key1WasPressed;
    private bool key2WasPressed;
    private float timeSinceLastPress1;
    private float timeSinceLastPress2;
    private int key1result;
    private int key2result;

    [SerializeField] private Transform playerNose;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cap = GetComponent<CapsuleCollider2D>();

        playerRunSpeed = playerSpeed * (13f / 10f);

        //jumpForce = 21.6f;
        rb.gravityScale = 4f;//6.28f;
        fallMultiplier = 4f;
    }

    private void Update()
    {
        if (facingRight && playerNose != null)
        {
            playerNose.localPosition = new Vector2(0.5f, playerNose.localPosition.y);
        } else if (playerNose != null)
        {
            playerNose.localPosition = new Vector2(-0.5f, playerNose.localPosition.y);
        }

        if (Input.GetKeyDown(keyToDetect1) && !Input.GetKey(keyToDetect2))
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

        if (Input.GetKeyDown(keyToDetect2) && !Input.GetKey(keyToDetect1))
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

        MoveX();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                if (isSkid)
                {
                    StopCoroutine("SkidToStop");
                    isSkid = false;
                }

                if (move != 0)
                {
                    airDrag = fastAirDrag;
                } else
                {
                    airDrag = slowAirDrag;
                }

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsLeft = midAirJumps;
                isJumping = true;
                print("isJump true");
                jumpCounter = 0;

            } else if (jumpsLeft > 0)
            {
                if (isSkid)
                {
                    StopCoroutine("SkidToStop");
                    isSkid = false;
                }

                if (move != 0)
                {
                    airDrag = fastAirDrag;
                }
                else
                {
                    airDrag = slowAirDrag;
                }

                rb.velocity = new Vector2(rb.velocity.x, midAirJumpForce);
                jumpsLeft--;

                if (move == 1)
                {
                    facingRight = true;
                } else if (move == -1)
                {
                    facingRight = false;
                }
            }
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) { isJumping = false; print("isJump false"); }

            float t = jumpCounter / jumpTime;

            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }

            rb.velocity = new Vector2(rb.velocity.x, jumpMultiplier);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isJumping)
        {
            print("isJump false");
            isJumping = false;
        }

        if (rb.velocity.y < 0.0f)
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                rb.velocity -= -Physics2D.gravity * fallMultiplier * fastFallSpeed * Time.deltaTime;
            }
            else
            {
                rb.velocity -= -Physics2D.gravity * fallMultiplier * Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        IsGrounded();

        ApplyMove();
    }

    private void IsGrounded()
    {
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
        if (Input.GetKey(KeyCode.D))
        {
            move = 1;
        } else if (Input.GetKey(KeyCode.A))
        {
            move = -1;
        } else
        {
            move = 0;
        }

        //disable move
        //move = 0;

        if (!HitWall(move))
        {
            if (grounded)
            {
                if ((key1result == 2 && move < 0) || (key2result == 2 && move > 0))
                {
                    isRun = true;
                    xMovement = new Vector3(move * playerRunSpeed, 0.0f, 0.0f);
                }
                else
                {
                    isRun = false;
                    xMovement = new Vector3(move * playerSpeed, 0.0f, 0.0f);
                }
            } else
            {
                if (move == 0 && Mathf.Approximately(rb.velocity.x, 0.0f)) // Jumping in place
                {
                    airDrag = slowAirDrag;
                }
                else if (facingRight)
                {
                    if (move > 0)
                    {
                        xMovement = new Vector3(playerAirSpeed, 0.0f, 0.0f);
                    } else if (move == 0)
                    {
                        xMovement = new Vector3(rb.velocity.x * airDrag, 0.0f, 0.0f);
                    } else if (move < 0)
                    {
                        xMovement = new Vector3(-playerAirBackwardSpeed, 0.0f, 0.0f);
                    }
                } else
                {
                    if (move > 0)
                    {
                        xMovement = new Vector3(playerAirBackwardSpeed, 0.0f, 0.0f);
                    }
                    else if (move == 0)
                    {
                        xMovement = new Vector3(rb.velocity.x * airDrag, 0.0f, 0.0f);
                    }
                    else if (move < 0)
                    {
                        xMovement = new Vector3(-playerAirSpeed, 0.0f, 0.0f);
                    }
                }
            }
        }
    }

    private void ApplyMove()
    {
        if (!isSkid) //x movement
        {
            if (grounded)
            {
                if (xMovement.x > 0)
                {
                    facingRight = true;
                }
                else if (xMovement.x < 0)
                {
                    facingRight = false;
                }
            }

            if (grounded && (isRun && (move < 0 && rb.velocity.x > 0) || (move > 0 && rb.velocity.x < 0)))
            {
                isSkid = true;
                StartCoroutine("SkidToStop");
            }
            else
            {
                rb.velocity = xMovement + new Vector3(0.0f, rb.velocity.y, 0.0f);
            }
        }
    }

    private IEnumerator SkidToStop()
    {
        isSkid = true;

        float startPos = transform.position.x;
        float maxSkid = 3.0f;
        int dir = 1;

        if (rb.velocity.x > 0.0f) //find direction to skid
        {
            dir = 1;
            facingRight = false; //face opposite of skid
        }
        else if (rb.velocity.x < 0)
        {
            dir = -1;
            facingRight = true; //face opposite of skid
        }

        float endPos = transform.position.x + (maxSkid * dir);

        float i = 0;

        if (endPos > startPos)
        {
            while (endPos > transform.position.x)
            {
                if (i > 25)
                {
                    break;
                }

                i++;

                rb.velocity = new Vector2(rb.velocity.x * Mathf.Pow(1f - decelerate * Time.deltaTime, 2f), rb.velocity.y);

                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (endPos < transform.position.x)
            {
                if (i > 25)
                {
                    break;
                }

                i++;

                rb.velocity = new Vector2(rb.velocity.x * Mathf.Pow(1f - decelerate * Time.deltaTime, 2f), rb.velocity.y);

                yield return new WaitForFixedUpdate();
            }
        }

        rb.velocity = new Vector2(0, rb.velocity.y);

        isSkid = false;
        yield break;
    }
}