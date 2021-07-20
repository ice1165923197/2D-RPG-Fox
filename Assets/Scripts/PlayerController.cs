using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    //public Rigidbody2D rd;
    public float speed = 10f;
    public float jumpforce;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump, isDashing;
    public int jumpCount;
    bool jumpPressed;

    private float horizontalmove = 0;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")&&jumpCount>0)
        {
            jumpPressed = true;
            anim.SetBool("jumping", true);
        }

    }
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Movement();
        Jump();
        SwitchAnimation();

    }

    void Movement()
    {
        horizontalmove = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("running", Mathf.Abs(horizontalmove));
        rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);

        if(horizontalmove!=0)
        {
            transform.localScale = new Vector3(horizontalmove, 1, 1);
        }
    }
    void Jump()
    {
        if(isGround)
        {
            jumpCount = 1;
        }
        if(jumpPressed&&isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            jumpPressed = false;
        }
        else if(!isGround&&jumpPressed&&jumpCount>0)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            jumpPressed = false;
        }
    }

    void SwitchAnimation()
    {
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if(anim.GetBool("falling"))
        {
            if(isGround)
            {
                anim.SetBool("falling", false);
            }
        }
    }
}
