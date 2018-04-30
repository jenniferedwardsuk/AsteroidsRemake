using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject newasteroid1;
    public GameObject newasteroid2;
    public GameObject newasteroid3;

    GameObject gameController;
    GameController gameControllerscript;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameControllerscript = null;
        if (gameController != null)
            gameControllerscript = gameController.GetComponent<GameController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid")
        {
            return;
        }
        Instantiate(explosion, transform.position, transform.rotation);

        // create child asteroids if destroyed asteroid was large enough
        if (transform.localScale.x > 1)
        {
            // at half scale
            newasteroid1.transform.localScale = new Vector3(transform.localScale.x * 0.5f, transform.localScale.y * 0.5f, transform.localScale.z * 0.5f);
            newasteroid2.transform.localScale = newasteroid1.transform.localScale;

            // similar but differing y-rotations
            System.Random randomangle = new System.Random();
            int angle = randomangle.Next(180) + 1;
            Quaternion angledrotation1 = Quaternion.Euler(0.0f, angle, 0.0f);
            angle = angle + 30 + randomangle.Next(30);
            Quaternion angledrotation2 = Quaternion.Euler(0.0f, angle, 0.0f);

            // slightly offset position from destroyed asteroid
            Vector3 movement1 = new Vector3(transform.position.x + Random.Range(-0.1f,0.1f) + 0.1f, 0.0f, transform.position.z + Random.Range(-0.1f, 0.1f) + 0.1f);
            Vector3 movement2 = new Vector3(transform.position.x + Random.Range(-0.1f, 0.1f) - 0.1f, 0.0f, transform.position.z + Random.Range(-0.1f, 0.1f) - 0.1f);
            
            GameObject newa1 = Instantiate(newasteroid1, movement1, angledrotation1);
            GameObject newa2 = Instantiate(newasteroid2, movement2, angledrotation2);
            
            // set speed/direction to mimic parent but diverging (+x -z, -x +z)
            newa1.GetComponent<Rigidbody>().velocity = new Vector3(
            gameObject.GetComponent<Rigidbody>().velocity.x + 0.5f*System.Math.Max(gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.z),
            0.0f,
            gameObject.GetComponent<Rigidbody>().velocity.z - 0.5f * System.Math.Max(gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.z));

            newa2.GetComponent<Rigidbody>().velocity = new Vector3(
            gameObject.GetComponent<Rigidbody>().velocity.x - 0.5f * System.Math.Max(gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.z),
                0.0f,
            gameObject.GetComponent<Rigidbody>().velocity.z + 0.5f * System.Math.Max(gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.z));
        }
        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);

            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            GameController gameControllerscript = null;
            if (gameController != null)
                gameControllerscript = gameController.GetComponent<GameController>();
            if (gameControllerscript != null)
            {
                gameControllerscript.PlayerLifeCount -= 1;
                GameObject life = GameObject.FindGameObjectWithTag("Life");
                Destroy(life);
                if (gameControllerscript.PlayerLifeCount <= 0)
                {
                    gameControllerscript.GameOver();
                }
                else
                {
                    gameControllerscript.respawnPlayer = true;
                }
            }
            else
            {
                Debug.Log("Could not find game controller script when player hit asteroid");
                Application.Quit();
            }
        }
        if (other.tag == "Bolt")
        {
            if (gameControllerscript != null)
            {
                GameController.Score = GameController.Score + 10;
                gameControllerscript.setScoreText();
            }
            else
            {
                Debug.Log("Could not find game controller script when shooting asteroid");
            }
        }
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}