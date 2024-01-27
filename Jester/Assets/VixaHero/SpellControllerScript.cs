using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellControllerScript : MonoBehaviour
{
    public int WhoAmI = 0;
    private VivaInput input;
    private bool IsInTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        input.Enable();
        IsInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsInTrigger = false;
    }

    private void Awake()
    {
        input = new VivaInput();
    }

    private void OnEnable()
    {
        input.Viixa.Key1.performed += ((c) => OnInputPress(1));
        input.Viixa.Key2.performed += ((c) => OnInputPress(2));
        input.Viixa.Key3.performed += ((c) => OnInputPress(3));
        input.Viixa.Key4.performed += ((c) => OnInputPress(4));
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void OnInputPress(int inp)
    {
        if (IsInTrigger)
        {
            Debug.Log($"woop: {inp}");
            input.Disable();
        }
    }
}