using DG.Tweening;
using Manager;
using Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BulletShot : BulletLogic
{
    Tween tween;
    public override void Init(Plant define)
    {
        plantDefine= define;
        bulletDefine = DataManager.Instance.Bullets[define.Bullet];
        bulletType = BulletType.Shot;
    }
    private void Start()
    {
        tween = transform.DOMoveX(13f, 4f).SetRelative().SetSpeedBased().SetEase(Ease.Linear);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(tween != null) 
            tween.Kill();
        DoOnTriggerEnter2D(collision);
    }
}
