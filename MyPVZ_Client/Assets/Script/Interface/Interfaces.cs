using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleReact
{
    void DoAction();
    bool CheckActionScope();
}
