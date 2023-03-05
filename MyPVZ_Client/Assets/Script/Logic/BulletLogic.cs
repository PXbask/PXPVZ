using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BulletLogic : MonoBehaviour
{
    public Bullet bulletDefine;
    public Plant plantDefine;
    public BulletType bulletType;
    public abstract void Init(Plant define);
    public abstract void OnTriggerEnter2D(Collider2D other);
    protected void DoOnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            ZombieLogic zombieLogic = other.gameObject.GetComponent<ZombieLogic>();
            zombieLogic.DoHit(bulletDefine);
            Destroy(gameObject);
        }
    }

}
