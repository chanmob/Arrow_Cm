using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider2D;
    private Vector2 dir = Vector2.zero;
    public float speed = 5f;
    public float waitingTime = 0.5f;
    private bool go = false;

    private void Start()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    public void OnEnable()
    {
        Invoke("Waiting", waitingTime);
    }

    public void OnDisable()
    {
        //capsuleCollider2D.enabled = false;
    }

    private void Waiting()
    {
        go = true;
        //capsuleCollider2D.enabled = true;
    }

    public void DirectionSetting(float _time, SpawnArrow.WASD _dir)
    {
        waitingTime = _time;
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
            Debug.Log("LEFT 충돌");
        }
        else if (dir == Vector2.right && collision.CompareTag("LeftGround"))
        {
            Debug.Log("RIGHT 충돌");
        }
    }
}
