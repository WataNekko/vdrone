using UnityEngine;

namespace Util
{
    [System.Serializable]
    public struct IntInRange
    {
        [SerializeField]
        private int _min;
        [SerializeField]
        private int _max;
        [SerializeField]
        private int _value;

        public IntInRange(int value, int min, int max) : this() => (Range, Value) = ((min, max), value);

        public (int min, int max) Range
        {
            get
            {
                return (_min, _max);
            }

            set
            {
                if (value.min > value.max)
                {
                    throw new System.ArgumentException("Min is larger than max value.");
                }

                (_min, _max) = value;
                Value = _value; // Ensure value stays within new range
            }
        }

        public int Value
        {
            get => _value;
            set => _value = Mathf.Clamp(value, _min, _max);
        }
    }
}
