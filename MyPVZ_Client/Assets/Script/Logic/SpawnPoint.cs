using Manager;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int index;
    public Vector3 Position { get => transform.position; }
    public void InstantiateEnemy(Zombie zombie)
    {
        EnemySpawnManager.Instance.InstantiateEnemy(zombie, this);
    }
}
