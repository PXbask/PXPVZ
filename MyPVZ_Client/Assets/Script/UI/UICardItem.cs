using Manager;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardItem : MonoBehaviour
{
    public enum CardItemState
    {
        None,
        Selected,
        Disabled,
        Remain,
    }
    public UICardCollection owner;
    public Plant plantDefine;
    public bool isCoolDown;

    public Material grayMask;
    public Image coolMask;
    public Image plantImage;
    UICardItem relatedItem;
    Sprite plantSprite;
    GameState state;
    public CardItemState cardState;
    public float coolTime;
    float coolTimer;

    Action<UICardItem, bool> OnCardItemSelectAction;
    Action<GameState> OnGameStateChanged;
    private void Awake()
    {
        state = LevelManager.Instance.GameState;
    }
    private void Start()
    {
        OnGameStateChanged = (GameState s) => { state= s; };
        LevelManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    public void SetBarData(UICardItem cardItem,UICardCollection owner)
    {
        var plant = cardItem.plantDefine;
        plantDefine = plant;
        relatedItem = cardItem;
        cardItem.relatedItem = this;
        this.owner= owner;

        coolTime = plant.CoolTime;
        plantSprite = Resources.Load<Sprite>(plant.ImageResPath);
        coolTimer = coolTime;
        plantImage.sprite = plantSprite;
        cardState = CardItemState.Selected;

        Init();
    }
    public void SetGridData(Plant plant)
    {
        this.plantDefine = plant;
        plantSprite = Resources.Load<Sprite>(plant.ImageResPath);
        plantImage.sprite = plantSprite;
        cardState = CardItemState.Remain;

        Init();
    }
    public void Init()
    {
        OnCardItemSelectAction = LevelManager.Instance.OnCardItemSelectAction;
    }
    public void Update()
    {
        switch (state)
        {
            case GameState.SelectCard:
                CardSelectUpdate();
                break;
            case GameState.Battle:
                CardBattleUpdate();
                break;
        }
    }

    private void CardSelectUpdate()
    {
        plantImage.material = null;
        switch (cardState)
        {
            case CardItemState.Remain:
                coolMask.fillAmount = 0;
                coolMask.gameObject.SetActive(false);
                break;
            case CardItemState.Selected:
                coolMask.fillAmount = 0;
                coolMask.gameObject.SetActive(false);
                break;
            case CardItemState.Disabled:
                coolMask.fillAmount = 1;
                coolMask.gameObject.SetActive(true);
                break;
            default: break;
        }
    }

    private void CardBattleUpdate()
    {
        coolTimer -= Time.deltaTime;
        isCoolDown = coolTimer > 0;
        if (!isCoolDown)
        {
            if (LevelManager.Instance.SunTotal < plantDefine.Cost)
                cardState = CardItemState.Disabled;
            else
                cardState = CardItemState.Selected;
            if (cardState == CardItemState.Disabled)
            {
                coolMask.fillAmount = 1;
                coolMask.gameObject.SetActive(true);
            }
            else if(cardState == CardItemState.Selected)
            {
                plantImage.material = null;
                coolMask.fillAmount = 0;
                coolMask.gameObject.SetActive(false);
            }
        }
        else
        {
            plantImage.material = grayMask;
            coolMask.fillAmount = coolTimer / coolTime;
            coolMask.gameObject.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        if (LevelManager.Instance.OnGameStateChanged != null)
            LevelManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    public void OnClickCardItem()
    {
        Debug.Log("ClickCard:" + plantDefine.Name);
        switch (state)
        {
            case GameState.SelectCard:
                switch (cardState)
                {
                    case CardItemState.Remain:
                        OnCardItemSelectAction(this, true);
                        cardState = CardItemState.Disabled;
                        break;
                    case CardItemState.Selected:
                        relatedItem.cardState = CardItemState.Remain;
                        OnCardItemSelectAction(this, false);
                        break;
                    case CardItemState.Disabled:
                        break;
                    default: break;
                }
                break;
            case GameState.Battle:
                if (isCoolDown) return;
                switch (cardState)
                {
                    case CardItemState.Selected:
                        if (LevelManager.Instance.selectedCard!=null && LevelManager.Instance.selectedCard != this) return;
                        LevelManager.Instance.selectedCard = this;
                        cardState = CardItemState.Disabled;
                        break;
                    case CardItemState.Disabled:
                        if(LevelManager.Instance.selectedCard == this)
                        {
                            LevelManager.Instance.selectedCard = null;
                            cardState = CardItemState.Selected;
                        }
                        break;
                }
                break;
        }

    }

    public void CancalDisabledState()
    {
        this.cardState = CardItemState.Selected;
    }
    public void ResetCooling()
    {
        coolTimer = coolTime;
    }
}
