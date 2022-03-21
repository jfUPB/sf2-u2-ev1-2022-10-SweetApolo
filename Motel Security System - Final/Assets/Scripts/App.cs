using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private SerialController SerialController1;
    [SerializeField] private SerialController SerialController2;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Sending A");
            SerialController1.SendSerialMessage("A");
            SerialController2.SendSerialMessage("A");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Sending Z");
            SerialController1.SendSerialMessage("Z");
            SerialController2.SendSerialMessage("Z");
        }

        string msgSC1 = SerialController1.ReadSerialMessage();
        string msgSC2 = SerialController2.ReadSerialMessage();

        if (msgSC1 != null)
        {
            Debug.Log("Message from Controller1: " + msgSC1);
        }
        
        if (msgSC2 != null)
        {
            Debug.Log("Message from Controller2: " + msgSC2);
        }
    }
}
