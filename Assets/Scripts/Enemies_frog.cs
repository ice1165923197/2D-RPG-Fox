using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_frog : Enemies
{
    //private Rigidbody2D rb;
    private Collider2D coll;
    //private Animator anim;
    private float leftValue, rightValue;
    private bool isFaceLeft = true, isGround;

    public LayerMask ground;
    public Transform leftPoint, rightPoint;
    public float speed,jumpSpeed;

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
        leftValue = leftPoint.position.x;
        rightValue = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
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
            if(isFaceLeft)
            {
                if(transform.position.x<leftValue)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isFaceLeft = false;
                }
                else
                {
                    anim.SetBool("jumping", true);
                    rb.velocity = new Vector2(-speed, jumpSpeed);
                }
            }
            else
            {
                if (transform.position.x > rightValue)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isFaceLeft = true;
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
