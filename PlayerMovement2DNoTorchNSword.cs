using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2DNoTorchNSword : MonoBehaviour
{
    //-------------------------------------------
    //      VARIAVEIS DA INSTANCIA
    [SerializeField]private bool TorchSwordBool = false;
    [SerializeField]public Transform SwordNTorch;
    [SerializeField]public GameObject PlayerST;
    //-------------------------------------------
    //      VARIAVEIS DE MOVIMENTACAO
    [SerializeField]private float move;
    [SerializeField]private float moveSpeed = 10f;
    [SerializeField]private bool jumping;
    [SerializeField]private float jumpSpeed = 20f;
    //-------------------------------------------
    //         VARIAVEL DO CHAO
                    public bool isGrounded;
                    public Transform feetPosition;
                    public float sizeRadius;
                    public LayerMask  whatIsGround;

    //-------------------------------------------
    //           COMPONENTES    

    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator animationPlayer;
    //-------------------------------------------
    
    //-------------------------------------------
    //           WALL SLIDE
    [SerializeField]private int amountOfJumpsLeft;
    [SerializeField]private float movementInputDirection;
    [SerializeField]private float movementSpeed = 10f;
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private bool isFacingRight = true;
    [SerializeField]private bool isTouchingWall;
    [SerializeField]private bool isWallSliding;
                    
                    
                    public float movementForceInAir;
                    public float airDragMultiplier = 0.9f;
                    public float variableJumpHeightMultiplier = 0.5f;
                    public float wallCheckDistance = 0.75f; 
                    public float wallSlideSpeed;               
                                   
                    public Transform wallCheck;

    [SerializeField]private bool isTouchingWall2;
    [SerializeField]private bool isWallSliding2;  
                    public float wallCheckDistance2 = -0.75f; 
                    public float wallSlideSpeed2;
                           

                    public Transform wallCheck2;     
    //----------------------------------------------------------
    //          WALL JUMP
                    public Vector2 wallHopDirection;
                    public Vector2 wallJumpDirection;

    [SerializeField]private int facingDirection = 1;
    [SerializeField]private int amountOfJumps = 1;
    [SerializeField]private int lastWallJumpDirection;
    [SerializeField]private float wallJumpTimer;
    [SerializeField]private float wallJumpTimerSet = 0.5f;
                    
                    public float wallHopForce;
                    public float wallJumpForce;

                    public bool hasWallJumped;



    //                  
    //-----------------------------------------------------------
    //          NORMAL JUMP / OTHER'S JUMPS

                    private float jumpTimer;
                    private float turnTimer;
                    private float jumpTimerSet = 0.5f;
                    private float turnTimerSet = 0.1f;

                    private bool isAttemptingToJump;
                    private bool canNormalJump;
                    private bool canWallJump;
                    private bool checkJumpMultiplier;
                    private bool canMove;
                    private bool canFlip;

    //----------------------------------------------------------
    //                     LEDGE CLIMB 
    
                    private Vector2 ledgePosBot;
                    private Vector2 ledgePos1;
                    private Vector2 ledgePos2;

                    public float ledgeClimbXOffset1 = 1f;
                    public float ledgeClimbXOffset2 = 3f;
                    public float ledgeClimbYOffset1 = 1f;
                    public float ledgeClimbYOffset2 = 3f;

                    private bool isTouchingLedge;
                    private bool canClimbLedge = false;
                    private bool ledgeDetected;

                    public Transform ledgeCheck; 

                
    //-----------------------------------------------------------
    
    //###########################################################
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationPlayer = GetComponent<Animator>();
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();

    }
    //---------------------------------------------------------------
    //###########################################################
    void Update()
    {
        //--------------------------------------------------------
        //    CHECAGENS DO SLIDE
            CheckInput();
            CheckMovementDirection();
            UpdateAnimations();
            
            CheckIfWallSliding();
            CheckIfWallSliding2();

            CheckLedgeClimb();

            CheckJump();

        //-----------------------------------------------------
     
        if(TorchSwordBool == false)
        {

            isGrounded =  Physics2D.OverlapCircle(feetPosition.position, sizeRadius, whatIsGround);
     
        
        
        
                move = Input.GetAxis("Horizontal");

                    if(Input.GetButtonDown("Jump") && isGrounded)  
                        {
                            jumping = true; 
                        }
                                
                   
                    
            //-------------------------------------------------------------------------
            //se for chao entao libera a sprint do andando e bloca o resto
            if(isGrounded)  
            {
               animationPlayer.SetBool("JumpingV", false);
               animationPlayer.SetBool("FallingV", false);
               animationPlayer.SetBool("JumpingH", false);
               animationPlayer.SetBool("FallingH", false); 

                // se ele nao estiver no alto -> sprinte andando se nao ;....
                if (rb.velocity.x != 0 && move != 0) 
                {
                     animationPlayer.SetBool("Walking", true);
                }
                else
                {
                     animationPlayer.SetBool("Walking", false);
                }
            
            
            }
            else
            {
                if (rb.velocity.x == 0)
                {
                    animationPlayer.SetBool("Walking", false);

                    if (rb.velocity.y > 0)
                    {
                        animationPlayer.SetBool("JumpingV", true);
                        animationPlayer.SetBool("FallingV", false);
                        animationPlayer.SetBool("JumpingH", false);
                        animationPlayer.SetBool("FallingH", false);
                    }
                    if (rb.velocity.y < 0)
                    {
                        animationPlayer.SetBool("JumpingV", false);
                        animationPlayer.SetBool("FallingV", true);
                        animationPlayer.SetBool("JumpingH", false);
                        animationPlayer.SetBool("FallingH", false);
                    }
                }
                else
                {

                    if (rb.velocity.y > 0)
                    {
                        animationPlayer.SetBool("JumpingV", false);
                        animationPlayer.SetBool("FallingV", true);
                        animationPlayer.SetBool("JumpingH", false);
                        animationPlayer.SetBool("FallingH", false);
                    }
                    if (rb.velocity.y < 0)
                    {
                        animationPlayer.SetBool("JumpingV", false);
                        animationPlayer.SetBool("FallingV", false);
                        animationPlayer.SetBool("JumpingH", false);
                        animationPlayer.SetBool("FallingH", true);
                    }

                }

            }

        }
        else
        {
            animationPlayer.SetBool("Walking", false);
            animationPlayer.SetBool("PickST", true);    
        }
       
       
    }
    //------------------------------------------------------------------------
    //###########################################################
    void FixedUpdate()
    {
        //----------------------------------------------------------------
        //          CHECAGEM DOS SLIDE
            ApplyMovement();
            ApplyMovement2();
            CheckSurroundings();
            CheckSurroundings2();


        //-----------------------------------------------------------------
    
    
    
        //--------------------------------------------------------------------------------
        //            if de que nao vai pular enquanto roda a animacao
        if(TorchSwordBool == false)
        {

                   rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
    
    
            if (jumping)
                {
                    //rb.velocity = Vector2.up * jumpSpeed;
                    rb.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);

                    jumping = false;
                }
    

        }
        //---------------------------------------------------------------------------------
    
    }
    //---------------------------------------------------------------------------
    //          FUNCOES DO SLIDE
    private void CheckMovementDirection()
    {
         if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
    }   



    private void CheckIfWallSliding()
    {
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0 && !canClimbLedge)
        {
                isWallSliding = true;
        }
        else
        {
                isWallSliding = false;
        }

       
    }



    private void CheckIfWallSliding2()
    {
         if (isTouchingWall2 && !isGrounded && rb.velocity.y > 0)
        {
                isWallSliding2 = true;
        }
        else
        {
                isWallSliding2 = false;
        }
    }



    private void CheckSurroundings()
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        //verificacao do climb
        if(isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
           ledgeDetected = true; 
           ledgePosBot = wallCheck.position;
        }
    }



    private void CheckSurroundings2()
    {
        isTouchingWall2 = Physics2D.Raycast(wallCheck2.position, transform.right, wallCheckDistance2, whatIsGround);
    }



    private void UpdateAnimations()
    {
        animationPlayer.SetBool("isWallSliding", isWallSliding);

    }
//-----------------------------------------------------------------------------------
//                          LEDGE CLIMB

    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if(isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);

            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            animationPlayer.SetBool("canClimbLedge", canClimbLedge);
        
        }

        if (canClimbLedge)
        {
           transform.position = ledgePos1;
        }
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        animationPlayer.SetBool("canClimbLedge", canClimbLedge);

    }

//-----------------------------------------------------------------------------------
//                          JUMP SISTEM

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded || (amountOfJumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump =  true;
            }
        }


        if(Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if(!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        
        }
        
        if(turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                 canMove = true;
                canFlip = true;
            }
        }

        if(checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }
    
    
    
    
       private void CheckJump()
    {
       if(jumpTimer > 0)
       {
            //wall jump
            if(!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
       
       }
       
       if(isAttemptingToJump)
       {
            jumpTimer -= Time.deltaTime;
       }

       if(wallJumpTimer > 0)
       {
            if(hasWallJumped && movementInputDirection == - lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if(wallJumpTimer <= 0)
                {
                    hasWallJumped = false;
                }
                else
                {
                    wallJumpTimer -= Time.deltaTime;
                }
       }



    }
    
    
    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if(isTouchingWall)
        {
            canWallJump = true;
        }

        if(amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

 

    private void NormalJump()
    {
         if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if(canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;

        }
    }

    private void ApplyMovement()
    {
        
        if(!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        else  if (canMove)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }

        if(isWallSliding)
        {
            if(rb.velocity.y <- wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }

        }
    }



    private void ApplyMovement2()
    {
       

        if (isWallSliding2)
        {
            if (rb.velocity.y < -wallSlideSpeed2)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed2);
            }
        
        }
    }


//-----------------------------------------------------------------------------------------------
    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        } 
      
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        Gizmos.DrawLine(wallCheck2.position, new Vector3(wallCheck2.position.x + wallCheckDistance2, wallCheck2.position.y, wallCheck2.position.z));
    }

    

    //---------------------------------------------------------------------------
    //                  FUNCAO

    
    //Se o personagem enconstar no colisor onde a tag e "swordtorch" & estiver no chao 
    //entao TorchSwordBool e verdadeiro e roda a animacao
    void OnTriggerStay2D(Collider2D Coll)
    {
            if(Coll.gameObject.tag == "SwordTorch" && isGrounded)
            {
                TorchSwordBool = true;
                //transform.position = TorchNSword.position;
                rb.velocity = new Vector2(0, 0);
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
            }
    
    }
    

    //----------------------------------------------------------------------------
    //             FUNCAO QUE VAI DESTROIR O PLAYER E O CAVALERO MORRENDO
   
    void DestroyTorchNSword()
    {
        Destroy(SwordNTorch.gameObject);
    }


    void DestroyPlayer()
    {    //aqui destroi
        Destroy(gameObject);
        Instantiate(PlayerST, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }

}

