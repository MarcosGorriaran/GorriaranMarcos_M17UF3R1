using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
public class IACharacter : Character
{
    StateSO _currentNode;
    [SerializeField]
    List<StateSO> _nodes;
    [SerializeField]
    StateSO _defaultNode;
    [SerializeField]
    TargetFinder _targetFinder;
    NavMeshAgent _agent;
    bool _firstTimeSpawning = true;

    public TargetFinder TargetFinder 
    { 
        get { return _targetFinder; }
        private set { _targetFinder = value; }
    }
    public NavMeshAgent Agent
    {
        get { return _agent; }
        private set { _agent = value; }
    }
    public bool FirstTimeSpawning
    {
        get
        {
            return _firstTimeSpawning;
        }
        private set
        {
            _firstTimeSpawning=value;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>(); 
    }
    protected virtual void Start()
    {
        InitializeStateSystem();
        FirstTimeSpawning = false;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _targetFinder.onTargetedFound += CheckEndingConditions;
        _targetFinder.onTargetLost += CheckEndingConditions;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _targetFinder.onTargetedFound -= CheckEndingConditions;
        _targetFinder.onTargetLost -= CheckEndingConditions;
    }
    protected virtual void Update()
    {
        _currentNode.OnStateUpdate(this);
    }
    /**
     * <summary>
     * Check each condition listed on the End Conditions variable of the ConditionSO currently being used on this component StateSO
     * and will call ExitCurrentNode method, when at least one of the conditions are met, to properly exit it and select a new one.
     * </summary>
     */
    public void CheckEndingConditions()
    {
        Debug.Log("Checking for end conditions");
        foreach (ConditionSO condition in _currentNode.EndConditions)
            if (condition.CheckCondition(this)) ExitCurrentNode();
    }
    /**
     * <summary>
     * Calling this method directly will force this component to exit its current State and loo
     * </summary>
     */
    public void ExitCurrentNode()
    {
        bool stateChanged = false;
        foreach (StateSO stateSO in _nodes)
        {
            if (stateSO.StartCondition == null)
            {
                EnterNewState(stateSO);
                stateChanged = true;
                break;
            }
            else
            {
                if (stateSO.StartCondition.CheckCondition(this))
                {
                    EnterNewState(stateSO);
                    stateChanged = true;
                    break;
                }
            }
        }
        if (!stateChanged)
        {
            EnterNewState(_defaultNode);
        }
        _currentNode.OnStateEnter(this);
    }
    public void IALookAt(Vector3 target)
    {
        transform.LookAt(target);
        Vector3 actualRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, actualRotation.y, 0);
    }
    public void IALookAway(Vector3 target)
    {
        transform.LookAt(target);
        Vector3 actualRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, actualRotation.y+180, 0);
    }
    /**
     * <summary>
     * Used to enter a new specified state and will call the End method of the old state and the entering state of new State
     * </summary>
     * <param name="state">
     * The new state the method will change the instance into
     * </param>
     */
    private void EnterNewState(StateSO state)
    {
        _currentNode.OnStateExit(this);
        _currentNode = state;
        _currentNode.OnStateEnter(this);
    }
    private void InitializeStateSystem()
    {
        _currentNode = _defaultNode;
        _currentNode.OnStateEnter(this);
    }
    protected override void OnDeath()
    {
        gameObject.SetActive(false);
    }

    protected override void OnRevive()
    {
        EnterNewState(_defaultNode);
    }

    protected override void OnHPChange(int hpChange)
    {
        CheckEndingConditions();
    }
}
