using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Level
    {
        public int ID { get; set; }
        public int Chapter { get; set; }
        public int Levels { get; set; }
        public string SceneRes { get; set; }
        public List<int> ZombieTypes { get; set; }
        public float Difficulty { get; set; }
        public List<int> FlagProgress { get; set; }
        public float Urgentment { get; set; }
        public int MaxCount { get; set; }
        public float CountRatio { get; set; }
    }
}


