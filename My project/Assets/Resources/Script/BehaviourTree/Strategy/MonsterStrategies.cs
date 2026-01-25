using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class MoveToTarget : IStrategy
{
    readonly Transform entity;
    readonly Transform target;
    private float moveSpeed;
    private float stoppingDistance;

    public MoveToTarget(Transform entity, Transform target, float moveSpeed, float stoppingDistance)
    {
        this.entity = entity;
        this.target = target;
        this.moveSpeed = moveSpeed;
        this.stoppingDistance = stoppingDistance;
    }

    public Node.EStatus Process()
    {
        float distanceX = Mathf.Abs(entity.position.x - target.position.x);
        float distanceY = Mathf.Abs(entity.position.y - target.position.y);

        float directionX = target.position.x - entity.position.x;

        if (distanceY >= 1.5f)
        {
            return Node.EStatus.Failure;
        }

        if (directionX <= 0)
        {
            entity.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            entity.localRotation = Quaternion.Euler(0f, 180f, 0f);
        } 

        if (distanceX < stoppingDistance)
        {
            return Node.EStatus.Success;
        }

        Vector3 finalPosition = new Vector3(directionX * moveSpeed * Time.deltaTime, 0f, 0f);
        entity.position += new Vector3(finalPosition.x, 0f, 0f);

        return Node.EStatus.Success;
    }
}

public class AttackToTarget : IStrategy
{
    readonly GameObject attackCollider;
    private float maxAttackTime;

    private StopwatchTimer attackTimer;

    public AttackToTarget(GameObject attackCollider, float maxAttackTime = 1.0f)
    {
        this.attackCollider = attackCollider;
        this.maxAttackTime = maxAttackTime;

        attackTimer = new StopwatchTimer();
    }

    public Node.EStatus Process()
    {
        if(!attackTimer.IsRunning)
        {
            attackTimer.Start();
        }

        attackCollider.SetActive(true);

        if (attackTimer.GetTime() >= maxAttackTime)
        {
            attackCollider.SetActive(false);
            attackTimer.Reset();
            return Node.EStatus.Success;
        }
        return Node.EStatus.Running;
    }
}

public class Idle : IStrategy
{
    readonly Transform entity;

    public Idle(Transform entity)
    {
        this.entity = entity;
    }

    public Node.EStatus Process()
    {
        return Node.EStatus.Success;
    }
}

public class ChangeAnimation : IStrategy
{
    readonly Animator animator;
    private string animName;
    private string currentAnimName;
    private float conversionTime;

    public ChangeAnimation(Animator animator, string animName, float conversionTime = 0.1f)
    {
        this.animator = animator;
        this.animName = animName;
        currentAnimName = " ";
        this.conversionTime = conversionTime;
    }

    public Node.EStatus Process()
    {
        if (currentAnimName == animName)
        {
            return Node.EStatus.Failure;
        }

        animator.CrossFade(animName, conversionTime);
        currentAnimName = animName;
        return Node.EStatus.Success;
    }
}

public class Death : IStrategy
{
    private GameObject owner;
    private GameObject hurtCollider;

    public Death(GameObject owner, GameObject hurtCollider)
    {
        this.owner = owner;
        this.hurtCollider = hurtCollider;
    }

    public Node.EStatus Process()
    {
        hurtCollider.SetActive(false);
        Object.Destroy(owner, 2f);
        return Node.EStatus.Running;
    }
}

public class Patrol : IStrategy
{
    readonly Transform entity;

    private float initialX;
    private float patrolRange;
    private float moveSpeed;

    private float targetX;
    private bool hasTarget;

    public Patrol(Transform entity, float patrolRange, float moveSpeed)
    {
        this.entity = entity;
        this.patrolRange = patrolRange;
        this.moveSpeed = moveSpeed;

        initialX = entity.position.x;
        hasTarget = false;
    }


    public Node.EStatus Process()
    {
        if (!hasTarget)
        {
            targetX = initialX + Random.Range(-patrolRange, patrolRange);
            hasTarget = true;
        }

        float directionX = targetX - entity.position.x;

        if (directionX <= 0)
        {
            entity.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            entity.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }

        entity.position += Vector3.right * Mathf.Sign(directionX) * moveSpeed * Time.deltaTime;

        if (Mathf.Abs(directionX) < 0.05f)
        {
            hasTarget = false;
        }

        return Node.EStatus.Running;
    }

}