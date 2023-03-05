using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EquipLogic : MonoBehaviour
{
    public Equipment equipDefine;
    public int MaxHp => equipDefine.HP;
    public List<Sprite> stateSprites = new List<Sprite>();
    SpriteRenderer spriteRenderer;

    public int stateIndex;
    public int StateCount => stateSprites.Count;
    public int currentHp;
    public int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = value;
            OnCurrentHpChanged();
        }
    }
    ZombieLogic zombie;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Equip";
    }
    public void Init(Equipment equipment, ZombieLogic zombie)
    {
        equipDefine = equipment;
        this.zombie = zombie;
        currentHp = equipDefine.HP; 
        if(stateSprites!=null && StateCount > 0)
        {
            stateIndex = StateCount - 1;
            spriteRenderer.sprite = stateSprites[stateIndex];
        }
    }
    public void OnCurrentHpChanged()
    {
        if(CurrentHp <= 0)
        {
            Destroy(gameObject);
            zombie.equipQueue.Dequeue();
            return;
        }
        int index;
        if (CurrentHp == MaxHp) index = StateCount - 1;
        else
            index = (CurrentHp * StateCount) / MaxHp;
        if (index != stateIndex)
        {
            spriteRenderer.sprite = stateSprites[index];
            stateIndex = index;
        }
    }
    public void DoHit(Bullet bulletDefine)
    {
        CurrentHp -= bulletDefine.Damage;
    }
}
