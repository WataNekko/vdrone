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
        /// Attaches an interrupt service routine to this <see cref="Controller"/>.
        /// Every update, <paramref name="interrupt"/> is passed into <paramref name="condition"/> to be polled. If <paramref name="condition"/> returns <c>true</c>, <paramref name="ISR"/> is called.
        /// </summary>
        /// <seealso cref="detachInterrupt"/>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="interrupt">The object of the interrupt to be passed into <paramref name="condition"/>.</param>
        /// <param name="ISR">The ISR to call when the interrupt occurs. This function must take no parameters and return nothing.</param>
        /// <param name="condition">Defines when the interrupt should be triggered.</param>
        protected void attachInterrupt<T>(T interrupt, Action ISR, Func<T, bool> condition)
        {
            if (ISR is null)
            {
                throw new ArgumentNullException(nameof(ISR), $"{nameof(ISR)} must not be null.");
            }
            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition), $"{nameof(condition)} must not be null.");
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
                    if (condition(interrupt))
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

        protected static class InterruptCondition
        {
            //private static Func<T, bool> PreviousValueComparer<T, TValue>(Func<T, TValue> valueExtracter, Func<TValue, TValue, bool> comparer)
            //{
            //    bool init = false;
            //    TValue prevValue = default;
            //    return (T obj) =>
            //    {
            //        bool result = comparer(prevValue, valueExtracter(obj));

            //        if (!init)
            //        {
            //            return false;
            //        }
            //        else
            //        {
            //            return comparer()
            //        }

            //    };
            //}

            //public static Func<T, bool> Change<T, TValue>(Func<T, TValue> valueExtracter) where T
            //{
            //    return PreviousValueComparer(valueExtracter, (prev, curr) => prev != curr);
            //}
        }
    }
}
