using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.KinectGame.Enums;

public class ObjectsGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public  GameObject TargetPrefab;
    private GameObject[] Targets = new GameObject[4];



    void Start()
    {
        Targets = new GameObject[4];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i!= Targets.Length; i++)
        {
            if (Targets[i] == null || Targets[i].IsDestroyed())
            {
                LimbType lt = (LimbType)(i + 1);
                Targets[i] = Instantiate(TargetPrefab);
                Targets[i].name = "Target" + lt;
                Targets[i].GetComponent<TargetController>().TargetLimbType = lt;
            }
        }
        
    }
}
