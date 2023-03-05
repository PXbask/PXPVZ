using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public enum PlantType
    {
        None,
        Attack,
        Defense,
        Assist,
    }
    public class Plant
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HP { get; set; }
        public float FireSpeed { get; set; }
        public int Bullet { get; set; }
        public PlantType Type { get; set; }
        public float CoolTime { get; set; }
        public string ImageResPath { get; set; }
        public string PrefabResPath { get; set; }
        public BattleScope Scope { get; set; }
        public int Cost { get; set; }
    }
}

