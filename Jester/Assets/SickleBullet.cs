using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SickleBullet : MonoBehaviour
{
    public float deactivate_Timer = 10f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0, 0, 180 * Time.deltaTime));
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
