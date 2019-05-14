using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class Arrow : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider2D;
    private Vector2 dir = Vector2.zero;
    public ObscuredFloat speed = 5f;
    public ObscuredFloat waitingTime = 1f;
    private bool go = false;
    public bool soundPlay = false;

    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    public void OnDisable()
    {
        go = false;
        capsuleCollider2D.enabled = false;
        soundPlay = false;
    }

    private void Waiting()
    {
        go = true;
        capsuleCollider2D.enabled = true;

        if (soundPlay)
        {
            var gm = GameManager.instance;
            gm.arrowAudio.PlayOneShot(gm.audioclip);
        }
    }

    public void DirectionSetting(SpawnArrow.WASD _dir)
    {
        switch (_dir)
        {
            case SpawnArrow.WASD.DOWN:
                dir = Vector2.left;
                transform.eulerAngles = new Vector3(0, 0, 90);
                transform.localScale = new Vector3(0.5f, 0.5f, 1);
                break;
            case SpawnArrow.WASD.UP:
                dir = Vector2.right;
                transform.eulerAngles = new Vector3(0, 0, 90);
                transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                break;
            case SpawnArrow.WASD.LEFT:
                dir = Vector2.left;
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.localScale = new Vector3(0.5f, 0.5f, 1);
                break;
            case SpawnArrow.WASD.RIGHT:
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                dir = Vector2.right;
                break;
        }

        Invoke("Waiting", waitingTime);
    }

    private void FixedUpdate()
    {
        if (go)
            transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dir == Vector2.left && collision.CompareTag("RightGround"))
        {
            GameManager.instance.DisableGameobject(this.gameObject);
        }
        else if (dir == Vector2.right && collision.CompareTag("LeftGround"))
        {
            GameManager.instance.DisableGameobject(this.gameObject);
        }
        else if(collision.CompareTag("Player"))
        {
            Debug.Log("Player에 닿음");
            Player.instance.Hit();
            GameManager.instance.DisableGameobject(this.gameObject);
        }
    }
}
