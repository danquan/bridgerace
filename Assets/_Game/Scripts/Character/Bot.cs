using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private LevelManager levelManager;

    private Stage currentStage;
    private IState<Bot> currentState;

    [SerializeField] private Vector3 destination;
    private bool IsReachedDestination => Vector3.Distance(tf.position, destination + (tf.position.y - destination.y) * Vector3.up) < 0.1f;
    
    override protected void Start()
    {
        base.Start();
        OnInit();
    }

    override public void OnInit()
    {
        base.OnInit();
        // Do nothing
        return;
    }

    /// <summary>
    /// Stop building stair and collect more bricks
    /// </summary>
    override public void BlockRunUp()
    {
        agent.speed = 0f;
        agent.isStopped = true;
        ChangeState(new CollectState(currentStage.GetRandNumBricks()));
        agent.isStopped = false;
    }
    override public void UnBlockRunUp()
    {
        return;
    }

    /// <summary>
    /// Make bot stop moving
    /// </summary>
    public override void Pause()
    {
        agent.isStopped = true;
        this.PauseAnimation();
    }
    /// <summary>
    /// Make bot continue moving
    /// </summary>
    public override void Resume()
    {
        agent.isStopped = false;
        this.ResumeAnimation();
    }

    /// <summary>
    /// Set Destination for bot
    /// </summary>
    /// <param name="destination"></param>
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
        agent.speed = moveSpeed;
    }
    
    /// <summary>
    /// Check if Bot reach Destination
    /// </summary>
    /// <returns>True if Bot reaches destination</returns>
    public bool IsReached()
    {
        return IsReachedDestination;
    }

    /// <summary>
    /// Find and set the closet brick as destination
    /// </summary>
    public void ChooseBrick()
    {
        // currentStage wasn't set
        if (currentStage == null)
        {
            Invoke(nameof(ChooseBrick), 0.02f);
            return;
        }

        Vector3 destBrick = currentStage.GetClosetBrick(tf.position, this.GetColor());
        SetDestination(destBrick);
    }

    /// <summary>
    /// Find and set the stair for bot to go up
    /// </summary>
    public void ChooseStair()
    {
        // Why we have to choose stair ? ;)
        // Just force bot to go to winpos :Đ
        SetDestination(levelManager.GetWinPos());
    }

    /// <summary>
    /// Connect to LevelManager for communicating with other object
    /// </summary>
    /// <param name="levelManager"></param>
    /// <returns></returns>
    public Bot SetLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        return this;
    }

    /// <summary>
    /// Set new stage as a checkpoint
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public Bot SetStage(Stage stage)
    {
        this.currentStage = stage;
        ChangeState(new CollectState(currentStage.GetRandNumBricks()));
        return this;
    }

    public void ChangeState(IState<Bot> state)
    {
        if(currentState != null) 
            currentState.OnExit(this);

        currentState = state;
        currentState.OnEnter(this);
    }

    void Update()
    {
        if(currentState != null)
        {
            currentState.OnExecute(this);
        }
    }
}
