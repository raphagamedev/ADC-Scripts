using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
//-------------------------------------------
//           VARIAVEIS DE MOVIMENTACAO
    //move precisa ficar public e static para ser acessivel ao script de ataque
    [SerializeField]public static float move;
    [SerializeField]private float moveSpeed = 10f;
    [SerializeField]private float jumpSpeed = 20f;
    [SerializeField]private bool jumping;
    
//-------------------------------------------
//              VARIAVEL DO CHAO

                    public bool isGrounded;
                    public Transform feetPosition;
                    public Transform groundCheck;
                    public float sizeRadius;
                    public float groundCheckRadius;
                    public LayerMask  whatIsGround;
                    

//--------------------------------------------
//             VARIAVEL PARA ATK

    [SerializeField]private bool attackingBool;
    
   
//-------------------------------------------
//              VARIAVEL JUMP
    [SerializeField]private int amountOfJumpsLeft;
    [SerializeField]private int facingDirection = 1;
    
    [SerializeField]public int amountOfJumps = 1;
    
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private float jumpTimer;
    [SerializeField]private float turnTimer;
    [SerializeField]private float turnTimerSet = 0.1f;
   
    
    [SerializeField]private bool canNormalJump;
    [SerializeField]private bool canWallJump;
    [SerializeField]private bool isAttemptingToJump;
    [SerializeField]private bool checkJumpMultiplier;
    [SerializeField]private bool canMove;
    [SerializeField]private bool canFlip;

                    public float jumpTimerSet = 0.5f;

                    
                    


//-------------------------------------------
//              WALL SLIDING

  
    [SerializeField]public static float movementInputDirection;
    [SerializeField]private float movementSpeed = 10f;
    [SerializeField]public static bool isFacingRight = true;
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
//---------------------------------------------------
//             WALL JUMPING

                    public Vector2 wallHopDirection;
                    public Vector2 wallJumpDirection;

   
    [SerializeField]private int lastWallJumpDirection;
    [SerializeField]private float wallJumpTimer;
    [SerializeField]private float wallJumpTimerSet = 0.5f;
                    
                    public float wallHopForce;
                    public float wallJumpForce;

                    public bool hasWallJumped;

//---------------------------------------------------
//             LEDGE CLIMB

                    public Transform ledgeCheck;

   
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
                
                    



//-------------------------------------------
//           COMPONENTES    

    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator animationPlayer;
    



    public bool doubleAtk, lockAtk = false;

//----------------------------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationPlayer = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();

    }
//---------------------------------------------------------------
    void Update()
    {
        //-----------------------------------------------------
        //          CHECK MOVIMENTS


            CheckInput();
            CheckMovementDirection();
            UpdateAnimations();
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckIfWallSliding2();
            CheckJump();
            CheckLedgeClimb();
            CheckIfWallSliding2();

 

 



        //-------------------------------------------------------
         
        
        
        //Movimentacao do personagem
        move = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && isGrounded && attackingBool == false)  
        {
           jumping = true; 
        }

        
       
            //ATK do personagem
            if (Input.GetButtonDown("Fire3") && lockAtk == false)
            {
                attackingBool = true;

                if (isGrounded)
                {
                    animationPlayer.SetBool("SingleATKGround", true);
                    animationPlayer.SetBool("SingleATKJump", false);

                    animationPlayer.SetBool("DoubleATKGround", false);

                }
                else
                {
                    animationPlayer.SetBool("SingleATKJump", true);
                    animationPlayer.SetBool("SingleATKGround", false);

                    animationPlayer.SetBool("DoubleATKGround", false);

                }

                if (doubleAtk == true)
                {
                    animationPlayer.SetBool("DoubleATKGround", true);
                    animationPlayer.SetBool("SingleATKGround", false);
                }
            }
            
             if (attackingBool == true && isGrounded)
               {
                    move = 0;
                }
        
            
            
            
            //se for chao entao libera a sprint do andando e bloca o resto
            if(isGrounded)  
            {
               animationPlayer.SetBool("JumpingV", false);
               animationPlayer.SetBool("FallingV", false);
               animationPlayer.SetBool("JumpingH", false);
               animationPlayer.SetBool("FallingH", false); 
            
                if (rb.velocity.x != 0 && move != 0) // se ele nao estiver no alto -> sprinte andando se nao ;....
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
//------------------------------------------------------------------------
    void FixedUpdate()
    {
            ApplyMovement();
            ApplyMovement2();
            CheckSurroundings();
            CheckSurroundings2();
        
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
         if (jumping)
        {
            rb.velocity = Vector2.up * jumpSpeed;
         
            jumping = false;
        }

    }
//#############################################################################    
//-----------------------------------------------------------------------------
//                          CHECK DE CHAO / WALL / DIRECAO
    
    private void CheckSurroundings()
    {       //CHAO
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
            //PAREDE
            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
            //ESCALA
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

    
    
    // DIRECAO DE MOVIMENTO (DIREITA / ESQUERDA)
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

    
    
    
    // ATIVA ANIMACAO DE CORRIDA
    private void UpdateAnimations()
    {
      
        animationPlayer.SetBool("isWallSliding", isWallSliding);


    }
    // VERIFICA BOTAO DE PULO
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            if(isGrounded || (amountOfJumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;

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
//--------------------------------------------------------------------------------------
//                               JUMP

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


    private void CheckJump()
    {
        
     
      if(jumpTimer > 0)
      {
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
    
        if(hasWallJumped && movementInputDirection == -lastWallJumpDirection)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            hasWallJumped = false;
        }
        else if(wallJumpTimer <= 0)
        {
            hasWallJumped = false;
        }else{
            wallJumpTimer -= Time.deltaTime;
        }
    
    }

    private void NormalJump()
    {
        if(canNormalJump)
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
        if (canWallJump)
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
 //--------------------------------------------------------------------------------------
 //                              WALL SLIDING

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0 && !canClimbLedge )
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
//--------------------------------------------------------------------------------------
//                               LEDGE CLIMB
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

 //--------------------------------------------------------------------------------------
 //                                WALKING
 
    private void ApplyMovement()
    {
        
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        
       
        
        if(isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
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

//---------------------------------------------------------------------------------------
    private void Flip()
    {

        if(!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        
    }








//###########################################################################
//---------------------------------------------------------------------------
//                  MARCA DE GIZ

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    
        Gizmos.DrawLine(wallCheck2.position, new Vector3(wallCheck2.position.x + wallCheckDistance2, wallCheck2.position.y, wallCheck2.position.z));
    }
    









//#############################################################################
//------------------------------------------------------------------------------
//          FUNCAO ATK PERSONAGEM
    void EndAnimationATK()
    {
        animationPlayer.SetBool("SingleATKJump", false);
        animationPlayer.SetBool("SingleATKGround", false);
        
        attackingBool = false;
    }

    void EndAnimationDoubleATK()
    {
        animationPlayer.SetBool("DoubleATKGround", false);
        
        doubleAtk = false;
        attackingBool = false;
    }

}
