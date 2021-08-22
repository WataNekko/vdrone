using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    public class WeighingScale : MonoBehaviour
    {
        [SerializeField]
        [ReadOnlyInspector]
        [Tooltip("Calculated mass (kg)")]
        private float _weight;
        [SerializeField]
        [Tooltip("Text UI on which to display the weight.")]
        private Text _displayText;


        private Dictionary<Rigidbody, float> _registeredWeights = new Dictionary<Rigidbody, float>();

        private void OnCollisionEnter(Collision collision)
        {
            var rb = collision.rigidbody;
            if (rb != null)
            {
                // Convert world space to local
                Vector3 impulseLocal = transform.InverseTransformDirection(collision.impulse);
                Vector3 gravityLocal = transform.InverseTransformDirection(Physics.gravity);

                // Calculate weight accordingly
                float force = -impulseLocal.y / Time.fixedDeltaTime;
                _registeredWeights[rb] = force / gravityLocal.y;

                UpdateWeight();
            }
        }

        private void OnCollisionStay(Collision collision) => OnCollisionEnter(collision);

        private void OnCollisionExit(Collision collision)
        {
            var rb = collision.rigidbody;
            if (rb != null)
            {
                _registeredWeights.Remove(rb);
                UpdateWeight();
            }
        }

        private void UpdateWeight()
        {
            // Sum the total weight
            float totalWeight = 0f;
            foreach (float weight in _registeredWeights.Values)
            {
                totalWeight += weight;
            }

            _weight = totalWeight;

            // Display text
            if (_displayText != null)
            {
                _displayText.text = totalWeight.ToString();
            }
        }
    }
}
