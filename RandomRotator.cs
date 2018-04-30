using UnityEngine;

/// <summary>
/// Used to rotate the asteroids.
/// </summary>
public class RandomRotator : MonoBehaviour
{
    public float tumble;

    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
    }
}
