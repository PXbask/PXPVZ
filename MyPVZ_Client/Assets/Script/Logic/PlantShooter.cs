using Manager;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShooter : PlantLogic
{
    GameObject bulletPrefab;
    public override void Init(Plant plant)
    {
        base.Init(plant);
        if (plant.Bullet == 0) return;
        Bullet bullet = DataManager.Instance.Bullets[plant.Bullet];
        bulletPrefab = Resources.Load<GameObject>(bullet.PrefabRes);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void DoAction()
    {
        if(!CheckActionScope()) return;
        if(!isReady) return;
        if (bulletPrefab == null) return;
        GameObject obj = Instantiate(bulletPrefab, bulletRoot.position, Quaternion.identity, bulletRoot);
        BulletLogic bulletLogic = obj.GetComponent<BulletLogic>();
        bulletLogic.Init(plantDefine);
        AfterAction();
    }

    public override bool CheckActionScope()
    {
        return true;
    }
}
