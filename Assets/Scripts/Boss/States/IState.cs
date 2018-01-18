﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<in T>
{
    void EnterState(T owner);
    void UpdateState(T owner);
    void ExitState(T owner);
}
