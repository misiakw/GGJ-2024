using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -16;
    public GameObject[] sprites;
    private static int _failsCounter = 3;
    private LogicManagerScript _logicManager;

    // Start is called before the first frame update
    void Start()
    {
        _logicManager = GameObject.FindGameObjectWithTag("LogicManagerScriptTag").GetComponent<LogicManagerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            if (gameObject.layer != 6)
            {
                _failsCounter--;
                Debug.Log($"SpellMoveScript counter: {_failsCounter}");
                _logicManager.UpdateScore(_failsCounter);
            }

            Destroy(gameObject);
        }       
    }

    //void Destroyer()
    //{

    //    DestroyImmediate(gameObject);
    //}

    //public void SpellDefeated()
    //{
    //    StartCoroutine("Destroyer");
    //    //_destroyerOn = true;
    //}
}