using UnityEngine;

public class SpellMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -16;
    public GameObject[] sprites;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}