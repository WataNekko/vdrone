using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Robotics
{
    /// <summary>
    /// Base class for all controllers.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Controller : MonoBehaviour
    {
        private Dictionary<object, List<Coroutine>> _interrupts = new Dictionary<object, List<Coroutine>>();

        /// <summary>
        /// Attaches a pseudo interrupt service routine to this <see cref="Controller"/>.
        /// Every update, <paramref name="interrupt"/> is passed into <paramref name="predicate"/> to be polled. If <paramref name="predicate"/> returns true, <paramref name="ISR"/> is called.
        /// </summary>
        /// <seealso cref="detachInterrupt"/>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="interrupt">The object of the interrupt to be passed into <paramref name="predicate"/>.</param>
        /// <param name="ISR">The ISR to call when the interrupt occurs. This function must take no parameters and return nothing.</param>
        /// <param name="predicate">Defines when the interrupt should be triggered.</param>
        protected void attachInterrupt<T>(T interrupt, Action ISR, Func<T, bool> predicate)
        {
            if (ISR is null)
            {
                throw new ArgumentNullException(nameof(ISR), $"{nameof(ISR)} must not be null.");
            }
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate), $"{nameof(predicate)} must not be null.");
            }

            var routine = StartCoroutine(InterruptPolling());

            // Stores the coroutine in a list corresponding to the interrupt object for later detachment.
            if (!_interrupts.ContainsKey(interrupt))
            {
                _interrupts.Add(interrupt, new List<Coroutine>());
            }
            _interrupts[interrupt].Add(routine);

            // Interrupt polling definition
            IEnumerator InterruptPolling()
            {
                for (; ; )
                {
                    if (predicate(interrupt))
                    {
                        ISR();
                    }
                    yield return null;
                }
            }
        }

        /// <summary>
        /// Turns off the given interrupt. This detaches all ISRs attached to the <paramref name="interrupt"/> object.
        /// </summary>
        /// <seealso cref="attachInterrupt"/>
        /// <param name="interrupt">The object of the interrupt to disable.</param>
        protected void detachInterrupt(object interrupt)
        {
            if (_interrupts.ContainsKey(interrupt))
            {
                // Stops all coroutines corresponding to the interrupt object.
                foreach (Coroutine routine in _interrupts[interrupt])
                {
                    StopCoroutine(routine);
                }
                _interrupts.Remove(interrupt);
            }
        }

        protected static class InterruptPredicate
        {
            /// <summary>
            /// Gets a function that returns true when the value extracted from the input object changes, false otherwise.
            /// </summary>
            /// <typeparam name="T">Type of the input object.</typeparam>
            /// <typeparam name="TValue">Type of the value extracted from the input object.</typeparam>
            /// <param name="getValue">A function that extracts a value from an input object.</param>
            /// <returns>A function that returns true when the value extracted from the input object changes, false otherwise.</returns>
            public static Func<T, bool> Change<T, TValue>(Func<T, TValue> getValue)
            {
                return PreviousValueComparer(getValue, (prev, curr) => !curr.Equals(prev));
            }

            /// <summary>
            /// Gets a function that returns true when the current value extracted from the input object follows the previous value, false otherwise.
            /// </summary>
            /// <typeparam name="T">Type of the input object.</typeparam>
            /// <typeparam name="TValue">Type of the value extracted from the input object.</typeparam>
            /// <param name="getValue">A function that extracts a value from an input object.</param>
            /// <returns>A function that returns true when the current value extracted from the input object follows the previous value, false otherwise.</returns>
            public static Func<T, bool> Rising<T, TValue>(Func<T, TValue> getValue) where TValue : IComparable<TValue>
            {
                return PreviousValueComparer(getValue, (prev, curr) => curr.CompareTo(prev) > 0);
            }

            /// <summary>
            /// Gets a function that returns true when the current value extracted from the input object precedes the previous value, false otherwise.
            /// </summary>
            /// <typeparam name="T">Type of the input object.</typeparam>
            /// <typeparam name="TValue">Type of the value extracted from the input object.</typeparam>
            /// <param name="getValue">A function that extracts a value from an input object.</param>
            /// <returns>A function that returns true when the current value extracted from the input object precedes the previous value, false otherwise.</returns>
            public static Func<T, bool> Falling<T, TValue>(Func<T, TValue> getValue) where TValue : IComparable<TValue>
            {
                return PreviousValueComparer(getValue, (prev, curr) => curr.CompareTo(prev) < 0);
            }

            private static Func<T, bool> PreviousValueComparer<T, TValue>(Func<T, TValue> getValue, Func<TValue, TValue, bool> comparer)
            {
                bool init = false;
                TValue prev = default;

                return obj =>
                {
                    TValue curr = getValue(obj);
                    bool result = init ? comparer(prev, curr) : !(init = true);
                    prev = curr;

                    return result;
                };
            }
        }

        /// <summary>
        /// Gets the component in the child object based on the given children indices.
        /// </summary>
        /// <param name="childrenIndices">A list of indices to get to the child object.</param>
        /// <returns>The component in the child object based on the given children indices.</returns>
        protected T GetComponent<T>(params int[] childrenIndices)
        {
            Transform tf = transform;
            foreach (int index in childrenIndices)
            {
                tf = tf.GetChild(index);
            }
            return tf.GetComponent<T>();
        }
    }
}
