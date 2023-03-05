using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : Singleton<UserData>
{
    public int LevelID = 2;

    internal void Init()
    {
        Debug.Log("UserData Init");
    }
}
