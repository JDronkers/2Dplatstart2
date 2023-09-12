using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;

    //Your Speed! Horizontal Speed.
    private float speed = 16f;
    //How high your jump goes
    private float jumpingPower = 16f;
    
    //Not your concern
    private bool isFacingRight = true;

    //How low the death zone is. If you want a death material instead that is more adjustable, that can be done as well. 
    private float deathYValue = -30f;

    //Not your concern
    private bool hasDoubleJump = false;
    //How much stronger than the original jump the double jump is
    private float doubleJumpPower = .7f;

    private bool isWallSliding;
    //How slow or quick you slide down a wall
    private float wallSlidingSpeed = 2f;

    //Only Used for potential wall jump limit rn because wall jumps can climb lol
    private float totalStamina = 10f;
    //How much stamina wall jumping takes away
    private float wallJumpStamina = 3f;

    //Not your concern
    private float currentStamina;

    //Not your concern
    private bool isWallJumping;
    //Not your concern
    private float wallJumpingDirection;
    //One of 2 things relating to how long the wallJump is forced. Idrk, please play around with these so you can tell me what is what LOL
    private float wallJumpingTime = 0.1f;
    //Not your concern
    private float wallJumpingCounter;
    //One of 2 things relating to how long the wallJump is forced. Idrk, please play around with these so you can tell me what is what LOL
    private float wallJumpingDuration = 0.4f;
    //How strong the wallJump is. Numbers are horizontal and vertical respectively
    private Vector2 wallJumpingPower = new Vector2(4f, 24f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform DDcheck;
    [SerializeField] private LayerMask DDLayer;
    [SerializeField] private Transform Spawnpoint;

    private void Update()
    {
        Respawn();
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isDDRefreshable())
        {
            hasDoubleJump = true;
            currentStamina = totalStamina;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool isDDRefreshable()
    {
        return Physics2D.OverlapCircle(DDcheck.position, 0.2f, DDLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    public void setDDs()
    {
        hasDoubleJump = true;
        currentStamina = totalStamina;
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && currentStamina >= 0)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            currentStamina -= wallJumpStamina;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
        else if (Input.GetButtonDown("Jump") && !IsGrounded())
        {
            if (hasDoubleJump)
            {

                rb.velocity = new Vector2(rb.velocity.x, jumpingPower * doubleJumpPower);
                hasDoubleJump = false;
            }
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private bool isDead()
    {
        if (transform.position.y < deathYValue)
        {
            return true;
        }
        return false;
    }

    private void Respawn()
    {
        if (isDead())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //transform.position = Spawnpoint.position;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
