using System.Collections.Generic;
using UnityEngine;

public class SpellSpawnerScript : MonoBehaviour
{
    public GameObject Spell;
    public double spawnRate = 5;
    private List<int> _numbers = new List<int>() { -4, 5, -1, 5, 1, 5, 4, 5 };
    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        SpawnSpell();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer < spawnRate)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            SpawnSpell();
            _timer = 0;
        }
    }

    void SpawnSpell()
    {
        int randomIndexOf4 = Random.Range(0, 4);
        int randomIndexOf8 = Random.Range(0, 7);

        var spell = Instantiate(Spell, new Vector3(transform.position.x, _numbers[randomIndexOf4], 0), transform.rotation);
        spell.GetComponent<SpellMoveScript>().sprites[randomIndexOf8].active = true;
    }
}