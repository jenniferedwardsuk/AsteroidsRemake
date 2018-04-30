using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject livesCreationPoint;
    public GameObject lifePrefab;
    public GameObject playerPrefab;
    public GameObject hazard1;
    public GameObject hazard2;
    public GameObject hazard3;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public bool gameover = false;
    public bool respawnPlayer = false;
    public float respawnWait;

    public int PlayerLifeCount;
    public static int Score;
    public GUIText scoreText;
    public GUIText restartText;

    void Start()
    {
        // create life images
        float lifeWidth = livesCreationPoint.transform.localScale.y;
        Vector3 creationPosition = livesCreationPoint.transform.position;
        creationPosition = new Vector3(creationPosition.x + lifeWidth * PlayerLifeCount, creationPosition.y, creationPosition.z); // create from right to left
        for (float i = lifeWidth * PlayerLifeCount; i > 0; i -= lifeWidth)
        {
            Instantiate(lifePrefab, creationPosition, livesCreationPoint.transform.rotation);
            creationPosition = new Vector3(creationPosition.x - lifeWidth, creationPosition.y, creationPosition.z);
        }

        // determine spawn values for asteroids
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = null;
        if (player != null)
            playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            spawnValues = new Vector3(playerController.getBoundary().xMax + 2, 0.0f, playerController.getBoundary().zMax + 2);
        }
        else
        {
            Debug.Log("Could not find player controller");
            spawnValues = new Vector3(27.5f, 0.0f, 21f);
        }

        // start spawning
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if (gameover && Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        if (respawnPlayer)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    public IEnumerator RespawnPlayer()
    {
        respawnPlayer = false;
        while (GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            yield return new WaitForSeconds(respawnWait);
            Instantiate(playerPrefab);
        }
    }

    public void setScoreText()
    {
        scoreText.text = "Score: " + Score;
    }

    public void GameOver()
    {
        Score = 0;
        gameover = true;
        restartText.text = "Game Over!\nPress 'R' to restart";
    }

    IEnumerator SpawnWaves()
    {
        System.Random picker = new System.Random();
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            if (GameObject.FindGameObjectsWithTag("Asteroid").Length < 9)
            {
                hazardCount = Random.Range(1, 4);
                for (int i = 0; i < hazardCount; i++)
                {
                    int spawnside = picker.Next(4);
                    int asteroidtype = picker.Next(3) + 1;
                    Vector3 spawnPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    if (spawnside == 0)
                        spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 0.0f, spawnValues.z); //top of screen
                    else if (spawnside == 1)
                        spawnPosition = new Vector3(-spawnValues.x, 0.0f, Random.Range(-spawnValues.z, spawnValues.z)); //left of screen
                    else if (spawnside == 2)
                        spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 0.0f, -spawnValues.z); //bottom of screen
                    else if (spawnside == 3)
                        spawnPosition = new Vector3(spawnValues.x, 0.0f, Random.Range(-spawnValues.z, spawnValues.z)); //right of screen
                    Quaternion spawnRotation = Quaternion.identity;
                    if (asteroidtype == 1)
                        Instantiate(hazard1, spawnPosition, spawnRotation);
                    if (asteroidtype == 2)
                        Instantiate(hazard2, spawnPosition, spawnRotation);
                    if (asteroidtype == 3)
                        Instantiate(hazard3, spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }
            }
            yield return new WaitForSeconds(waveWait);
        }
    }
}