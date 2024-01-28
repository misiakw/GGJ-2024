using UnityEngine;
using UnityEngine.UI;

public class LogicManagerScript : MonoBehaviour
{
    public Text textUI;
    public GameObject gameOver;
    public GameObject jester;
    public GameObject priest;
    public GameObject spellSpawner;
    public GameObject[] pointers;
    public Text clock;
    private float timer = 45;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        clock.text = timer.ToString();
    }

    public void GameOver()
    {
        jester.SetActive(false);
        priest.SetActive(false);
        spellSpawner.GetComponent<SpellSpawnerScript>().Stop();
        DeactivatePointers();
        DeactivateSpells();

        gameOver.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        Debug.Log($"LogicManagerScript counter: {score}");
        textUI.text = score.ToString();

        if (score ==  0)
        {
            GameOver();
        }
    }

    private void DeactivatePointers()
    {
        foreach (GameObject p in pointers)
        {
            p.SetActive(false);
        }
    }

    private void DeactivateSpells()
    {
        var spells = GameObject.FindGameObjectsWithTag("SpellTag");

        foreach (GameObject spell in spells)
        {
            spell.SetActive(false);
        }
    }
}