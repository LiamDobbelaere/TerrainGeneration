using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    internal bool inWater;
    internal Rigidbody rb;
    internal float currentSurfaceLevel;
    internal float surfaceLevel;

    public StateMachine StateMachine { get; }

    public BaseEntity()
    {
        this.StateMachine = new StateMachine();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterVolume"))
        {
            surfaceLevel = other.bounds.max.y;
            currentSurfaceLevel = surfaceLevel;

            this.StateMachine.ChangeState(typeof(PufferfishWaterState));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterVolume"))
        {
            this.StateMachine.ChangeState(typeof(PufferfishLandState));
        }
    }
}
