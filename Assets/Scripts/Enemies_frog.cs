using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_frog : Enemies
{
    //private Rigidbody2D rb;
    private Collider2D coll;
    //private Animator anim;

    public LayerMask ground;
    public Transform leftpoint, rightpoint;
    private float leftvalue, rightvalue;
    public float speed,jumpSpeed;


    private bool faceleft = true,isGround;


    void Awake()
    {

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        //anim = GetComponent<Animator>();
        leftvalue = leftpoint.position.x;
        rightvalue = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        isGround = coll.IsTouchingLayers(ground);
        SwitchAnimation();
    }

    void Movement()
    {
        if (isGround)
        {
            if(faceleft)
            {
                if(transform.position.x<leftvalue)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    faceleft = false;
                }
                else
                {
                    anim.SetBool("jumping", true);
                    rb.velocity = new Vector2(-speed, jumpSpeed);
                }
            }
            else
            {
                if (transform.position.x > rightvalue)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    faceleft = true;
                }
                else
                {
                    anim.SetBool("jumping", true);
                    rb.velocity = new Vector2(speed, jumpSpeed);
                }
            }
        }
    }

    void SwitchAnimation()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if(isGround&& anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

}
