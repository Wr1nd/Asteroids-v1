using UnityEngine;


public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Animator animator;
    public Game gameManager;
    
    public float thrustSpeed = 200;
    public float maxSpeed = 20;
    public float rotationSpeed = 200;
    public float maxAngularVelocity = 100;

    public float invulnerability;

    public Vector2 respawnPoint;
    
    
    public AudioSource engineAudioSource;
    public AudioSource effectsAudioSource;

    public AudioClip thrustSound;
    public AudioClip fireSound;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        gameManager = FindObjectOfType<Game>();
        
        engineAudioSource.clip = thrustSound;
        engineAudioSource.loop = true;
        engineAudioSource.Play();
        engineAudioSource.volume = 0;
    }
    
    private void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        rb.AddTorque(rotationSpeed * -horizontalInput * Time.deltaTime);

        if (verticalInput > 0)
        {
            //direction vector x speed constant x analog input
            rb.AddForce(transform.up * thrustSpeed * verticalInput * Time.deltaTime);
            
            engineAudioSource.volume = 1;
            animator.Play("thrust");
        }
        else
        {
            engineAudioSource.volume = 0;
            animator.Play("idle");
        }


        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularVelocity, +maxAngularVelocity);
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 bulletPosition = transform.position + transform.up * 0.35f;

            GameObject newBullet = Instantiate(bulletPrefab, bulletPosition, transform.rotation);

            effectsAudioSource.PlayOneShot(fireSound, 1);
        }

        invulnerability -= Time.deltaTime;

    }
    
    private void OnEnable()
    {
        engineAudioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (invulnerability <= 0 && collision.gameObject.CompareTag("Asteroid"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;


            gameManager.PlayerDeath(this);

        }
    }

}
