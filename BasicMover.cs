using UnityEngine;

/// <summary>
/// For moving bullets.
/// </summary>
public class BasicMover : MonoBehaviour {

    public float speed;

    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * speed;
    }
}
