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
}
