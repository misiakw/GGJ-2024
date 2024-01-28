using System.Collections;

using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2f;
    public float bulletSpawnRate = 1f;
    public float searchRadius = 30f;
    private float current_Attack_Timer;
    public float attack_Timer = 2f;
    private Transform player;
    private Vector3 targetPosition;
    private bool canFire = true;
    public float yMin = -3f;
    public float yMax = 3f;
    public float spawnXPosition = 11f;

    void Start()
    {
        // Set the initial target position
        targetPosition = new Vector3(8f, transform.position.y, transform.position.z);
        // Invoke the method to search for the player periodically
        InvokeRepeating("SearchForPlayer", 0f, 1f / bulletSpawnRate);
    }

    void Update()
    {
        // Move towards target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
        current_Attack_Timer += Time.deltaTime;
        // Throw bullets at player
          if (attack_Timer < current_Attack_Timer)
        {
            canFire = true;

            if (canFire && player != null)
            {
                 Vector3 direction = (player.transform.position - transform.position).normalized;
                FireBullet(direction);
                canFire = false;
                current_Attack_Timer = 0f;
            }
        }
    }
    
    void SearchForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Vector3 spawnPosition = new Vector3(spawnXPosition, Random.Range(yMin, yMax), transform.position.z);
            Vector3 spawnPosition2 = new Vector3(spawnXPosition, Random.Range(yMin, yMax), transform.position.z);

            Instantiate(this.gameObject, spawnPosition, Quaternion.identity);
            Instantiate(this.gameObject, spawnPosition2, Quaternion.identity);

            var targetPosition = new Vector3(8f, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            Destroy(this.gameObject);
        }
    }
        void FireBullet(Vector3 direction)
    {
        // Create enemy bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}