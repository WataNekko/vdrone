namespace Util
{
    public static class MathUtil
    {

        /// <summary>
        /// Maps value from range [fromLow, fromHigh] to range [toLow, toHigh] unclamped.
        /// </summary>
        public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        /// <summary>
        /// <inheritdoc cref="Map(float, float, float, float, float)"/>
        /// </summary>
        public static int Map(this int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        /// <summary>
        /// Maps value from range [fromLow, fromHigh] to range [toLow, toHigh] clamped.
        /// </summary>
        public static float MapClamped(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            var t = Map(value, fromLow, fromHigh, toLow, toHigh);
            bool outOrder = toLow <= toHigh;
            return (t <= toLow) == outOrder
                ? toLow
                : (t >= toHigh) == outOrder
                ? toHigh
                : t;
        }

        /// <summary>
        /// <inheritdoc cref="MapClamped(float, float, float, float, float)"/>
        /// </summary>
        public static int MapClamped(this int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            var t = Map(value, fromLow, fromHigh, toLow, toHigh);
            bool outOrder = toLow <= toHigh;
            return (t <= toLow) == outOrder
                ? toLow
                : (t >= toHigh) == outOrder
                ? toHigh
                : t;
        }
    }
}
