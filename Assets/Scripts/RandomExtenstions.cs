using System;

public static class RandomExtensions
{
    /// <summary>
    /// Shuffle the array in place
    /// </summary>
    public static void Shuffle<T>(this Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}