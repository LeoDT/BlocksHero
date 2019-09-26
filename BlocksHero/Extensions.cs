using System;

namespace BlocksHero
{
    public static class Extensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static void Fill<T>(this T[] arr, T with)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = with;
            }
        }
    }
}
