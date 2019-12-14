using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T: MonoBehaviour
{
    public State<T> CurrentState { get; private set; } = null;

    private Dictionary<System.Type, State<T>> availableStates;

    public StateMachine()
    {
        this.availableStates = new Dictionary<System.Type, State<T>>();
    }

    public void AddState(State<T> state)
    {
        this.availableStates[state.GetType()] = state;
    }

    public void ChangeState(System.Type stateType)
    {
        State<T> previousState = this.CurrentState;
        State<T> nextState = this.availableStates[stateType];

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
public abstract class State<T> where T : MonoBehaviour
{
    public T Self { get; set; }

    public State(T self)
    {
        Self = self;
    }

    public virtual void Enter(State<T> previousState) { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit(State<T> nextState) { }
}