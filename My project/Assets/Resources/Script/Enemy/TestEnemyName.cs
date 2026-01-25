using UnityEngine;

public class TestEnemyName : MonoBehaviour
{
    private BehaviourTree tree;
    private Animator animator;
    private BaseStat stat;

    [SerializeField] private GameObject hurtCollider;

    public GameObject player;

    public LayerMask groundMask;

    private const float cliffMaxDistnace = 0.5f;
    //private float checkOffsetX = 0.8f;
    //private float checkOffsetY = 0.5f; 
    private const float moveSpeed = 1.0f;
    private float detectDistance = 7f;

    #region Unity method
    private void Awake()
    {
        animator = GetComponent<Animator>();
        stat = GetComponent<BaseStat>();

        if (!player)
        {
            player = GameObject.Find("Player");
        }
    }

    private void Start()
    {
        TreeSetting();
    }

    private void Update()
    {
        tree.Process();
    }

    private void TreeSetting()
    {
        tree = new BehaviourTree("Name");

        PrioritySelector NameSelector = new PrioritySelector("NameSelector");

        Sequence deathSQ = new Sequence("Death", 100);
        deathSQ.AddChild(new Leaf("IsDeath?", new Condition(IsDeath)));
        deathSQ.AddChild(new Leaf("Death", new Death(gameObject, hurtCollider)));
        NameSelector.AddChild(deathSQ);

        Sequence walkToPlayerSQ = new Sequence("Move", 10);
        walkToPlayerSQ.AddChild(new Leaf("IsSafeToMove?", new Condition(IsSafeToMove)));
        walkToPlayerSQ.AddChild(new Leaf("IsPlayerInRange?", new Condition(IsPlayerInRange)));
        walkToPlayerSQ.AddChild(new Leaf("MoveToPlayer", new MoveToTarget(transform, player.transform, moveSpeed, 2f)));
        NameSelector.AddChild(walkToPlayerSQ);

        Sequence patrolSQ = new Sequence("Patrol", 5);
        patrolSQ.AddChild(new Leaf("IsSafeToMove?", new Condition(IsSafeToMove)));
        patrolSQ.AddChild(new Leaf("PatrolMove", new Patrol(transform, 8f, moveSpeed)));
        NameSelector.AddChild(patrolSQ);

        Sequence idleSQ = new Sequence("Idle", 1);
        idleSQ.AddChild(new Leaf("IsCliff?", new Condition(() => !IsSafeToMove())));
        idleSQ.AddChild(new Leaf("Idle", new Idle(transform)));

        tree.AddChild(NameSelector);
    }
    #endregion

    #region Condition Checker
    private bool IsSafeToMove()
    {

        float directionX = (Mathf.Abs(transform.localEulerAngles.y - 180f) < 0.1f) ? 1f : -1f;


        Vector2 origin = new Vector2(
            transform.position.x + (directionX * 1.0f), 
            transform.position.y - 1.0f         
        );

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            cliffMaxDistnace,
            groundMask
        );

        Debug.DrawRay(origin, Vector2.down * cliffMaxDistnace, Color.red);

        return hit.collider != null;
    }

    private bool IsPlayerInRange()
    {
        if(player == null)
        {
            return false;
        }
        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
        return distance <= detectDistance;
    }

    private bool IsDeath()
    {
        Debug.Log("Monster Death");
        if(stat.CurrentHP <=0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion


}
