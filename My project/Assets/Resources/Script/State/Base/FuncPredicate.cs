using System;
using UnityEngine;
public class FuncPredicate : IPredicate
{
    // Func<> - 값을 반환하는 메서드
    readonly Func<bool> func;

    public FuncPredicate(Func<bool> func)
    {
        this.func = func;
    }


    public bool Evaluate() => func.Invoke();

}