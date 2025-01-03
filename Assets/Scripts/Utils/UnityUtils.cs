using UnityEngine;

public static class UnityUtils
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component =>
        gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    public static T GetOrAddComponent<T>(this Component component) where T : Component =>
        component.GetComponent<T>() ?? component.gameObject.AddComponent<T>();
}
