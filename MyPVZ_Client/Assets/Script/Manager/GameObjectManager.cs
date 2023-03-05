using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : Singleton<GameObjectManager>
{
    public Dictionary<Vector2Int, GameObject> AllPlantObjects = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<int, List<GameObject>> RowEnemys = new Dictionary<int, List<GameObject>>();
    int enemyCount = 0;
    public void Reset()
    {
        Clear();
    }
    public void Clear()
    {
        foreach (var item in AllPlantObjects)
        {
            UnityEngine.Object.Destroy(item.Value);
        }
        foreach (var item in RowEnemys)
        {
            for(int i = 0; i < item.Value.Count; i++)
            {
                UnityEngine.Object.Destroy(item.Value[i]);
            }
        }
        AllPlantObjects.Clear();
        RowEnemys.Clear();
    }
    public void AddPlant(Vector2Int pos,GameObject plant)
    {
        AllPlantObjects.Add(pos, plant);
    }
    public void RemovePlant(Vector2Int pos)
    {
        AllPlantObjects.Remove(pos);
    }
    public void AddEnemy(int row,GameObject enemy)
    {
        List<GameObject> list;
        if(!RowEnemys.TryGetValue(row, out list))
        {
            list= new List<GameObject>();
            RowEnemys.Add(row, list);
        }
        list.Add(enemy);
        enemyCount++;
    }
    public void RemoveEnemy(int row, GameObject enemy)
    {
        List<GameObject> list;
        if (RowEnemys.TryGetValue(row, out list))
        {
            list.Remove(enemy);
        }
        enemyCount--;
    }
    public int GetAllEnemyCounts()
    {
        return enemyCount;
    }
}
