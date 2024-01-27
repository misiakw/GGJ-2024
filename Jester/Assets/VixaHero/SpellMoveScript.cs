using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -16;
    public GameObject[] pointers;

    // Start is called before the first frame update
    void Start()
    {
        var sr = this.GetComponent<SpriteRenderer>();
        pointers = GameObject.FindGameObjectsWithTag("PointerTag");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

        if (keyToPress != KeyCode.None)
        {
            if (Input.GetKeyDown(keyToPress))
            {
                Debug.Log("noice ! ", this);
                keyToPress = KeyCode.None;
            }
        }

        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }

    //private bool IsInTrigger = false;
    private KeyCode keyToPress = KeyCode.None;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //keyToPress = collision.gameObject switch
        //{
        //    pointers[0]  => KeyCode.Alpha1,
        //    pointers[1] => KeyCode.Alpha2,
        //    _ => KeyCode.None
        //}; ;

        if (collision.gameObject == pointers[0])
        {
            keyToPress = KeyCode.Alpha4;
        }
        else if (collision.gameObject == pointers[1])
        {
            keyToPress = KeyCode.Alpha3;
        }
        else if (collision.gameObject == pointers[2])
        {
            keyToPress = KeyCode.Alpha2;
        }
        else if (collision.gameObject == pointers[3])
        {
            keyToPress = KeyCode.Alpha1;
        }
        else
        {
            Debug.Log("DUPAAA");
        }
        //IsInTrigger = true;   
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        keyToPress = KeyCode.None;
        //IsInTrigger = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}