using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public SpawnArrow[] spawnArrow;

    public GameObject walls;
    public GameObject[] arrowPrefab;
    public GameObject objectPoolParent;

    public Text text;
    public Text time;
    public Text die;

    public List<Pattern> patternList = new List<Pattern>();
    private List<GameObject> fastArrowList = new List<GameObject>();
    private List<GameObject> slowArrowList = new List<GameObject>();
    private int[] tempIdx;

    private float timeCount;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    void Start()
    {
        spawnArrow = walls.GetComponentsInChildren<SpawnArrow>();

        InitializeObjectPools();

        if(patternList.Count <= 0)
        {
            Debug.LogError("패턴 없음");
            return;
        }

        StartCoroutine(StartSpawnArrow());
    }

    int testIdx;

    private void FixedUpdate()
    {
        timeCount += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.O))
        {
            testIdx--;
            if(testIdx < 0)
            {
                testIdx = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            testIdx++;
            if(testIdx >= patternList.Count)
            {
                testIdx = patternList.Count - 1;
            }
        }

        time.text = "버틴 시간 : " + timeCount.ToString("0.00");
    }

    private void InitializeObjectPools()
    {
        GameObject newArrow = null;

        for (int i = 0; i < 20; i++)
        {
            newArrow = Instantiate(arrowPrefab[0]);
            newArrow.name = "FastArrow";
            newArrow.transform.SetParent(objectPoolParent.transform);
            newArrow.SetActive(false);
            fastArrowList.Add(newArrow);
        }

        for (int i = 0; i < 20; i++)
        {
            newArrow = Instantiate(arrowPrefab[1]);
            newArrow.name = "SlowArrow";
            newArrow.transform.SetParent(objectPoolParent.transform);
            newArrow.SetActive(false);
            slowArrowList.Add(newArrow);
        }
    }

    public GameObject GetGameobejct(int _idx)
    {
        GameObject result = null;

        switch (_idx)
        {
            case 0:
                if(fastArrowList.Count > 0)
                {
                    result = fastArrowList[0];
                    fastArrowList.Remove(fastArrowList[0]);
                }
                else
                {
                    result = Instantiate(arrowPrefab[_idx]);
                    result.transform.SetParent(objectPoolParent.transform);
                }
                break;
            case 1:
                if (slowArrowList.Count > 0)
                {
                    result = slowArrowList[0];
                    slowArrowList.Remove(slowArrowList[0]);
                }
                else
                {
                    result = Instantiate(arrowPrefab[_idx]);
                    result.transform.SetParent(objectPoolParent.transform);
                }
                break;
        }

        result.gameObject.SetActive(true);
        return result;
    }

    public void DisableGameobject(GameObject _object)
    {
        _object.SetActive(false);
        if (_object.name == "FastArrow")
            fastArrowList.Add(_object);
        else if (_object.name == "SlowArrow")
            slowArrowList.Add(_object);
    }

    private IEnumerator StartSpawnArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);

            int idx = Random.Range(0, patternList.Count);

            text.text = "이번 패턴은 " + (idx + 1) + "번째 패턴"; 
            //text.text = "테스트 " + testIdx + "번"; 

            yield return StartCoroutine(StartPattern(patternList[78]));
        }
    }

    private int[] RandomIdx(int _length)
    {
        int[] result = new int[_length];
        bool same;

        for(int i = 0; i < _length; i++)
        {
            while (true)
            {
                result[i] = Random.Range(0, spawnArrow.Length);
                same = false;

                for(int j = 0; j < i; j++)
                {
                    if(result[j] == result[i])
                    {
                        same = true;
                        break;
                    }
                }

                if (!same) break;
            }
        }

        return result;
    }

    private IEnumerator StartPattern(Pattern _pattern)
    {
        if(_pattern.type == Pattern.TYPE.RANDOM)
        {
            tempIdx = null;
            tempIdx = RandomIdx(_pattern.patternLength);
        }

        for (int j = 0; j < _pattern.patternLength; j++)
        {
            if (_pattern.patternWaitingTime[j] == 0)
                yield return null;
            else
                yield return new WaitForSeconds(_pattern.patternWaitingTime[j]);

            GameObject arrow = null;

            if(_pattern.arrowType[j] == 0)
            {
                arrow = GetGameobejct(0);
            }
            else if (_pattern.arrowType[j] == 1)
            {
                arrow = GetGameobejct(1);
            }

            SpawnArrow dir = null;

            if(_pattern.type == Pattern.TYPE.CONST)
            {
                arrow.transform.position = spawnArrow[_pattern.spawnIndex[j]].transform.position;
                dir = spawnArrow[_pattern.spawnIndex[j]];
            }
            else if(_pattern.type == Pattern.TYPE.RANDOM)
            {             
                arrow.transform.position = spawnArrow[tempIdx[j]].transform.position;
                dir = spawnArrow[tempIdx[j]];
            }

            dir.CoroutineControl();
            arrow.GetComponent<Arrow>().DirectionSetting(dir.wasd);
        }
    }
}
