using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class GameManager : Singleton<GameManager>
{
    public enum TYPE
    {
        PRACTICE,
        INFINITY,
        BINGE,
        NONE
    }

    public TYPE type = TYPE.NONE;

    public SpawnArrow[] spawnArrow;

    public GameObject walls;
    public GameObject[] arrowPrefab;
    public GameObject objectPoolParent;
    public GameObject resultPanel;

    public Text time;
    public Text health;

    public List<Pattern> patternList = new List<Pattern>();
    private List<GameObject> fastArrowList = new List<GameObject>();
    private List<GameObject> slowArrowList = new List<GameObject>();

    public AudioSource arrowAudio;
    public AudioClip audioclip;

    private int[] tempIdx;
    private ObscuredInt idx = 0;

    private ObscuredFloat timeCount;

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

    private void Update()
    {
        if(type == TYPE.INFINITY)
        {
            timeCount += Time.deltaTime;

            time.text = "버틴 시간 : " + timeCount.ToString("0.00") + "초";
        }
    }

    public void GetResult()
    {
        AdmobScreenAd.instance.Show();
        StopAllCoroutines();
        resultPanel.SetActive(true);

        if(type == TYPE.BINGE)
        {
            if (ObscuredPrefs.HasKey("BINGESCORE"))
            {
                if (idx >= ObscuredPrefs.GetInt("BINGESCORE"))
                {
                    ObscuredPrefs.SetInt("BINGESCORE", idx);
                }
            }
            else
            {
                ObscuredPrefs.SetInt("BINGESCORE", idx);
            }

            if(idx == 100)
            {
                if (!Social.localUser.authenticated)
                {
                    Social.localUser.Authenticate((bool success) =>
                    {
                        if (success)
                        {
                            if (Player.instance.hp == 5)
                            {
                                Social.ReportProgress(GPGSIds.achievement_2, 100f, null);
                            }
                            else
                            {
                                Social.ReportProgress(GPGSIds.achievement, 100f, null);
                            }
                        }
                    });
                }
                else
                {
                    if (Player.instance.hp == 5)
                    {
                        Social.ReportProgress(GPGSIds.achievement_2, 100f, null);
                    }
                    else
                    {
                        Social.ReportProgress(GPGSIds.achievement, 100f, null);
                    }
                }
            }

            resultPanel.transform.Find("Result").GetComponent<Text>().text = "진행 상황 : " + idx + " / 100";
            resultPanel.transform.Find("BestResult").GetComponent<Text>().text = "최고 기록 : " + ObscuredPrefs.GetInt("BINGESCORE") + " / 100";
        }
        else if (type == TYPE.INFINITY)
        {
            if (ObscuredPrefs.HasKey("INFINITYSCORE"))
            {
                if(timeCount >= ObscuredPrefs.GetFloat("INFINITYSCORE"))
                {
                    ObscuredPrefs.SetFloat("INFINITYSCORE", timeCount);
                }
            }
            else
            {
                PlayerPrefs.SetFloat("INFINITYSCORE", timeCount);
            }

            if (!Social.localUser.authenticated)
            {
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        if (timeCount >= 300)
                        {
                            Social.ReportProgress(GPGSIds.achievement___300, 100f, null);
                        }

                        if (timeCount >= 200)
                        {
                            Social.ReportProgress(GPGSIds.achievement___200, 100f, null);
                        }

                        if (timeCount >= 100)
                        {
                            Social.ReportProgress(GPGSIds.achievement___100, 100f, null);
                        }
                    }
                });
            }
            else
            {
                if (timeCount >= 300)
                {
                    Social.ReportProgress(GPGSIds.achievement___300, 100f, null);
                }

                if (timeCount >= 200)
                {
                    Social.ReportProgress(GPGSIds.achievement___200, 100f, null);
                }

                if (timeCount >= 100)
                {
                    Social.ReportProgress(GPGSIds.achievement___100, 100f, null);
                }
            }

            resultPanel.transform.Find("Result").GetComponent<Text>().text = "버틴 시간 : " + timeCount.ToString("0.00") + "초";
            resultPanel.transform.Find("BestResult").GetComponent<Text>().text = "최고 기록 : " + PlayerPrefs.GetFloat("INFINITYSCORE").ToString("0.00") + "초";
        }
    }

    public void PracticeButton(bool _plus)
    {
        if (_plus)
        {
            idx++;

            if(idx >= patternList.Count)
            {
                idx = 0;
            }
        }
        else
        {
            idx--;

            if(idx < 0)
            {
                idx = patternList.Count - 1;
            }
        }

        time.text = "다음 패턴 : " + (idx + 1)+ "번";
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

            switch (type)
            {
                case TYPE.PRACTICE:
                    break;
                case TYPE.INFINITY:
                    idx = Random.Range(0, patternList.Count);
                    break;
                case TYPE.BINGE:
                    if(idx == 100)
                    {
                        GetResult();
                        yield break;
                    }

                    time.text = "진행 상황 : " + (idx + 1) + " / 100";
                    break;
            }

            yield return StartCoroutine(StartPattern(patternList[idx]));
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

    public void AudioPlay()
    {
        arrowAudio.PlayOneShot(audioclip);
    }
    private bool first = false;

    private IEnumerator StartPattern(Pattern _pattern)
    {
        first = false;

        if(type == TYPE.BINGE)
        {
            idx++;
        }

        if(_pattern.type == Pattern.TYPE.RANDOM)
        {
            tempIdx = null;
            tempIdx = RandomIdx(_pattern.patternLength);
        }

        for (int j = 0; j < _pattern.patternLength; j++)
        {
            GameObject arrowObject = null;
            Arrow arrow = null;

            if (_pattern.arrowType[j] == 0)
            {
                arrowObject = GetGameobejct(0);
            }
            else if (_pattern.arrowType[j] == 1)
            {
                arrowObject = GetGameobejct(1);
            }

            arrow = arrowObject.GetComponent<Arrow>();

            if (_pattern.patternWaitingTime[j] == 0)
            {
                yield return null;
                if (!first)
                {
                    first = true;
                    arrow.soundPlay = true;
                }
            }
            else
            {
                yield return new WaitForSeconds(_pattern.patternWaitingTime[j]);
                arrow.soundPlay = true;
            }

            SpawnArrow dir = null;

            if(_pattern.type == Pattern.TYPE.CONST)
            {
                arrowObject.transform.position = spawnArrow[_pattern.spawnIndex[j]].transform.position;
                dir = spawnArrow[_pattern.spawnIndex[j]];
            }
            else if(_pattern.type == Pattern.TYPE.RANDOM)
            {             
                arrowObject.transform.position = spawnArrow[tempIdx[j]].transform.position;
                dir = spawnArrow[tempIdx[j]];
            }

            dir.CoroutineControl();
            arrow.DirectionSetting(dir.wasd);
            arrowObject.SetActive(true);
        }
    }
}
