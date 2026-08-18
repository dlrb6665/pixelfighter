using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float climbSpeed;
    public float restoreTime;

    private Rigidbody2D rig;
    private Animator anim;
    private BoxCollider2D box;
    private bool isGround;
    private bool canDoubleJump;
    private bool isOneWayPlatform;

    private bool isLadder;
    private bool isClimbing;

    private bool isJumping;
    private bool isFalling;
    private bool isDoubleJumping;
    private bool isDoubleFalling;

    private float playerGravity;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        playerGravity = rig.gravityScale;

    }

    void Update()
    {
        if (GameController.isGameAlive)
        {
            CheckAirStatus();
            Flip();
            Run();
            Jump();
            Climb();
            //Attack();
            CheckGrounded();
            CheckLadder();
            SwitchAnimation();
            OneWayPlatFormCheck();
            
        }
    }

    void CheckGrounded()
    {
        isGround = box.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
                   box.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) ||
                   box.IsTouchingLayers(LayerMask.GetMask("DestructibleLayer")) ||
                   box.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        isOneWayPlatform = box.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
    }

    void CheckLadder()
    {
        isLadder = box.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }

    void Flip()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(rig.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if(rig.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (rig.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, rig.velocity.y);
        rig.velocity = playerVel;
        bool playerHasXAxisSpeed = Mathf.Abs(rig.velocity.x) > Mathf.Epsilon;
        anim.SetBool("Run", playerHasXAxisSpeed);
    }
    void Jump() 
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                anim.SetBool("Jump", true);
                Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                rig.velocity = Vector2.up * jumpSpeed;
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    anim.SetBool("DoubleJump", true);
                    Vector2 doubleJumpVel = new Vector2(0.0f, doubleJumpSpeed);
                    rig.velocity = Vector2.up * doubleJumpSpeed;
                    canDoubleJump = false;
                }
            }           
        }
    }

    void Climb()
    {
        if (isLadder)
        {
            float moveY = Input.GetAxis("Vertical");
            if (moveY > 0.5f || moveY <-0.5f)
            {
                anim.SetBool("Climbing", true);
                rig.gravityScale = 0.0f;
                rig.velocity = new Vector2(rig.velocity.x, moveY * climbSpeed);
            }
            else
            {
                if(isJumping || isFalling || isDoubleJumping || isDoubleFalling)
                {
                    anim.SetBool("Climbing", false);
                }
                else
                {
                    anim.SetBool("Climbing", false);
                    rig.velocity = new Vector2(rig.velocity.x, 0.0f);
                }
            }
        }
        else
        {
            anim.SetBool("Climbing", false);
            rig.gravityScale = playerGravity;
        }


    }

   /* void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("Attack");
        }
    }*/

    void SwitchAnimation()
    {
        anim.SetBool("Idle", false);
        if (anim.GetBool("Jump"))
        {
            if (rig.velocity.y < 0.0f)
            {
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", true);
            }
        }
        else if(isGround)
        {
            anim.SetBool("Fall", false);
            anim.SetBool("Idle", true);
        }

        if (anim.GetBool("DoubleJump"))
        {
            if (rig.velocity.y < 0.0f)
            {
                anim.SetBool("DoubleJump", false);
                anim.SetBool("DoubleFall", true);
            }
        }
        else if (isGround)
        {
            anim.SetBool("DoubleFall", false);
            anim.SetBool("Idle", true);
        }
    }

    void OneWayPlatFormCheck()
    {
        float moveY = Input.GetAxis("Vertical");
        if (isOneWayPlatform && moveY < -0.1f)
        {
            gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
            Invoke("RestorePlayerLayer",restoreTime);
        }
    }

    void RestorePlayerLayer()
    {
        if(!isGround && gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    void CheckAirStatus()
    {
        isJumping = anim.GetBool("Jump");
        isFalling = anim.GetBool("Fall");
        isDoubleJumping = anim.GetBool("DoubleJump");
        isDoubleFalling = anim.GetBool("DoubleFall");
        isClimbing = anim.GetBool("Climbing");
    }
}
