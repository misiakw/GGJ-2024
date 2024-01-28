using System.Collections;
using System.Security.Cryptography;

using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 3f;
    public float yMin = -5f;
    public float yMax = 5f;
    public float respawnInterval = 5f;
    public float spawnXPosition = 11f;
    public float respawnTime = 5.0f;
    private static float time_To_Render = 0;
    public float timeToRenderNext = 10f;
    public bool addEnemy = true;

    private Transform player;
    private Animator animator;
    public GameObject enemyPrefab;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        
    }
    private void Update()
    {
        Vector3 targetPosition = new Vector2(player.position.x, player.position.y);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        time_To_Render += Time.deltaTime;
        AddNextEnemy();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            PlayDestructionAnimation();
        }
    }
    void AddNextEnemy()
    {
        if (timeToRenderNext < time_To_Render && addEnemy)
          {
            addEnemy = false;
            Vector3 spawnPosition = new Vector3(spawnXPosition, Random.Range(yMin, yMax), transform.position.z);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
       }

    private void PlayDestructionAnimation()
    {
        if (animator != null)
        {
            Vector3 spawnPosition = new Vector3(spawnXPosition, Random.Range(yMin, yMax), transform.position.z);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
