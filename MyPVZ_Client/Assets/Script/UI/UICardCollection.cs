using Manager;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICardCollection : MonoBehaviour
{
    List<UICardItem> plantCards;
    Transform root;
    public GameObject itemPrefab;
    public Action<UICardItem, bool> OnCardItemSelectAction;
    private void Awake()
    {
        root = gameObject.transform;
        plantCards = new List<UICardItem>(10);
    }
    private void Start()
    {
        OnCardItemSelectAction = LevelManager.Instance.OnCardItemSelectAction;
    }
    public void Init(bool active = false, UICardItem cardItem = null, bool selected = true)
    {
        RefreshUI(cardItem, selected);
        gameObject.SetActive(active);
    }
    public void RefreshUI(UICardItem cardItem, bool selected)
    {
        if (cardItem == null) return;
        if(selected)
        {
            GameObject obj = Instantiate(itemPrefab, root);
            UICardItem card = obj.GetComponent<UICardItem>();
            card.SetBarData(cardItem, this);
            plantCards.Add(cardItem);
        }
        else
        {
            UICardItem item = FindRelatedItem(cardItem);
            Destroy(cardItem.gameObject);
            plantCards.Remove(item);
        }
    }
    public void Update()
    {
        //foreach (var item in plantCards)
        //{
        //    item.OnUpdate();
        //}
    }
    public void Clear()
    {
        for(int i=0;i<plantCards.Count;i++)
        {
            if (plantCards[i]!=null)
                Destroy(plantCards[i].gameObject);
        }
        plantCards.Clear();
    }
    public UICardItem FindRelatedItem(UICardItem card)
    {
        foreach (var item in plantCards)
        {
            if(item.plantDefine == card.plantDefine)
                return item;
        }
        return null;
    }
    private void OnDestroy()
    {
        
    }
}
