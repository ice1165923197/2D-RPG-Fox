using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_eagle : Enemies
{
    //private Rigidbody2D rb;
    //private Animator anim;

    public Transform upPoint, downPoint;
    private float upValue, downValue;
    public float speed;

    private bool isFaceUp = true;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        upValue = upPoint.transform.position.y;
        downValue = downPoint.transform.position.y;
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(transform.position.y>=upValue)
        {
            isFaceUp = true;
        }
        else if(transform.position.y<=downValue)
        {
            isFaceUp = false;
        }
        rb.velocity = new Vector2(rb.velocity.x, (isFaceUp?-1:1) * speed);

    }
}
