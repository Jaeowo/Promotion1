using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using static Node;

public interface IStrategy
{
    Node.EStatus Process();

    void Reset()
    {
        // Noop
    }
}

public class ActionStrategy : IStrategy
{
    readonly Action doSomething;

    public ActionStrategy(Action doSomething)
    {
        this.doSomething = doSomething;
    }

    public Node.EStatus Process()
    {
        doSomething();
        return Node.EStatus.Success;
    }
}

public class Condition : IStrategy
{
    readonly Func<bool> predicate;

    public Condition(Func<bool> predicate)
    {
        this.predicate = predicate;
    }

    public Node.EStatus Process() => predicate() ? Node.EStatus.Success : Node.EStatus.Failure;
}

//public class PatrolStrategy : IStrategy
//{
//    readonly Transform entity;
//    readonly NavMeshAgent agent;
//    readonly List<Transform> patrolPoints;
//    readonly float patrolSpeed;
//    int currentIndex;
//    bool isPathCalculated;

//    public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
//    {
//        this.entity = entity;
//        this.agent = agent;
//        this.patrolPoints = patrolPoints;
//        this.patrolSpeed = patrolSpeed;
//    }

//    public Node.EStatus Process()
//    {
//        if (currentIndex == patrolPoints.Count) return Node.EStatus.Success;

//        var target = patrolPoints[currentIndex];
//        agent.SetDestination(target.position);
//        //entity.LookAt(target.position.With(y: entity.position.y));

//        if (isPathCalculated && agent.remainingDistance < 0.1f)
//        {
//            currentIndex++;
//            isPathCalculated = false;
//        }

//        if (agent.pathPending)
//        {
//            isPathCalculated = true;
//        }

//        return Node.EStatus.Running;
//    }

//    public void Reset() => currentIndex = 0;
//}
