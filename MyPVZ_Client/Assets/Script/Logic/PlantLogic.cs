using Manager;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlantLogic : MonoBehaviour, IBattleReact
{
    public Plant plantDefine;
    protected float Timer;
    public bool isReady;
    public Transform bulletRoot;
    public Vector2Int numPostion;
    int maxHP;
    public int currentHp;
    public virtual void Init(Plant plant)
    {
        plantDefine = plant;
        Timer = plantDefine.FireSpeed;
        isReady = false;

        maxHP = plantDefine.HP; 
        currentHp = plantDefine.HP;
    }
    private void Update()
    {
        OnUpdate();
    }
    protected virtual void OnUpdate()
    {
        if (isReady) return;
        Timer -= Time.deltaTime;
        if (Timer < 0) {
            isReady= true;
        }
    }
    public abstract void DoAction();
    public abstract bool CheckActionScope();
    protected virtual void AfterAction()
    {
        isReady= false;
        Timer = plantDefine.FireSpeed;
    }

    public void DoHit(Zombie zombieDefine)
    {
        currentHp -= zombieDefine.Damage*10;
        if (currentHp < 0)
        {
            DoDeath();
        }
    }

    private void DoDeath()
    {
        Destroy(gameObject);
        MapManager.Instance.RemovePlant(numPostion);
        GameObjectManager.Instance.RemovePlant(numPostion);
    }
    public void SetNumPosition(Vector2Int pos)
    {
        numPostion = pos;
        gameObject.tag = pos.x.ToString();
    }
}
