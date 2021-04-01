using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_forg : Enemy
{
    //private Rigidbody2D rb;
    //private Animator anim;
    private Collider2D coll;

    public Transform leftpoint;
    public Transform rightpoint;
    public LayerMask ground;
    public float Speed;
    public float JumpForce;

    private bool Faceleft = true;
    private float leftx;
    private float rightx;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
        Animswitch();
    }

    void Movement()
    {
        if (Faceleft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
            }          
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(Speed, JumpForce);
                Faceleft = false;
            }

        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(Speed, JumpForce);
            }
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(-Speed, JumpForce);//
                Faceleft = true;
            }

         }
    }

    void Animswitch()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
            rb.velocity = new Vector2(0, 0);//避免落地漂移
        }
    }

    /*void Death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        anim.SetTrigger("death");
    }*/
}
