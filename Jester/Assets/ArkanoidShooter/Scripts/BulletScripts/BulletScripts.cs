using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScripts : MonoBehaviour
{
    public float speed = 5f;
    public float deactivate_Timer = 10f;
    void Start()
    {
        Invoke(nameof(DeactivateGameObject), deactivate_Timer);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 temp = transform.position;
        temp.x += speed * Time.deltaTime;
        transform.position = temp;
    }

    void DeactivateGameObject()
    {
        Destroy(gameObject);
    }
}
