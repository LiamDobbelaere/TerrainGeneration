using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; private set; } = null;

    private Dictionary<System.Type, State> availableStates;

    public StateMachine()
    {
        this.availableStates = new Dictionary<System.Type, State>();
    }

    public void AddState(State state)
    {
        this.availableStates[state.GetType()] = state;
    }

    public void ChangeState(System.Type stateType)
    {
        State previousState = this.CurrentState;
        State nextState = this.availableStates[stateType];

        if (this.CurrentState != null)
        {
            this.CurrentState.Exit(nextState);
        }

        this.CurrentState = this.availableStates[stateType];
        this.CurrentState.Enter(previousState);
    }

    public void Update()
    {
        if (this.CurrentState != null)
        {
            this.CurrentState.Update();
        }
    }

    public void FixedUpdate()
    {
        if (this.CurrentState != null)
        {
            this.CurrentState.FixedUpdate();
        }
    }
}
public abstract class State
{
    public BaseEntity Self { get; set; }

    public State(BaseEntity self)
    {
        Self = self;
    }

    public virtual void Enter(State previousState) { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit(State nextState) { }
}