using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector2 dir = Vector2.zero;
    private float speed = 5f;
    private bool go = false;

    public void OnEnable()
    {
        Invoke("Waiting", 0.5f);
    }

    private void Waiting()
    {
        go = true;
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
    }

    private void FixedUpdate()
    {
        if (go)
            transform.Translate(dir * speed * Time.deltaTime);
    }
}
