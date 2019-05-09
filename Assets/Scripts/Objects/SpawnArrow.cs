using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArrow : MonoBehaviour
{
    public enum WASD
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }

    public WASD wasd = WASD.NONE;

    private SpriteRenderer sprite;
    public IEnumerator displayCoroutine;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        displayCoroutine = SpawnDisplay();
    }

    public void CoroutineControl()
    {
        StopCoroutine(displayCoroutine);
        displayCoroutine = null;
        displayCoroutine = SpawnDisplay();
        StartCoroutine(displayCoroutine);
    }

    public IEnumerator SpawnDisplay()
    {        
        float progress = 0f;
        float increment = 0.02f / 0.5f;

        while (progress < 0.5f)
        {
            sprite.color = Color.Lerp(Color.white, Color.red, progress);
            progress += increment;
            yield return new WaitForSeconds(0.02f);
        }

        progress = 0;
        while (progress < 0.5f)
        {
            sprite.color = Color.Lerp(Color.red, Color.white, progress);
            progress += increment;
            yield return new WaitForSeconds(0.02f);
        }

        sprite.color = Color.white;
    }
}
