using UnityEngine;

namespace Util
{
    /// <summary>
    /// Add this script to set the max angular velocity of the rigidbody to a different value than default on awake.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public class SetMaxAngularVelocityOnAwake : MonoBehaviour
    {
        [Tooltip("Max angular velocity value to set on awake.")]
        public float MaxAngularVelocity = Mathf.Infinity;

        private void Awake()
        {
            var rb = GetComponent<Rigidbody>();
            rb.maxAngularVelocity = MaxAngularVelocity;
        }
    }
}
