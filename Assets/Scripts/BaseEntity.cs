using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    internal bool inWater;
    internal float waterSurfaceLevel;
    internal Rigidbody rb;

    public StateMachine StateMachine { get; }

    public BaseEntity()
    {
        this.StateMachine = new StateMachine();
    }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        StateMachine.Update();
    }

    public virtual void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterVolume"))
        {
            rb.useGravity = false;
            rb.velocity = rb.velocity * 0.25f;

            waterSurfaceLevel = other.bounds.max.y;

            StateMachine.ChangeState(typeof(PufferfishWaterState));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterVolume"))
        {
            inWater = false;
            rb.useGravity = true;

            StateMachine.ChangeState(typeof(PufferfishLandState));
        }
    }

    public GameObject NearestGameObjectWithTag(string tag)
    {
        List<GameObject> gameObjects= new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
        float closestDistance = Mathf.Infinity;
        GameObject closest = null;
        gameObjects.ForEach(wv =>
        {
            float d = Vector3.Distance(wv.transform.position, transform.position);

            if (d < closestDistance)
            {
                closestDistance = d;
                closest = wv;
            }
        });

        return closest;
    }
}
