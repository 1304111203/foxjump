using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SecondCharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Collider2D coll;//下部圆形碰撞器
    public Collider2D crouchColl;//上部方形碰撞器
    private Animator anim;
    public Transform groundCheck;//地面检测
    public Transform cellingCheck;//头部物体检测
    public LayerMask ground;

    public float speed;//速度
    public float jumpForce;//跳跃力
    private bool isGround;//判断是否在地面
    private bool isJump;//判断是否起跳
    public int collection;//收集物（樱桃,钻石）
    public Text collectionNum;

    bool jumpPressed;//判断跳跃键是否按下
    bool cellPressed;//判断蹲下键是否按下
    bool isHurt;
    int jumpCount;//剩余跳跃次数

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        if (Input.GetButtonDown("Crouch"))
        {
            cellPressed = true;
        }
        else if(Input.GetButtonUp("Crouch")){
            cellPressed = false;
        }
        collectionNum.text = collection.ToString();//收集物数量更新
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (!isHurt)
        {
            GroundMovement();
        }
        Jump();
        Crouch();
        SwitchAnim();
    }

    //水平移动
    void GroundMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if(horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    //跳跃
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    //切换动画
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && isJump == true && rb.velocity.y > 0)//避免上坡带来的y轴分量
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y <= 0)//确保单纯下落也会有动画
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        if (isHurt)//受伤动画
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                isHurt = false;
            }
        }
        if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("fallling", false);
        }
    }

    //下蹲
    void Crouch()
    {
        if (cellPressed)
        {
            crouchColl.enabled = false;
            anim.SetBool("crouching", true);
        }
        else if (!Physics2D.OverlapCircle(cellingCheck.position, 0.1f, ground))
        {
            crouchColl.enabled = true;
            anim.SetBool("crouching", false);
            cellPressed = false;
        }
    }

    //接触触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物
        if(collision.tag == "collection")
        {
            SoundManager.masterSound.CherryAudio();
            collision.GetComponent<Animator>().Play("isGot");
        }
        //掉出地图
        if(collision.tag == "deadLine")
        {
            SoundManager.masterSound.HurtAudio();
            Invoke(nameof(ResetScene), 2f);
        }
    }

    //碰撞触发器
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemies")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce*0.8f);
                anim.SetBool("jumping", true);
            }else if (transform.position.x <= collision.gameObject.transform.position.x)
            {
                isHurt = true;
                SoundManager.masterSound.HurtAudio();
                rb.velocity = new Vector2(-10, rb.velocity.y);
            }else if (transform.position.x > collision.gameObject.transform.position.y)
            {
                isHurt = true;
                SoundManager.masterSound.HurtAudio();
                rb.velocity = new Vector2(10, rb.velocity.y);
            }
        }
    }

    //重载当前场景
    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //收集物计数
    public void CollectionCount()
    {
        collection += 1;
    }
}
