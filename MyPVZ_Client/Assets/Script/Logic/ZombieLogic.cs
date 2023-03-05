using Manager;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieLogic : MonoBehaviour
{
    public int postion;
    public Zombie zombieDefine;
    public float speed;
    Animator animator;
    SpriteRenderer spriteRenderer;
    int maxHp;
    public int currentHp;
    Rigidbody2D rgb;
    float attackTimer;
    PlantLogic plantLogic;//Attack Obj
    public Transform equipRoot;
    bool isDead;
    bool IsAttacking => plantLogic != null;
    bool canMove;
    public SpawnState spawnState;
    public Queue<EquipLogic> equipQueue = new Queue<EquipLogic>();
    EquipLogic CurrentEquip
    {
        get
        {
            if (equipQueue.Count == 0) return null;
            return equipQueue.Peek();
        }
    }
    private void Awake()
    {
        animator= GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rgb = GetComponent<Rigidbody2D>();
        spawnState = SpawnState.None;
    }
    public void Init(Zombie zombie, SpawnPoint spawnPoint, SpawnState spawnState, bool canMove)
    {
        if(spawnPoint!= null)
            this.postion = spawnPoint.index;
        spriteRenderer.sortingOrder = postion;
        zombieDefine = zombie;
        speed = zombieDefine.Speed;
        this.spawnState = spawnState;
        this.canMove = canMove;

        maxHp = zombieDefine.HP;
        currentHp = zombieDefine.HP;

        attackTimer = 0;
        isDead= false;

        EquipItems();
    }

    private void EquipItems()
    {
        var items = zombieDefine.Equipments;
        if (items == null) return;
        foreach (var item in items)
        {
            Equipment equip = DataManager.Instance.Equipments[item];
            GameObject prefab = Resources.Load<GameObject>(equip.AssetPath);
            var obj = Instantiate(prefab, equipRoot.position, Quaternion.identity, equipRoot);
            var logic = obj.GetComponent<EquipLogic>();
            logic.Init(equip, this);

            equipQueue.Enqueue(logic);
        }
    }

    private void Update()
    {
        if(isDead) return;
        if (IsAttacking)
        {
            attackTimer -= Time.deltaTime;
            return;
        }
        if (!IsAttacking && animator.GetBool("attack"))
            animator.SetBool("attack", false);
        DoMove();
    }

    private void DoMove()
    {
        if(canMove)
        {
            Vector3 currentPos = transform.position;
            currentPos.x -= speed * Time.deltaTime;
            transform.position = currentPos;
        }
    }
    public void DoHit(Bullet bulletDefine)
    {
        if(isDead) return;
        if (CurrentEquip != null)
        {
            CurrentEquip.DoHit(bulletDefine);
            return;
        }
        currentHp -= bulletDefine.Damage;
        if (currentHp <= 0) StartCoroutine(DoDeath());
    }

    IEnumerator DoDeath()
    {
        animator.SetTrigger("dead");
        isDead = true;
        rgb.simulated = false;
        yield return new WaitForSeconds(4f);
        if (spawnState != SpawnState.Wave)
            if (zombieDefine != null)
                LevelManager.Instance.waveProgress += zombieDefine.Menace;
        Destroy(gameObject);
        GameObjectManager.Instance.RemoveEnemy(postion, gameObject);
        LevelManager.Instance.lastPosEnemyDead = transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 7) return;
        if(collision.gameObject.tag.Equals(postion.ToString()))
        {
            animator.SetBool("attack", true);
            if (plantLogic == null)
                plantLogic = collision.GetComponent<PlantLogic>();
            if (attackTimer <= 0)
            {
                plantLogic.DoHit(zombieDefine);
                attackTimer = zombieDefine.AttackInterval;
            }
        }
    }
}
