using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public float min_Y, max_Y;

    [SerializeField]
    private GameObject player_Bullet;
    [SerializeField]
    private Transform attack_Point;

    public float attack_Timer = 0.05f;
    public float attack_Delay = 0.2f;
    public int max_Bullets = 5;
    public float bullet_Delay_timer = 0f;

    private float current_Attack_Timer;
    private float current_Max_Attack_Timer = 8f;
    private bool canAttack =true;
    private int max_Health = 3;
    private int remainingBullets;
    public GameObject bulletsText;
    public GameObject healthText;

    public HealthManager healthManager;
    void Attack_1()
    {
        attack_Timer += Time.deltaTime;
        bullet_Delay_timer += Time.deltaTime;
        if (remainingBullets == 0)
        {
             if (bullet_Delay_timer > current_Max_Attack_Timer)
                 {
                    canAttack = true;
                    remainingBullets = max_Bullets;
                    bullet_Delay_timer = 0f;
                 }
        }
        //if (attack_Timer > current_Attack_Timer)
        //{
        //    canAttack = true;
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canAttack && remainingBullets > 0)
            {
                //canAttack = false;
                attack_Timer = 0f;
                remainingBullets--;
                Instantiate(player_Bullet, attack_Point.position, Quaternion.identity);
            }
        }
        UpdateBulletsText();
    }
    void UpdateBulletsText()
    {
        bulletsText.GetComponent<TextMeshProUGUI>().text = "Bullets: " + remainingBullets + " / " + max_Bullets;
    }

    void UpdateHealthText()
    {
        healthText.GetComponent<TextMeshProUGUI>().text = "Health: " + max_Health;
    }
    //void Attack()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space) && canAttack)
    //    {
    //        StartCoroutine(ShootWithDelay());
    //    }

    //}
    void Start()
    {
        current_Attack_Timer = attack_Timer;
        remainingBullets = max_Bullets;

        UpdateBulletsText();
        UpdateHealthText();
    }

    void Update()
    {
        MovePlayer();
        Attack_1();
        Endgame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
         {
            max_Health--;
            UpdateHealthText();
        }
    }

    private void Endgame()
    {
        if(max_Health <= 0)
        {

        }
    }

    void MovePlayer()
    {
        if(Input.GetAxisRaw("Vertical")>0f)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;
            
            if(temp.y > max_Y)
            {
                temp.y = max_Y;
            }

            transform.position = temp;
        }
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;
            if( temp.y < min_Y)
            {
                temp.y = min_Y;
            }
            transform.position = temp;
        }
    }

}
