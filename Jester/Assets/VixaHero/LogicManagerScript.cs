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
    private bool _gameFailedFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = 45;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        clock.text = timer.ToString();

        if (!_gameFailedFlag)
        {
            if (timer <= 0)
            {
                GameFinished();
            }
        }
    }

    public void GameFailed()
    {;
        DeactivateSpells();
        vixaHero.SetActive(false);

        gameFailed.SetActive(true);
    }

    public void GameFinished()
    {
        DeactivateSpells();
        vixaHero.SetActive(false);

        gameOver.SetActive(true);
        CrossSceneStorage.IsVixaComplete = true;
    }

    public void UpdateScore(int score)
    {
        textUI.text = score.ToString();

        if (score <=  0)
        {
            _gameFailedFlag = true;
            GameFailed();
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