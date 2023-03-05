using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    SelectCard,
    Battle,
    Settle,
    Failed,
}
public enum BattleScope
{
    One = 0,
    Three = 1,
    Five = 2,
    ALL = 3,
}
public enum BulletType
{
    None,
    Shot,
    Throw,
    Produce,
}
public enum SpawnState
{
    None,
    Idle,
    Warming,
    Warmed,
    WaveWarming,
    Wave,
    End
}
