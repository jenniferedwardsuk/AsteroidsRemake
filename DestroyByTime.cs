using UnityEngine;

/// <summary>
/// Used to destroy uncollided bullets.
/// </summary>
public class DestroyByTime : MonoBehaviour
{
    public float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}