using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;//刚体2D
    public float speed;//移动速度
    public float JumpFroce;//跳跃力度
    private Animator anim;
    public LayerMask ground;
    public Collider2D coll;
    public Collider2D crouchColl;
    public Transform cellingCheck;
    public Transform groundCheck;
    //public AudioSource bgm;
    //public AudioSource jumpAudio;//跳跃声音
    //public AudioSource gameoverAudio;
    //public AudioSource cherryAudio;

    public int Cherry;
    public Text CherryNum;//收集的樱桃数
    private int extraJump;

    private bool isHurt;//默认false
    private bool headCheck;
    private bool isGround;

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedircetion = Input.GetAxisRaw("Horizontal");
        
        //移动
        if(horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(facedircetion));
        }

        //转身
        if(facedircetion != 0)
        {
            transform.localScale = new Vector3(facedircetion, 1, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurt)
        {
            Movement();
        }   
        SwitchAnim();
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);
    }

    private void Update()
    {
        DoubleJump();
        Crouch();
        CherryNum.text = Cherry.ToString();
    }

    //动画效果
    void SwitchAnim()
    {
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))//跳跃动画
        {
            if (rb.velocity.y < 0)//跳跃转下落
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)//受伤动画
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))//落地恢复idle状态
        {
            anim.SetBool("falling", false);
        }
    }

    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集樱桃
        if (collision.tag == "collection")
        {
            SoundManager.masterSound.CherryAudio();
            collision.GetComponent<Animator>().Play("isGot");
        }
        //掉出地图
        if (collision.tag == "deadLine")
        {
            SoundManager.masterSound.HurtAudio();
            Invoke(nameof(ResetScene), 2f);
        }

    }

    //踩踏攻击
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemies")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, JumpFroce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                isHurt = true;
                SoundManager.masterSound.HurtAudio();
                rb.velocity = new Vector2(-10, rb.velocity.y);
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                isHurt = true;
                SoundManager.masterSound.HurtAudio();
                rb.velocity = new Vector2(10, rb.velocity.y);
            }
        }
    }

    //下蹲函数
    void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            crouchColl.enabled = false;
            anim.SetBool("crouching", true);
        }else if (Input.GetButtonUp("Crouch"))
        {
            headCheck = true;
        }
        if(headCheck)
        {
            if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
            {
                crouchColl.enabled = true;
                anim.SetBool("crouching", false);
                headCheck = false;
            }
        }
    }

    //重载当前场景
    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //跳跃
    /*void Jump()
    {
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpFroce * Time.deltaTime);
            jumpAudio.Play();
            anim.SetBool("jumping", true);
        }
    }*/

    //收集物计数
    public void CherryCount()
    {
        Cherry += 1;
    }

    //二段跳
    void DoubleJump()
    {
        if (isGround)
        {
            extraJump = 1;
        }
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            rb.velocity = Vector2.up * JumpFroce;//Vector2.up = Vector2(0,1)
            extraJump--;
            SoundManager.masterSound.JumpAudio();
            anim.SetBool("jumping", true);
        }
        if(Input.GetButtonDown("Jump")&&extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * JumpFroce;
            SoundManager.masterSound.JumpAudio();
            anim.SetBool("jumping", true);
        }
    }
}
