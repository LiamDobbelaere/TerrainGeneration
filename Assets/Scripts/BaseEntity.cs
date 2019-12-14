using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity<T> : MonoBehaviour where T: MonoBehaviour
{
    public StateMachine<T> StateMachine { get; }

    public BaseEntity()
    {
        this.StateMachine = new StateMachine<T>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        this.StateMachine.Update();
    }

    public virtual void FixedUpdate()
    {
        this.StateMachine.FixedUpdate();
    }
}
