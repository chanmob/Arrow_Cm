using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SpawnArrow[] spawnArrow;
    public GameObject walls;
    public GameObject arrow;

    void Start()
    {
        spawnArrow = walls.GetComponentsInChildren<SpawnArrow>();

        StartCoroutine(StartSpawnArrow());
    }

    private IEnumerator StartSpawnArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            var randomIdx = Random.Range(0, spawnArrow.Length);

            var arrowObject = Instantiate(arrow);
            arrowObject.transform.position = spawnArrow[randomIdx].transform.position;
            arrowObject.GetComponent<Arrow>().DirectionSetting(spawnArrow[randomIdx].wasd);
        }
    }
}
