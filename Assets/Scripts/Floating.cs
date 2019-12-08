using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private Rigidbody rb;
    private bool inWater;
    private float surfaceLevel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inWater)
        {
            Vector3 position = transform.position;
            Vector3 target = new Vector3(position.x, surfaceLevel, position.z);

            rb.AddForce((target - position).normalized * Mathf.Min(1f, Vector2.Distance(target, position)));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterVolume"))
        {
            inWater = true;
            rb.useGravity = false;
            surfaceLevel = other.bounds.max.y;
            rb.velocity = rb.velocity * 0.25f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterVolume"))
        {
            inWater = false;
            rb.useGravity = true;
        }
    }

}
