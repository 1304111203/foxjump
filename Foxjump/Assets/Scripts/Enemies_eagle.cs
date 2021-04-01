using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_eagle : Enemy
{
    public float Speed;
    public Transform toppoint;
    public Transform bottompoint;
    //private Rigidbody2D rb;
    //private Collider2D coll;

    private float TopY;
    private float BottomY;
    private bool rising = true;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collider2D>();
        TopY = toppoint.position.y;
        BottomY = bottompoint.position.y;
        Destroy(toppoint.gameObject);
        Destroy(bottompoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (rising)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if(transform.position.y > TopY)
            {
                rising = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if (transform.position.y < BottomY)
            {
                rising = true;
            }
        }
    }
}
