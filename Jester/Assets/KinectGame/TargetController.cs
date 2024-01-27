using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.KinectGame.Enums;

public class TargetController : MonoBehaviour
{
    public LimbType TargetLimbType;
    public float timeToScore = 5f;
    public GameObject ProgressBar;
    
    private Guid triggerGuid;
    private float timeFromTrigger = 0f;



    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(UnityEngine.Random.Range(-2f, 3), UnityEngine.Random.Range(-2f, 3), 0);
        this.transform.localScale = Vector3.one * 0.5f;
        Color color = LimbTypeToColor(TargetLimbType);
        if (this.GetComponent<MeshRenderer>() != null)
        {
            this.GetComponent<MeshRenderer>().material.color = color;
        }
        else if(this.GetComponent<SpriteRenderer>() != null)
        {
            this.GetComponent<SpriteRenderer>().color = color;
            this.GetComponentInChildren<TextMeshPro>().text = LimbTypeToString(TargetLimbType);
            ProgressBar.GetComponent<Image>().color = color;
        }
    }

    string LimbTypeToString(LimbType lt)
    {
        switch (lt)
        {
            case LimbType.LeftHand:
                return "LH";
            case LimbType.RightHand:
                return "RH";
            case LimbType.LeftFoot:
                return "LF";
            case LimbType.RightFoot:
                return "RF";
            default:
                return "U";
        }
    }

    Color LimbTypeToColor(LimbType lt)
    {
        switch (lt)
        {
            case LimbType.LeftHand:
                return Color.red;
            case LimbType.RightHand:
                return Color.blue;
            case LimbType.LeftFoot:
                return Color.green;
            case LimbType.RightFoot:
                return Color.yellow;
            default:
                return Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerGuid != Guid.Empty)
        {
            timeFromTrigger += Time.deltaTime;
        }
        ProgressBar.GetComponent<Image>().fillAmount = timeFromTrigger / timeToScore;
    }

    IEnumerator Collect(Guid guid)
    {
        triggerGuid = guid;
        yield return new WaitForSeconds(timeToScore);
        if (triggerGuid == guid)
        {
            Debug.Log("Zlapane!");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Limb limb = other.gameObject.GetComponent<Limb>();
        if (limb != null && limb.LimbType == TargetLimbType)
        {
            StartCoroutine(Collect(Guid.NewGuid()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Limb limb = other.gameObject.GetComponent<Limb>();
        if (limb != null && limb.LimbType == TargetLimbType)
        {
            triggerGuid = Guid.Empty;
            timeFromTrigger = 0f;
        }
    }
}
