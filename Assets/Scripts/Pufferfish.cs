using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PufferfishWaterState : State
{
    public PufferfishWaterState(Pufferfish self) : base(self)
    {

    }

    public override void FixedUpdate()
    {
        Self.rb.AddForce(Self.transform.right * 1f);
        Self.rb.AddTorque(new Vector3(0f, 0f, 1f) * Random.Range(-500f, 500f));
    }
}

public class PufferfishLandState : State
{
    private Transform waterVolume;
    private float jumpTime;

    public PufferfishLandState(Pufferfish self) : base(self)
    {

    }

    public override void Enter(State previousState)
    {
        jumpTime = 0f;

        waterVolume = Self.NearestGameObjectWithTag("WaterVolume").transform;
    }

    public override void FixedUpdate()
    {
        if (waterVolume != null)
        {
            Self.rb.AddForce((waterVolume.position - Self.transform.position).normalized * 10f);

            Vector3 targetDelta = waterVolume.position - Self.transform.position;
            float angleDiff = Vector3.Angle(Self.transform.right, targetDelta);
            Vector3 cross = Vector3.Cross(Self.transform.right, targetDelta);
            Self.rb.AddTorque(cross * angleDiff * 10f);

            jumpTime += Time.fixedDeltaTime;

            if (jumpTime > 1f)
            {
                Self.rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);
                jumpTime = 0f;
            }
        }
    }

    public override void Exit(State nextState)
    {

    }
}

public class Pufferfish : BaseEntity
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        StateMachine.AddState(new PufferfishLandState(this));
        StateMachine.AddState(new PufferfishWaterState(this));
        StateMachine.ChangeState(typeof(PufferfishLandState));
    }
}
