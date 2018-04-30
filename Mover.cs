using UnityEngine;

/// <summary>
/// For moving the asteroids.
/// </summary>
public class Mover : MonoBehaviour
{
    public float speed;
    public GameObject playerobject;
    private PlayerController playercontroller;
    private Boundary boundary;

    void Start()
    {
        System.Random randomangle = new System.Random();
        int angle = randomangle.Next(360) + 1;
        Quaternion angledrotation = Quaternion.Euler(0.0f, angle, 0.0f);
        Vector3 movement = angledrotation * new Vector3(transform.position.x, 0.0f, transform.position.z);
        if (GetComponent<Rigidbody>().velocity.x == 0
            && GetComponent<Rigidbody>().velocity.y == 0
            && GetComponent<Rigidbody>().velocity.z == 0)
        {
            GetComponent<Rigidbody>().velocity = movement * (speed * 0.1f);
        }

        GameObject playercontrollerobject = GameObject.FindWithTag("Player");
        if (playercontrollerobject != null)
        {
            playercontroller = playercontrollerobject.GetComponent<PlayerController>();
        }
        if (playercontroller == null)
        {
            Debug.Log("Cannot find PlayerController script");
        }
        else
        {
            boundary = playercontroller.getBoundary();
        }
    }

    void FixedUpdate()
    {
        // if player has been destroyed, destroy asteroid when outside area or spawning
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (boundary == null || 
            GameObject.FindWithTag("Player") == null && 
            (rigidbody.position.x > boundary.xMax || rigidbody.position.x < boundary.xMin || rigidbody.position.z > boundary.zMax || rigidbody.position.z < boundary.zMin))
        {
            Destroy(gameObject);
        }
        else
        {
            // stay in game area
            if (rigidbody.position.x > boundary.xMax + 2)
            {
                rigidbody.position = new Vector3(boundary.xMin, 0.0f, rigidbody.position.z);
            }
            else if (rigidbody.position.x < boundary.xMin - 2)
            {
                rigidbody.position = new Vector3(boundary.xMax, 0.0f, rigidbody.position.z);
            }
            if (rigidbody.position.z > boundary.zMax + 2)
            {
                rigidbody.position = new Vector3(rigidbody.position.x, 0.0f, boundary.zMin);
            }
            else if (rigidbody.position.z < boundary.zMin - 2)
            {
                rigidbody.position = new Vector3(rigidbody.position.x, 0.0f, boundary.zMax);
            }
        }
    }
}
