using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Zombie
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int HP { get; set; }
        public List<int> Equipments { get; set; }
        public float Speed { get; set; }
        public string PrefabRes { get; set; }
        public int Damage { get; set; }
        public float AttackInterval { get; set; } 
        public int Menace { get; set; }
        public int Base { get; set; }
    }
}

