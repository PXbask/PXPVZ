using Manager;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2Int numPosition;
    public bool isOccupied;
    public PlantLogic plantLogic;
    public Vector3 Position { get => transform.position; }
    public int Column { get=> numPosition.y; }
    public int Row { get => numPosition.x; }
    private void Awake()
    {
        isOccupied= false;
    }
    private void OnMouseDown()
    {
        if (LevelManager.Instance.state == GameState.Battle)
            if (LevelManager.Instance.selectedCard != null)
                Plant(LevelManager.Instance.selectedCard);
    }
    private void Plant(UICardItem cardItem)
    {
        if (isOccupied) return;
        Plant plant = cardItem.plantDefine;
        GameObject obj = Resources.Load<GameObject>(plant.PrefabResPath);
        GameObject plantObj = Instantiate(obj, Position, Quaternion.identity, transform);
        plantLogic = plantObj.GetComponent<PlantLogic>();
        plantLogic.Init(plant);
        plantLogic.SetNumPosition(numPosition);

        isOccupied= true;
        cardItem.cardState = UICardItem.CardItemState.Selected;
        cardItem.ResetCooling();
        LevelManager.Instance.selectedCard = null;

        MapManager.Instance.AddPlant(numPosition, plant);
        GameObjectManager.Instance.AddPlant(numPosition,plantObj);
        BattleManager.Instance.RegisterBattleReact(this, plantLogic);
        Debug.LogFormat("Block({0}:{1}) 种植成功 植物:{2}", numPosition.x, numPosition.y, plant.Name);

        LevelManager.Instance.SunTotal -= plant.Cost;
    }
    public void OnPlantRemove()
    {
        if (isOccupied)
        {
            isOccupied = false;
            plantLogic = null;
        }
    }
}
