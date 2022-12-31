using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
                    public static float move;
    [SerializeField]private float moveSpeed = 10f;
    [SerializeField]private bool jumping;
    [SerializeField]private float jumpSpeed = 20f;
//-------------------------------------------
//         VARIAVEL DO CHAO
    [SerializeField]private bool isGrounded;
    public Transform feetPosition;
    public float sizeRadius;
    public LayerMask  whatIsGround;
//--------------------------------------------
//             VARIAVEL PARA ATK

    [SerializeField]private bool attackingBool;
    
   


//-------------------------------------------
//           COMPONENTES    

    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator animationPlayer;



    public bool doubleAtk, lockAtk = false;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationPlayer = GetComponent<Animator>();

    }
//---------------------------------------------------------------
    void Update()
    {
     
       //Reconhece o chao
        isGrounded =  Physics2D.OverlapCircle(feetPosition.position, sizeRadius, whatIsGround);
     
        
        
      //Movimentacao do personagem
        move = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && isGrounded && attackingBool == false)  
        {
           jumping = true; 
        }

        if(move < 0)
        {
            sprite.flipX = true;
        }else if(move > 0)
        {
            sprite.flipX = false;
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
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
    
    
        if (jumping)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            //rb.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);

            jumping = false;
        }

    }

//------------------------------------------------------------------------------
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
