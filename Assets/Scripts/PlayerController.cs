using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Collider2D smallColl;
    //private Collider2D tmp;
    private Animator anim;    
    private float horizontalMove = 0;
    private float verticalMove = 0;
    private bool isHurt;
    private float hurtTime = 0.3f;

    public PhysicsMaterial2D friction0, friction1;
    public AudioSource bgmAudio,jumpAudio,cherryAudio,gemAudio,hurtAudio;

    public float speed = 10f;
    public float jumpForce;

    //跳跃、地面判断
    public Transform groundCheck;
    public Transform headCheck;
    public LayerMask ground;
    public bool isGround, isJump, isDashing, isHead;
    public int jumpCount;
    bool jumpPressed;

    //收集品计数
    public int cherry=0,gem=0;
    public Text cherryNumber,gemNumber;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        smallColl = GetComponent<CapsuleCollider2D>();
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
        isGround = Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale,0f, ground);
        isHead = Physics2D.OverlapCircle(headCheck.position, 0.49f, ground);
        //tmp= Physics2D.OverlapCircle(headCheck.position, 0.5f, ground);
        if (isGround)
        {
                rb.sharedMaterial = friction1;
        }
        else
        {
                rb.sharedMaterial = friction0;
        }
        if (!isHurt)
        {
            Movement();
            Crouch();
            Jump();
        }
        SwitchAnimation();
    }

    void Movement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("running", Mathf.Abs(horizontalMove));
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if(horizontalMove!=0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    void Crouch()
    {
        verticalMove = Input.GetAxisRaw("Vertical");
        if(verticalMove<0)
        {
            anim.SetBool("crouching", true);
            coll.enabled = false;
            smallColl.enabled = true;
        }
        else if(!isHead)
        {
            anim.SetBool("crouching", false);
            coll.enabled = true;
            smallColl.enabled = false;
        }
        //Debug.Log(tmp);
        //Debug.Log(isHead);
    }

    void Jump()
    {
        if(isGround)
        {
            jumpCount = 1;
        }
        if((jumpPressed&&isGround)||(!isGround&&jumpPressed&&jumpCount>0))
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
            jumpAudio.Play();
        }
    }

    void SwitchAnimation()
    {
        if(rb.velocity.y < 0.1f && !isGround)
        {
            anim.SetBool("falling", true);
        }
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
        if(isHurt)
        {
            anim.SetBool("hurt", true);
            hurtTime -= Time.deltaTime;
            if (hurtTime < 0)
            {
                isHurt = false;
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                hurtTime = 0.3f;
            }
               
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="DeadLine")
        {
            Invoke("ReStart", 1f);
            bgmAudio.Stop();
            hurtAudio.Play();
        }
        else if(collision.tag=="Collection")
        {
            cherry += 1;
            cherryNumber.text = cherry.ToString();
            Destroy(collision.gameObject);
            cherryAudio.Play();
        }
        else if(collision.tag=="Gem")
        {
            gem += 1;
            gemNumber.text = gem.ToString();
            Destroy(collision.gameObject);
            gemAudio.Play();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Enemies")
        {
            if(anim.GetBool("falling") && collision.gameObject.transform.position.y<transform.position.y)
            {
                Enemies enemy = collision.gameObject.GetComponent<Enemies>();
                enemy.OnJump();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("jumping", true);
            }
            else
            {
                rb.velocity = new Vector2(transform.position.x < collision.gameObject.transform.position.x? -5:5, rb.velocity.y);
                isHurt = true;
                hurtAudio.Play();
            }
        }
        
    }

    void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        bgmAudio.Play();
    }
}
