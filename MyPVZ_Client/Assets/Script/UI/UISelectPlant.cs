using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectPlant : UIWindow
{
    public GameObject itemPrefab;
    public Transform root;
    protected override void OnStart()
    {
        base.OnStart();
        Init();
    }
    private void Init()
    {
        foreach (var item in DataManager.Instance.Plants)
        {
            GameObject obj = Instantiate(itemPrefab,root);
            UICardItem cardItem = obj.GetComponent<UICardItem>();
            cardItem.SetGridData(item.Value);
        }
    }
    public override void OnYesClick()
    {
        base.OnYesClick();
        LevelManager.Instance.UnPauseMainTimeline();
    }
}
