using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAndDestroyOnCollision : MonoBehaviour
{
    public float Threshold = 0f;
    public GameObject InstantiationObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= Threshold)
        {
            Instantiate(InstantiationObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
