using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnTargets;

    void Start ()
    {
        if (spawnTargets != null && spawnTargets.Length > 0)
        {
            StartCoroutine(SpawnIterator());
        }
    }

    private IEnumerator SpawnIterator()
    {
        while (true)
        {
            var target = spawnTargets[Random.Range(0, spawnTargets.Length)];
            var go = Instantiate(target,transform.position,Quaternion.identity) as GameObject;
            Destroy(go, 5.0f);   //画面外判定とか面倒なので、5秒後に自殺
            yield return new WaitForSeconds(5);
        }
    }
}
