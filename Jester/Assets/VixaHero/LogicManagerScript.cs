using UnityEngine;
using UnityEngine.UI;

public class LogicManagerScript : MonoBehaviour
{
    public Text textUI;
    public GameObject gameOver;
    public GameObject gameFailed;
    public Text clock;
    private float timer = 45;
    public GameObject vixaHero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        clock.text = timer.ToString();

        if (timer <= 0)
        {
            GameFinished();
        }
    }

    public void GameFailed()
    {
        //jester.SetActive(false);
        //priest.SetActive(false);
        //spellSpawner.GetComponent<SpellSpawnerScript>().Stop();
        //DeactivatePointers();
        DeactivateSpells();
        vixaHero.SetActive(false);

        gameFailed.SetActive(true);
    }

    public void GameFinished()
    {
        DeactivateSpells();
        vixaHero.SetActive(false);

        gameOver.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        //Debug.Log($"LogicManagerScript counter: {score}");
        textUI.text = score.ToString();

        if (score <=  0)
        {

            GameFailed();
        }
    }

    //private void DeactivatePointers()
    //{
    //    foreach (GameObject p in pointers)
    //    {
    //        p.SetActive(false);
    //    }
    //}

    private void DeactivateSpells()
    {
        var spells = GameObject.FindGameObjectsWithTag("SpellTag");

        foreach (GameObject spell in spells)
        {
            spell.SetActive(false);
        }
    }
}