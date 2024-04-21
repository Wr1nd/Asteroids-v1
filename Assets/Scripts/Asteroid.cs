using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Game gameManager;

    public new Rigidbody2D rigidbody;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public float size = 1f;
    public float minSize = 0.35f;
    public float movementSpeed = 50f;

    private void Awake()
    {
        gameManager = FindObjectOfType<Game>();

        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if(rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        
        transform.localScale = Vector3.one * size;
        rigidbody.mass = size;
        
    }

    public void SetTrajectory(Vector2 direction)
    {
        rigidbody.AddForce(direction * movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if ((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            else
            {
                size = 0;
            }
            
            gameManager.AsteroidDestroyed(this);
            
            Destroy(gameObject);
            
            
        }
    }

    private Asteroid CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;
        
        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.5f;
        
        half.SetTrajectory(Random.insideUnitCircle.normalized);

        return half;
    }

}
