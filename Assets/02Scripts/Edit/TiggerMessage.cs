using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiggerMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            MessageManager.Instance.TriggerGuidance(GetComponent<EditTrigger>().TriggerID);
            Debug.Log("触发教程" + GetComponent<EditTrigger>().TriggerID);
        }
    }
}
