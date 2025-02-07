using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    public int _enemyCount = 50;

    void Start()
    {
        SpawnEnemies(_enemyCount);
    }

    public void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-100, 100), 0f, Random.Range(-100, 100));
            Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

}
