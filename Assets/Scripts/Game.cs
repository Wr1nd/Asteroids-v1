using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public Player player;
    public Asteroid asteroidPrefab;
    public ParticleSystem explosionEffect;
    public GameObject gameOverUI;

    public int score = 0;
    public GameObject live1;
    public GameObject live2;
    public GameObject live3;
    private int lives = 3;

    public int asteroidsPerWave = 3;
    public int UfosPerWave = 1;
    public float spawnMargin = 1f;

    public Text scoreText;

    public float respawnDelay = 2;
    public float respawnInvulnerability = 2;

    public AudioSource audioSource;
    public AudioClip smallExplosionSound;
    public AudioClip mediumExplosionSound;
    public AudioClip bigExplosionSound;

    
    private void Start()
    {
        scoreText.text = score.ToString();

        gameOverUI.SetActive(false);
        SpawnPlayer();
        AsteroidWave();
        
    }
    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        if (lives == 2)
        {
            live3.SetActive(false);
        }
        if (lives == 1)
        {
            live2.SetActive(false);
        }
        if (lives == 0)
        {
            live1.SetActive(false);
        }
        
    }
    
    public void SpawnPlayer()
    {
        player.transform.position = player.respawnPoint;
        player.invulnerability = respawnInvulnerability;
        player.gameObject.SetActive(true);
    }
    
    public void AsteroidWave()
    {
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
        float screenHeight = Camera.main.orthographicSize * 2;

        for (int i = 0; i < asteroidsPerWave; i++)
        {
            
            int side = Random.Range(0, 4);
            
            Vector2 spawnPoint;
            
            if (side == 0)
            {
                spawnPoint = new Vector2(Random.Range(-screenWidth / 2, screenWidth / 2), screenHeight / 2 + spawnMargin);
            }
            else if (side == 1)
            {
                spawnPoint = new Vector2(Random.Range(-screenWidth / 2, screenWidth / 2), -screenHeight / 2 - spawnMargin);

            }
            else if (side == 2)
            {
                spawnPoint = new Vector2(-screenWidth / 2 - spawnMargin, Random.Range(-screenHeight / 2, screenHeight / 2));
                
            }
            else 
            {
                spawnPoint = new Vector2(screenWidth / 2 +spawnMargin, Random.Range(-screenHeight / 2, screenHeight / 2));
                
            }
            
            Asteroid newAsteroid = Instantiate(asteroidPrefab);
            newAsteroid.transform.position = spawnPoint;
            
            
            
            
            newAsteroid.SetTrajectory(Random.insideUnitCircle);
            
        }
    }
    
    public void AsteroidDestroyed(Asteroid asteroid)
    {

        explosionEffect.transform.position = asteroid.transform.position;
        explosionEffect.Play();
        
        if (asteroid.size < 0.7f)
        {
            score += 100; // small asteroid
            audioSource.PlayOneShot(smallExplosionSound, 1);
        }
        else if (asteroid.size < 1.4f)
        {
            score += 50; // medium asteroid
            audioSource.PlayOneShot(mediumExplosionSound, 1);
        }
        else
        {
            score += 25; // large asteroid
            audioSource.PlayOneShot(bigExplosionSound, 1);
        }

        scoreText.text = score.ToString();


        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        
        bool stageClear = true;
        
        foreach (Asteroid a in asteroids)
        {
            if (a.size > 0)
                stageClear = false;
        }
        
        if(stageClear)
        {
            print("Wave over");
            asteroidsPerWave++;
            AsteroidWave();
        }
    }
    
    public void PlayerDeath(Player player)
    {
        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();

        player.gameObject.SetActive(false);
        
        lives--;

        audioSource.PlayOneShot(bigExplosionSound, 1);


        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke("SpawnPlayer", respawnDelay);
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }


}
