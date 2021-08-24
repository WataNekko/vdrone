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
        /// <inheritdoc cref="Map(float, float, float, float, float)"/>
        /// </summary>
        public static float Map(this float value, (float Low, float High) from, (float Low, float High) to)
        {
            return Map(value, from.Low, from.High, to.Low, to.High);
        }

        /// <summary>
        /// <inheritdoc cref="Map(float, float, float, float, float)"/>
        /// </summary>
        public static int Map(this int value, (int Low, int High) from, (int Low, int High) to)
        {
            return Map(value, from.Low, from.High, to.Low, to.High);
        }
    }
}
