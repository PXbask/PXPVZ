using Manager;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlantProducer : PlantLogic
{
    GameObject productPrefab;
    public override void Init(Plant plant)
    {
        base.Init(plant);
        Timer = UnityEngine.Random.Range(2, 6);
        Bullet bullet = DataManager.Instance.Bullets[plant.Bullet];
        productPrefab = Resources.Load<GameObject>(bullet.PrefabRes);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(isReady)
        {
            Produce();
            AfterAction();
        }
    }

    private void Produce()
    {
        GameObject obj = Instantiate(productPrefab, bulletRoot.position, Quaternion.identity, bulletRoot);
        BulletLogic bulletLogic = obj.GetComponent<BulletLogic>();
        bulletLogic.Init(plantDefine);
    }

    public override void DoAction()
    {
        
    }
    public override bool CheckActionScope()
    {
        return true;
    }
}
