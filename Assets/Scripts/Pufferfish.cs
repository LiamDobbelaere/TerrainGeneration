using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    public T Self { get; set; }

    public virtual void Enter(State<T> previousState) { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit(State<T> nextState) { }

    public State<T> Bind(T newSelf)
    {
        Self = newSelf;
        return this;
    }
}

public class PufferfishWaterState : State<Pufferfish>
{
    public override void Enter(State<Pufferfish> previousState)
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

    public override void Exit(State<Pufferfish> nextState)
    {
        Self.inWater = false;
        Self.rb.useGravity = true;
    }
}

public class PufferfishLandState : State<Pufferfish>
{
    private Transform waterVolume;
    private float jumpTime;

    public override void Enter(State<Pufferfish> previousState)
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

    public override void Exit(State<Pufferfish> nextState)
    {

    }
}

public class Pufferfish : MonoBehaviour
{
    public bool inWater;
    public Rigidbody rb;
    public float currentSurfaceLevel;
    public float surfaceLevel;

    private State<Pufferfish> currentState = null;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        this.TransitionState(new PufferfishLandState().Bind(this));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }

    void TransitionState(State<Pufferfish> newState)
    {
        if (currentState == newState)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.Exit(newState);
        }

        State<Pufferfish> lastState = currentState;

        currentState = newState;
        currentState.Enter(lastState);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterVolume"))
        {
            surfaceLevel = other.bounds.max.y;
            currentSurfaceLevel = surfaceLevel;

            this.TransitionState(new PufferfishWaterState().Bind(this));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterVolume"))
        {
            this.TransitionState(new PufferfishLandState().Bind(this));
        }
    }

    void OnTriggerStay(Collider other)
    {


    }
}
