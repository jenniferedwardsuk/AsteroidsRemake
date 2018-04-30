using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotationspeed;
    public float tilt;
    public Boundary boundary;
    public float fireRate;
    public GameObject shot;
    public Transform shotSpawn;

    private float nextFire;
    private float Xspeed;
    private float Zspeed;
    private float MaxSpeed = 30;
    private float MinSpeed = 0;
    private float Acceleration = 5;
    private float Deceleration = 20;
    private float currrotationdegrees = 0;
    private bool forwardstoggle = true;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GetComponent<AudioSource>().Play();
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }

        // adjust speed
        if ((Input.GetKey("up") || Input.GetKey("w")) && forwardstoggle) // accelerating
        { 
            if (speed < MaxSpeed)
                speed = speed + Acceleration * Time.deltaTime;
        }
        else if ((Input.GetKey("up") || Input.GetKey("w")) && !forwardstoggle) // slowing reverse
        { 
            speed = speed - Deceleration * Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
                forwardstoggle = true;
            }
        }
        else if ((Input.GetKey("down") || Input.GetKey("s")) && !forwardstoggle) // reversing
        { 
            if (speed < MaxSpeed)
                speed = speed + Acceleration * Time.deltaTime;
        }
        else if ((Input.GetKey("down") || Input.GetKey("s")) && forwardstoggle) // braking
        { 
            speed = speed - Deceleration * Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
                forwardstoggle = false;
            }
        }
        else if (speed > MinSpeed) //no input -> deceleration
        { 
            speed = speed - Deceleration * Time.deltaTime;
        }


        if (speed < 0)
            speed = 0;

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        // move ship (drift)
        Vector3 movementd = new Vector3(0.0f, 0.0f, 0.0f);
        if (speed > 0)
        {
            if (forwardstoggle)
                movementd = rigidbody.rotation * Vector3.forward;
            else
                movementd = rigidbody.rotation * Vector3.back;
        }
        rigidbody.position = new Vector3(rigidbody.position.x + movementd.x * speed * 0.01f, 0.0f, rigidbody.position.z + movementd.z * speed * 0.01f);
    }

    public Boundary getBoundary()
    {
        return boundary;
    }

    void FixedUpdate()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        
        // stay in game area
        if (rigidbody.position.x > boundary.xMax)
        {
            rigidbody.position = new Vector3 ( boundary.xMin, 0.0f, rigidbody.position.z );
        }
        else if (rigidbody.position.x < boundary.xMin)
        {
            rigidbody.position = new Vector3(boundary.xMax, 0.0f, rigidbody.position.z);
        }
        if (rigidbody.position.z > boundary.zMax)
        {
            rigidbody.position = new Vector3(rigidbody.position.x, 0.0f, boundary.zMin);
        }
        else if (rigidbody.position.z < boundary.zMin)
        {
            rigidbody.position = new Vector3(rigidbody.position.x, 0.0f, boundary.zMax);
        }

        // rotate ship
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            currrotationdegrees -= rotationspeed;
            rigidbody.rotation = Quaternion.Euler(0.0f, currrotationdegrees, 0.0f);
            // tilt ship
            rigidbody.rotation = Quaternion.Euler(0.0f, currrotationdegrees, System.Math.Max(rigidbody.velocity.x * -tilt, -30));
        }
        else if ((!Input.GetKey("left") && !Input.GetKey("a")) && rigidbody.rotation.z < 0) // untilt ship if necessary
        {
            rigidbody.rotation = Quaternion.Euler(0.0f, currrotationdegrees, System.Math.Min(rigidbody.rotation.z + 10, -30));
            if (rigidbody.rotation.z > 0)
                rigidbody.rotation = Quaternion.Euler(0.0f, currrotationdegrees, 0.0f);
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            currrotationdegrees += rotationspeed;
            rigidbody.rotation = Quaternion.Euler(0.0f, currrotationdegrees, 0.0f);
            //tilt ship
            rigidbody.rotation = Quaternion.Euler(0.0f, currrotationdegrees, rigidbody.velocity.x * tilt);
        }
        else if ((!Input.GetKey("right") && !Input.GetKey("d")) && rigidbody.rotation.z > 0) // untilt ship if necessary
        {
            rigidbody.rotation = Quaternion.Euler(0.0f, currrotationdegrees, rigidbody.rotation.z - 10);
            if (rigidbody.rotation.z < 0)
                rigidbody.rotation = Quaternion.Euler(0.0f, currrotationdegrees, 0.0f);
        }

        // move ship
        Vector3 movement = new Vector3(0.0f,0.0f,0.0f);
        if ((Input.GetKey("up") || Input.GetKey("w")) && forwardstoggle) // accelerating forwards
            movement = rigidbody.rotation * Vector3.forward;
        else if ((Input.GetKey("up") || Input.GetKey("w")) && !forwardstoggle) // slowing reverse backwards
            movement = rigidbody.rotation * Vector3.back;
        if ((Input.GetKey("down") || Input.GetKey("s")) && !forwardstoggle) // reversing backwards
            movement = rigidbody.rotation * Vector3.back;
        else if ((Input.GetKey("down") || Input.GetKey("s")) && forwardstoggle) // braking forwards
            movement = rigidbody.rotation * Vector3.forward;
        rigidbody.velocity = movement * speed;

    }
}
