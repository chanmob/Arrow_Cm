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
    public int[] arrowType;         //화살 종류

    /*
     * spawnIdx 설명
    * 아래 왼쪽부터 4/3/0/1/2
    * 왼쪽 아래부터 5/6/7/8/9
    * 위 왼쪽부터 14/13/10/11/12
    * 오른쪽 아래부터 15/16/17/18/19
    */

    //로우 33 미드 하이
}
