using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int hp = 5;

    public float speed = 1000f;
    public float jump_power = 8.5f;
    private float jump_force_count;

    private bool grounded;
    private bool leftBtnPress;
    private bool rightBtnPress;
    private bool jumpBtnPress;
    private bool sitBtnPress;
    private bool player_looks_right;
    private bool invincibility;
    public bool isDead;

    public GameObject mobileController;

    private Animator anim;
    private Rigidbody2D rb2d;
    private Vector3 my_sprite_originalscale;
    private BoxCollider2D box2d;
    private SpriteRenderer[] sprites;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        player_looks_right = false;
        my_sprite_originalscale = transform.localScale;
    }

    void Update()
    {
        if (isDead == true)
            return;

        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("Ground", grounded);
        anim.SetBool("Sit", sitBtnPress);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftButtonPress();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            LeftButtonRelease();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightButtonPress();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            RightButtonRelease();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButtonPress();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpButtonRelease();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SitButtonPress();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            SitButtonRelease();
        }
    }

    void FixedUpdate()
    {
        if (isDead == true)
            return;

        if (!sitBtnPress)
        {
            if (leftBtnPress == true && rightBtnPress == false)
            {
                if (player_looks_right == true)
                {
                    player_looks_right = false;
                    transform.localScale = my_sprite_originalscale;
                }
                rb2d.AddForce(Vector2.left * speed * Time.deltaTime);
            }

            else if (leftBtnPress == false && rightBtnPress == true)
            {
                if (player_looks_right == false)
                {
                    player_looks_right = true;
                    transform.localScale = new Vector3(-my_sprite_originalscale.x, my_sprite_originalscale.y, my_sprite_originalscale.z);
                }
                rb2d.AddForce(Vector2.right * speed * Time.deltaTime);
            }

            if (jumpBtnPress == true && jump_force_count > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jump_power);
                jump_force_count -= 0.1f * Time.deltaTime;
            }
        }

        if(grounded && sitBtnPress)
        {
            rb2d.velocity = new Vector2(0, 0);
            box2d.offset = new Vector2(-0.0246f, 0.0550f);
            box2d.size = new Vector2(0.7663f, 0.9722f);
        }

        if (grounded == true)
        {
            rb2d.gravityScale = 1f;
        }
        else
        {
            if (rb2d.gravityScale != 2.5f)
            {
                rb2d.gravityScale = 2.5f;
            }
        }
    }

    public void LeftButtonPress()
    {
        leftBtnPress = true;
    }

    public void LeftButtonRelease()
    {
        leftBtnPress = false;

        if (grounded)
            rb2d.velocity = new Vector2(0, 0);
    }

    public void RightButtonPress()
    {
        rightBtnPress = true;
    }

    public void RightButtonRelease()
    {
        rightBtnPress = false;

        if (grounded)
            rb2d.velocity = new Vector2(0, 0);
    }

    public void JumpButtonPress()
    {
        jumpBtnPress = true;

        if (sitBtnPress)
            return;

        if (grounded)
        {
            jump_force_count = 0.02f;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f) + new Vector2(transform.up.x, transform.up.y) * jump_power;
            grounded = false;
        }
    }

    public void JumpButtonRelease()
    {
        jump_force_count = 0f;
        jumpBtnPress = false;
    }

    public void SitButtonPress()
    {
        sitBtnPress = true;
    }

    public void SitButtonRelease()
    {
        sitBtnPress = false;

        box2d.offset = new Vector2(-0.0246f, 0.2066f);
        box2d.size = new Vector2(0.7663f, 1.2754f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.contacts[0].normal.y > 0.9f)
        {
            grounded = true;
        }
    }

    public void Hit()
    {
        if (invincibility == true || isDead)
            return;

        hp--;
        GameManager.instance.health.text = "남은 목숨 : " + hp;

        if (hp <= 0 && GameManager.instance.type != GameManager.TYPE.PRACTICE)
        {
            isDead = true;
            mobileController.SetActive(false);
            anim.SetTrigger("Die");
            GameManager.instance.GetResult();
            return;
        }

        invincibility = true;
        StartCoroutine(HitDisplay());
    }

    public IEnumerator HitDisplay()
    {
        float increment = 0.02f / 0.25f;

        float progress = 0f;
        while (progress < 1f)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = Color.Lerp(Color.white, Color.red, progress);
                progress += increment;
                yield return new WaitForSeconds(0.02f);
            }
        }

        progress = 0;
        while (progress < 1f)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = Color.Lerp(Color.red, Color.white, progress);
                progress += increment;
                yield return new WaitForSeconds(0.02f);
            }
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = Color.white;
        }

        invincibility = false;
    }
}