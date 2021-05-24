using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public event Action<GameObject> OnSpawn;

    public void SpawnWave(WaveSO wave)
    {
        StartCoroutine(Spawn(wave));
    }

    private IEnumerator Spawn(WaveSO wave)
    {
        foreach (var enemy in wave.waveSettings)
        {
            for (int i = 0; i < enemy.Value; i++)
            {
                yield return new WaitForSeconds(wave.delayBetweenSpawn);
                var obj = Instantiate(enemy.Key.enemyPrefab);
                obj.transform.position = transform.position;
                OnSpawn?.Invoke(obj);
            }
        }
    }
}
