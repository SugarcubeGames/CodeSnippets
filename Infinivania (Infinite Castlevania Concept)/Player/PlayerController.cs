using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

    /// <summary>
    /// Player movement speed modifier
    /// </summary>
    public float moveSpeed = 1.5f;

    /// <summary>
    /// Player jump height modifier
    /// </summary>
    public float jumpSpeed = 5.0f;

    /// <summary>
    /// How long after jump do we wait before checking for landing?
    /// </summary>
    public float jumpCheckDelay = 0.5f;

    /// <summary>
    /// How long is left in the current jump delay before we check for jumping?
    /// </summary>
    private float jumpCheckDelayRemaining = 0.0f;

    /// <summary>
    /// Is the character currently jumping?
    /// </summary>
    public bool isJumping = false;


    /// <summary>
    /// Is the player currently moving to the left?
    /// </summary>
    private bool isMovingLeft = false;

    /// <summary>
    /// Is the player curently moving to the right?
    /// </summary>
    private bool isMovingRight = false;

    /// <summary>
    /// Is the player moving down stairs?
    /// </summary>
    private bool isMovingDown = false;

    /// <summary>
    /// Is the player currently crouching?
    /// </summary>
    private bool isCrouching = false;


    /// <summary>
    /// accessor for rigid body 2d
    /// </summary>
    public Rigidbody2D rb;

    /// <summary>
    /// Trigger object to check if the player is in the ground. (Right Side)
    /// </summary>
    //public Transform groundcheckRight;
    /// <summary>
    /// Trigger object to check if the player is in the ground. (Left Side)
    /// </summary>
    //public Transform groundCheckLeft;

    /// <summary>
    /// How large of a circle to use to check if you are on the ground
    /// </summary>
    //public float groundCheckRadius;
    /// <summary>
    /// Which layer is considered to be ground?
    /// </summary>
    public LayerMask whatIsGround;

    /// <summary>
    /// Raycast hit for the right corner of the collision box.
    /// </summary>
    private RaycastHit hitRight;
    /// <summary>
    /// Raycast hit for the left corner of the collision box.
    /// </summary>
    private RaycastHit hitLeft;
    /// <summary>
    /// Box Collider for this object.
    /// </summary>
    public BoxCollider2D boxCol;

    /********************
     * Animation Variables
     * *******************/
     /// <summary>
     /// Animator object on this script.
     /// </summary>
    private Animator anim;
    /// <summary>
    /// Sprite renderer
    /// </summary>
    private SpriteRenderer sr;


    // Use this for initialization
    void Start () {
        anim = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        boxCol = this.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

        //Standard input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X))
        {
            //applyMovementJump();
            Jump();
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            //isMovingRight = true;
            MoveRight();
        }

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            //isMovingLeft = true;
            MoveLeft();
        }

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            //isMovingRight = true;
            MoveDown();
        }

        //When the left or right key is released, stop movement
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            //isMovingLeft = false;
            EndMoveLeft();
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            //isMovingRight = false;
            EndMoveRight();
        }

        if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)))
        {
            //isMovingRight = true;
            EndMoveDown();
        }

        //when veolcity is above 0, the player is moving.  Pass this data to the animator.
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isCrouching", isCrouching);

        //Flip the animation if necessary
        //Only update the animation direciton if the player is actually moving
        if(Mathf.Abs(rb.velocity.x) > 0)
        {
            sr.flipX = !(rb.velocity.x < 0);
        }
	}


    void FixedUpdate()
    {
        /*
        //Check to see if the player has landed.
        if(!Physics2D.OverlapCircle(groundcheckRight.position, groundCheckRadius, whatIsGround) &&
            !Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, whatIsGround)){
            isJumping = true;
        } else
        {
            isJumping = false;
        }
        */

        //Instead of the overlapping circles, which are causing issues with side-wall collision unless they
        //are pulled in, which in turn causes collision issues with the corners of the player not being
        //picked up as colliding, I'm going to cast two raycasts down from the very corners of the bounding box
        //to determine if the ground is being hit.

        //We don't want's to check immediatly.  Make sure delay time has passed.
        if(jumpCheckDelayRemaining <= 0.0f && isJumping)
        {
            if (!checkRaycastDown(new Vector2(boxCol.bounds.min.x, boxCol.bounds.min.y), whatIsGround) &&
            !checkRaycastDown(new Vector2(boxCol.bounds.max.x, boxCol.bounds.min.y), whatIsGround))
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
                jumpCheckDelayRemaining = 0.0f;
            }
        }

        //if there is still time left on the jumpCheckDelayRemaining, reduce it by the amount of time that's passed.
        if(jumpCheckDelayRemaining > 0.0f)
        {
            jumpCheckDelayRemaining -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        //DebugExtension.DrawCircle(groundCheckLeft.position, 0.05f);
        //DebugExtension.DrawCircle(groundcheckRight.position, 0.05f);
        
        
        //DebugExtension.DrawCircle(new Vector2(boxCol.bounds.min.x, boxCol.bounds.min.y), 0.01f);
        //DebugExtension.DrawCircle(new Vector2(boxCol.bounds.max.x, boxCol.bounds.min.y), 0.01f);
        
        //Draw Rays down from the bottom of the collision box.
        //Vector3 rightRay = new Vector3(boxCol.bounds.min.x, boxCol.bounds.min.y, 0.0f);
        //Vector3 leftRay = new Vector3(boxCol.bounds.max.x, boxCol.bounds.min.y, 0.0f);
        //Debug.DrawRay(rightRay, Vector3.down, Color.blue);
        //Debug.DrawRay(leftRay, Vector3.down, Color.blue);
    }

    //Doing this as a late update in the hopes that, when using the touch
    //screen controls, it will be called in the same frame after the player
    //has touched the screen, preventing a one-frame lag issue with the controls
    private void LateUpdate()
    {
        if (isMovingRight)
        {
            applyMovementRight();
        }

        if (isMovingLeft)
        {
            applyMovementLeft();
        }

        if (isMovingDown)
        {
            //Are we on stairs, or crouching?

            //for now, assume crouchingm which doesn't require us to do anything.
        }
    }

    /*******************************************************************
     * TOUCH CONTROLLER INPUT FUNCTIONS
     * *****************************************************************/
    public void Jump()
    {
        if (!isCrouching)
        {
            applyMovementJump();
        }
    }

    public void MoveRight()
    {
        if (!isMovingLeft && !isJumping && !isCrouching)
        {
            isMovingRight = true;
        }
    }

    public void EndMoveRight()
    {
        isMovingRight = false;
    }

    public void MoveLeft()
    {
        if (!isMovingRight  && ! isJumping && !isCrouching)
        {
            isMovingLeft = true;
        }
    }

    public void EndMoveLeft()
    {
        isMovingLeft = false;
    }

    public void MoveDown()
    {
        //Check to see if we're on stairs.  If so, switch to moving on stairs.
        //Otherwise, crouch.
        if (!isJumping)
        {
            isCrouching = true;
        }
    }

    public void EndMoveDown()
    {
        isCrouching = false;
    }

    /*****************************************************************************
     * Apply movement
     * ***************************************************************************/
    private void applyMovementJump()
    {
        if (!isJumping)
        {
            jumpCheckDelayRemaining = jumpCheckDelay;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            isJumping = true;
        }
    }

    private void applyMovementRight()
    {
        if (!isJumping)
        {
            //rb.velocity = Vector2.right * moveSpeed;
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    private void applyMovementLeft()
    {
        if (!isJumping)
        {
            //rb.velocity = Vector2.left * moveSpeed;
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }


    /***************************************************************************
     * Utility Methods
     * *************************************************************************/

    private bool checkRaycastDown(Vector2 origin, LayerMask lm)
    {
        //return Physics2D.Raycast(origin, Vector2.down, .5f, lm);
        RaycastHit2D  hit = Physics2D.Raycast(origin, Vector2.down, .5f, lm);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }


    /***************************************************************************
     * Debug Methods
     * *************************************************************************/
    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
