using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    public class WeighingScale : MonoBehaviour
    {
        public float Weight;

        private Dictionary<Rigidbody, float> registeredWeights = new Dictionary<Rigidbody, float>();

        private void FixedUpdate()
        {
            Debug.Log(transform.InverseTransformDirection(Physics.gravity));
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("enter " + collision.impulse.y);
        }

        private void OnCollisionStay(Collision collision)
        {
            //Weight = collision.impulse.magnitude;
            Debug.Log("stay " + collision.impulse.y);
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log("exit " + collision.impulse.y);
        }

        private void UpdateWeight()
        {

        }
    }
}
