using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    public enum TYPE
    {
        RANDOM,
        CONST
    }

    public TYPE type = TYPE.CONST;

    public int patternLength;           //패턴의 화살 생성 갯수
    public int[] spawnIndex;            //화살 생성 위치
    public float[] patternWaitingTime;  //패턴간 화살 생성 간격
    public float[] waitingTime;         //생성 후 화살 대기 시간
}
