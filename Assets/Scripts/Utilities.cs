using UnityEngine;
public static class Utilities
{
    public static void logArr<T>(T[] arr) {
        Debug.LogFormat("[{0}]", string.Join(", ", arr));
    }
}
