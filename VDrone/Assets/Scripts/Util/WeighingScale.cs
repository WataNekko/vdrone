using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    public class WeighingScale : MonoBehaviour
    {
        [SerializeField]
        [ReadOnlyInspector]
        [Tooltip("Calculated mass (kg) rounded to 4 decimal places.")]
        private float _weight;
        [SerializeField]
        private UnityEvent _weightUpdated;

        private Dictionary<Collider, float> _registeredWeights = new Dictionary<Collider, float>();

        public float Weight => _weight;

        private void OnCollisionEnter(Collision collision)
        {
            // Convert world space to local
            Vector3 impulseLocal = transform.InverseTransformDirection(collision.impulse);
            Vector3 gravityLocal = transform.InverseTransformDirection(Physics.gravity);

            // Calculate weight accordingly
            float force = -impulseLocal.y / Time.fixedDeltaTime;
            _registeredWeights[collision.collider] = force / gravityLocal.y;

            UpdateWeight();
        }

        private void OnCollisionStay(Collision collision) => OnCollisionEnter(collision);

        private void OnCollisionExit(Collision collision)
        {
            _registeredWeights.Remove(collision.collider);
            UpdateWeight();
        }

        private void UpdateWeight()
        {
            // Sum the total weight
            float totalWeight = 0f;
            foreach (float weight in _registeredWeights.Values)
            {
                totalWeight += weight;
            }
            // Round to 4 decimal places
            totalWeight = Mathf.Round(totalWeight * 1e4f) * 1e-4f;

            _weight = totalWeight;

            _weightUpdated.Invoke();
        }
    }
}
