using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOrchestrator : MonoBehaviour
{
    public float TimeLimit = 60.0f;
    private float ElapsedTime = 0f;
    private bool isRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
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

        }
    }

    private void StopGame()
    {
        isRunning = false;
        ElapsedTime = 0f;
    }
}
