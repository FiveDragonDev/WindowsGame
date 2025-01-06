using UnityEngine;

public class Money : MonoBehaviour
{
    public void Destroy() => GameWorld.MoniesPool.Release(gameObject);
}
