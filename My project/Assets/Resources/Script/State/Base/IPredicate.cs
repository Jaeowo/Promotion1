using System;
using Unity.VisualScripting;
using UnityEngine;

// Action Predicate / Func Predicate / Complex Predicate -> IPredicate
// 컨디션 체크후 불값 반환
public interface IPredicate
{
    bool Evaluate();
}


