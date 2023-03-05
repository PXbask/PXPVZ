using DG.Tweening;
using Manager;
using Model;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BulletProduce : BulletLogic
{
    Tween tween;
    public override void Init(Plant define)
    {
        plantDefine = define;
        bulletDefine = DataManager.Instance.Bullets[define.Bullet];
        bulletType = BulletType.Produce;
    }
    private void Start()
    {
        //TODO:Å×ÎïÏß      
    }
    private void Update()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z += Time.deltaTime * 45f;
        transform.eulerAngles = rotation;
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }
    private void OnMouseEnter()
    {
        tween?.Kill();
        LevelManager.Instance.SunTotal += bulletDefine.Damage;
        Destroy(gameObject);
    }
}
