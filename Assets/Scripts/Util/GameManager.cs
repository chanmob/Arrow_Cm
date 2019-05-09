using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private SpawnArrow[] spawnArrow;
    public GameObject walls;
    public GameObject arrowPrefab;
    public GameObject obejctPoolParent;

    public List<Pattern> patternList = new List<Pattern>();
    private List<GameObject> arrowList = new List<GameObject>();

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

    private void InitializeObjectPools()
    {
        GameObject newArrow = null;

        for (int i = 0; i < 20; i++)
        {
            newArrow = Instantiate(arrowPrefab);
            newArrow.transform.SetParent(obejctPoolParent.transform);
            newArrow.SetActive(false);
            arrowList.Add(newArrow);
        }
    }

    public GameObject GetGameobejct()
    {
        GameObject result = null;

        if (arrowList.Count > 0)
        {
            result = arrowList[0];
            arrowList.Remove(arrowList[0]);
            result.gameObject.SetActive(true);
        }
        else
        {
            result = Instantiate(arrowPrefab);
            result.transform.SetParent(obejctPoolParent.transform);
        }

        return result;
    }

    public void DisableGameobject(GameObject _object)
    {
        _object.SetActive(false);
        arrowList.Add(_object);
    }

    private IEnumerator StartSpawnArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            int idx = Random.Range(0, patternList.Count - 1);

            yield return StartCoroutine(StartPattern(patternList[idx]));
        }
    }

    private IEnumerator StartPattern(Pattern _pattern)
    {
        for (int j = 0; j < _pattern.patternLength; j++)
        {
            yield return new WaitForSeconds(_pattern.patternWaitingTime[j]);

            var arrow = GetGameobejct();
            SpawnArrow dir = null;

            if(_pattern.type == Pattern.TYPE.CONST)
            {
                arrow.transform.position = spawnArrow[_pattern.spawnIndex[j]].transform.position;
                dir = spawnArrow[_pattern.spawnIndex[j]];
            }
            else if(_pattern.type == Pattern.TYPE.RANDOM)
            {
                int randomIdx = Random.Range(0, spawnArrow.Length);
                arrow.transform.position = spawnArrow[randomIdx].transform.position;
                dir = spawnArrow[randomIdx];
            }

            arrow.GetComponent<Arrow>().DirectionSetting(_pattern.waitingTime[j], dir.wasd);
        }
    }
}
