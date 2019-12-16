using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PufferfishWaterState : State
{
    public PufferfishWaterState(Pufferfish self) : base(self)
    {

    }

    public override void Enter(State previousState)
    {
        Self.inWater = true;
        Self.rb.useGravity = false;
        Self.rb.velocity = Self.rb.velocity * 0.25f;
    }
    public override void FixedUpdate()
    {
        Vector3 position = Self.transform.position;
        Vector3 target = new Vector3(position.x, Self.currentSurfaceLevel, position.z);

        Self.rb.AddForce(Self.transform.right * 1f);
        Self.rb.AddTorque(new Vector3(0f, 0f, 1f) * Random.Range(-500f, 500f));

        Self.currentSurfaceLevel = Mathf.Lerp(Self.currentSurfaceLevel, Self.surfaceLevel - Random.Range(0f, 1f), Time.fixedDeltaTime);
    }

    public override void Exit(State nextState)
    {
        Self.inWater = false;
        Self.rb.useGravity = true;
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

        List<GameObject> waterVolumes = new List<GameObject>(GameObject.FindGameObjectsWithTag("WaterVolume"));
        float closestDistance = Mathf.Infinity;
        GameObject closest = null;
        waterVolumes.ForEach(wv =>
        {
            float d = Vector3.Distance(wv.transform.position, Self.transform.position);

            if (d < closestDistance)
            {
                closestDistance = d;
                closest = wv;
            }
        });

        waterVolume = closest.transform;
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
