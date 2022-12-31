using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKPlayer : MonoBehaviour
{
    private BoxCollider2D colliderAtkPlayer;
    
    void Start()
    {
        colliderAtkPlayer = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (PlayerMovement2D.move < 0)
        {
            colliderAtkPlayer.offset = new Vector2(-0.6f, 0);
        }
        else if (PlayerMovement2D.move > 0)
        {
           colliderAtkPlayer.offset = new Vector2(0.6f, 0);     
        }
    }
}
