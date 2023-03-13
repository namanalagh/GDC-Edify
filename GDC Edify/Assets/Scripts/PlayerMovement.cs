using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(0,10)]private float playerSpeed;
    [SerializeField][Range(0,10)]private float jumpForce;
    [SerializeField]private bool isGrounded;
    private float horizontal;
    
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]private LayerMask groundLayerMask;
    
    public Transform respawnPoint;
    public Text CoinCountText;
    private int coinCount=0;
    
    
    void Start()
    {
        coinCount = 0;
        respawnPoint.position = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
        
        horizontal = Input.GetAxis("Horizontal");
        
        rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);

        if (rb.velocity.x != 0)
        {
            anim.SetBool("isMoving",true);
        }
        else
        {
            anim.SetBool("isMoving",false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, .5f,groundLayerMask);
        Debug.DrawRay(transform.position,Vector2.down * 0.5f, Color.cyan);
        
        anim.SetBool("isGrounded",isGrounded);

        CoinCountText.text = "x" + coinCount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            coinCount += 1;
        }

        if (other.gameObject.tag == "Respawn")
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;
    }
}
