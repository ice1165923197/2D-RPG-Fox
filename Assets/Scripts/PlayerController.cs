using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Collider2D smallcoll;
    //private Collider2D tmp;
    private Animator anim;
    //public Rigidbody2D rd;
    public float speed = 10f;
    public float jumpforce;
    public Transform groundCheck;
    public Transform headCheck;
    public LayerMask ground;
    public PhysicsMaterial2D f0,f1;
    public AudioSource bgmAudio,jumpAudio,cherryAudio,gemAudio,hurtAudio;

    //Ã¯‘æ°¢µÿ√Ê≈–∂œ
    public bool isGround, isJump, isDashing, isHead;
    public int jumpCount;
    bool jumpPressed;

    //
    public int Cherry=0,Gem=0;
    public Text CherryNumber,GemNumber;

    private float horizontalmove = 0;
    private float verticalmove = 0;

    private bool isHurt;
    private float hurtTime = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        smallcoll = GetComponent<CapsuleCollider2D>();
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
                rb.sharedMaterial = f1;
        }
        if (!isGround)
        {
                rb.sharedMaterial = f0;
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
        horizontalmove = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("running", Mathf.Abs(horizontalmove));
        rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);

        if(horizontalmove!=0)
        {
            transform.localScale = new Vector3(horizontalmove, 1, 1);
        }
    }

    void Crouch()
    {
        verticalmove = Input.GetAxisRaw("Vertical");
        if(verticalmove<0)
        {
            anim.SetBool("crouching", true);
            coll.enabled = false;
            smallcoll.enabled = true;
        }
        else if(!isHead)
        {
            anim.SetBool("crouching", false);
            coll.enabled = true;
            smallcoll.enabled = false;
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
        if(jumpPressed&&isGround)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            jumpPressed = false;
            jumpAudio.Play();
        }
        else if(!isGround&&jumpPressed&&jumpCount>0)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
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
                hurtTime = 0.5f;
            }
               
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Collection")
        {
            Cherry += 1;
            CherryNumber.text = Cherry.ToString();
            Destroy(collision.gameObject);
            cherryAudio.Play();
        }else if(collision.tag=="Gem")
        {
            Gem += 1;
            GemNumber.text = Gem.ToString();
            Destroy(collision.gameObject);
            gemAudio.Play();
        }
        if(collision.tag=="DeadLine")
        {
            Invoke("ReStart", 1f);
            bgmAudio.Stop();
            hurtAudio.Play();
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
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
                anim.SetBool("jumping", true);
            }
            else if(transform.position.x<collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
                isHurt = true;
                hurtAudio.Play();
            }
            else if(transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
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
