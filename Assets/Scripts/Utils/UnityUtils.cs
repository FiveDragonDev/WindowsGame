using UnityEngine;

public static class UnityUtils
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component =>
        gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    public static T GetOrAddComponent<T>(this Component component) where T : Component =>
        component.GetComponent<T>() ?? component.gameObject.AddComponent<T>();

    public static T GetClosestObjectByType<T>() where T : Component
    {
        float distance = float.PositiveInfinity;
        var enemies = Object.FindObjectsByType<T>(
            FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        T resultEnemy = null;


        foreach (var currentEnemy in enemies)
        {
            float currentDistance = Vector2.Distance(Player.Singleton
                .transform.position, currentEnemy.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                resultEnemy = currentEnemy;
            }
        }

        return resultEnemy;
    }
}
