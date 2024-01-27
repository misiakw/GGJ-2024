using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillSpawnerScript : MonoBehaviour
{
    public GameObject pill;
    public double spawnRate = 5;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        SpawnSpell();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            SpawnSpell();
            timer = 0;
        }



    }

    void SpawnSpell()
    {
        List<int> numbers = new List<int>() { -4,5, -1,5, 1,5, 4,5 };
        int randomIndex = Random.Range(0, 4);

        Instantiate(pill, new Vector3(transform.position.x, numbers[randomIndex], 0), transform.rotation);
    }
}