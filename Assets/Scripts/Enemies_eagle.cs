using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_eagle : Enemies
{
    //private Rigidbody2D rb;
    //private Animator anim;

    public Transform uppoint, downpoint;
    private float upvalue, downvalue;
    public float speed;

    private bool faceup = true;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        upvalue = uppoint.transform.position.y;
        downvalue = downpoint.transform.position.y;
        Destroy(uppoint.gameObject);
        Destroy(downpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(transform.position.y>=upvalue)
        {
            faceup = true;
        }
        else if(transform.position.y<=downvalue)
        {
            faceup = false;
        }
        if(faceup)
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }
    }
}
