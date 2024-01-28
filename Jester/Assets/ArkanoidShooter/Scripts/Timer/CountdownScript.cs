using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class CountdownScript : MonoBehaviour
{
    private float TimeLimit = 60.0f;

    private float ElapsedTime = 0f;
    private bool isRunning = false;

    private void FixedUpdate()
    {
        ElapsedTime += Time.deltaTime;
        if(ElapsedTime >= TimeLimit )
        {
            StopGame();
        }
        this.GetComponent<TextMeshProUGUI>().text = $"Time left: {TimeLimit - ElapsedTime:00.0}";
    }

    private void StopGame()
    {
        isRunning = false;
    }
    void Start()
    {
        ElapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
