using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOrchestrator : MonoBehaviour
{
    public float TimeLimit = 60.0f;
    public GameObject ScoreText;
    public GameObject TimeLeftText;
    public GameObject ObjectsGenerator;
    public GameObject Kween;

    private float ElapsedTime = 0f;
    private bool isRunning = false;
    private SpriteRenderer happyKweenSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0f;
        happyKweenSpriteRenderer = Kween.transform.Find("KweenHappy").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        int score = ObjectsGenerator.GetComponent<ObjectsGenerator>().score;
        TimeLeftText.GetComponent<TextMeshProUGUI>().text = $"Time left: {TimeLimit - ElapsedTime:00.0}";
        ScoreText.GetComponent<TextMeshProUGUI>().text = $"Score: {score}";
        happyKweenSpriteRenderer.color = new Color(happyKweenSpriteRenderer.color.r, happyKweenSpriteRenderer.color.g, happyKweenSpriteRenderer.color.b, score/30f);
        ObjectsGenerator.GetComponent<ObjectsGenerator>().isRunning = isRunning;
        if(isRunning)
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= TimeLimit)
            {
                StopGame();
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                ElapsedTime = 0f;
                ObjectsGenerator.GetComponent<ObjectsGenerator>().score = 0;
                isRunning = true;
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StopGame();
            SceneManager.LoadScene("MainMap");
        }
    }

    private void StopGame()
    {
        isRunning = false;
    }
}
