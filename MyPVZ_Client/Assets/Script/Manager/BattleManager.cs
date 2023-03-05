using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class BattleManager:Singleton<BattleManager>
    {
        public Dictionary<int, List<IBattleReact>> BattleReactMap = new Dictionary<int, List<IBattleReact>>();
        public Dictionary<int, bool> IsRowHasEnemy= new Dictionary<int, bool>();
        public void Init()
        {
            Debug.Log("BattleManager Init");
        }
        public void Reset()
        {
            BattleReactMap.Clear();
            IsRowHasEnemy.Clear();
        }
        public void RegisterBattleReact(Block block, IBattleReact react)
        {
            int row = block.numPosition.x;
            BattleScope scope = block.plantLogic.plantDefine.Scope;
            for(int i = row - (int)scope; i <= row + (int)scope; i++)
            {
                if(i > 0)
                {
                    List<IBattleReact> list;
                    if(!BattleReactMap.TryGetValue(i, out list))
                    {
                        list= new List<IBattleReact>();
                        BattleReactMap.Add(i, list);
                        IsRowHasEnemy[i] = false;
                    }
                    list.Add(react);
                    Debug.LogFormat("{0}×¢²áÕ½¶·ÐÐ:[{1}]", block.plantLogic.plantDefine.Name, i.ToString());
                }
            }
        }
        public void InvokeBattleReact(int row)
        {
            List<IBattleReact> list;
            if (BattleReactMap.TryGetValue(row,out list))
            {
                foreach (var item in list)
                {
                    item.DoAction();
                }
            }
        }
        public void OnUpdate()
        {
            foreach (var item in GameObjectManager.Instance.RowEnemys)
            {
                if(item.Value != null && item.Value.Count> 0)
                {
                    IsRowHasEnemy[item.Key] = true;
                    InvokeBattleReact(item.Key);
                }
            }
        }
    }
}

