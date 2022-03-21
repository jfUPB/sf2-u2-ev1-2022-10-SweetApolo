using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Text;
using Microsoft.SqlServer.Server;

public class AppUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SerialControllerBinary SerialController2;

    // For controller2
    [SerializeField] private Dropdown dropdownSerialPortsAvailableController2;
    [SerializeField] private Text buttonOpenController2Text;
    [SerializeField] private Text inputFieldController2Speed;
    [SerializeField] private Text bPMText;
    [SerializeField] private Text tempText;
    [SerializeField] private Text humText;
    [SerializeField] private Image _panel;
    [SerializeField] private GameObject _heart;
    [SerializeField] private Light _light;


    
    private bool isPortOpenController2 = false;
    private bool _appOn = false;
    private float time = 1;
    private float _timeCounter;
    private int _bPM;
    private int _temp;
    private int _hum;
    
    bool _sistole = false;
    
    


    void Start()
    {
        populatePortsForContoller2();
    }

    private void populatePortsForContoller2()
    {
        dropdownSerialPortsAvailableController2.ClearOptions();
        string[] ports = SerialController2.SerialPortsAvailable();
        if (ports != null)
        {
            Dropdown.OptionData portOption;

            foreach (var port in ports)
            {
                portOption = new Dropdown.OptionData();
                portOption.text = port;
                dropdownSerialPortsAvailableController2.options.Add(portOption);
            }
        }
        else
        {
            Debug.Log("Connect Controller 2");
        }

    }
    
    public void openClosePortController2()
    {
        if (isPortOpenController2 == false)
        {
            int portSelection = dropdownSerialPortsAvailableController2.value;
            string port = dropdownSerialPortsAvailableController2.options[portSelection].text;
            SerialController2.portName = port;

            try
            {
                int speed = Int32.Parse(inputFieldController2Speed.text);
                SerialController2.baudRate = speed;
            }
            catch
            {
                Debug.Log("Default speed: " + SerialController2.baudRate.ToString());
            }
            isPortOpenController2 = true;
            buttonOpenController2Text.text = "CLOSE";
            SerialController2.OpenPort();
        }
        else
        {
            isPortOpenController2 = false;
            buttonOpenController2Text.text = "OPEN";
            SerialController2.ClosePort();
        }
    }
    public void AplicationOff()
    {
        if (isPortOpenController2 == true)
        {
            _appOn = false;
            _panel.transform.position = _panel.transform.position + (new Vector3(0,+1250,0));
        }
        else
        {
            Debug.Log("Serial port is closed");
        }
    }
    public void AplicationOn()
    {
        if (isPortOpenController2 == true)
        {
            _appOn = true;
            _panel.transform.position = _panel.transform.position + (new Vector3(0,-1250,0));
        }
        else
        {
            Debug.Log("Serial port is closed");
        }
    }
    public void GiveMeData()
    {
        if (isPortOpenController2 == true)
        {
            byte[] frame = new byte[1] { 0x01};
            SerialController2.SendSerialMessage(frame);
        }
        else
        {
            Debug.Log("Serial port is closed");
        }
    }
    private void beat()
    {
        if(_sistole)
        {
            _heart.transform.localScale = new Vector3(0.05f,0.05f,0.05f);
            _sistole = false;
        }
        else
        {
            _heart.transform.localScale = new Vector3(0.06f,0.06f,0.06f);
            _sistole = true;
        }
    }
    
    private void luz(int hum)
    {
        if (hum <= 0)
        {
            _light.color = Color.Lerp(_light.color, Color.red, Mathf.PingPong(Time.time, 1));    
        }
        else if (hum <= 60)
        {
            _light.color = Color.Lerp(_light.color, Color.cyan, Mathf.PingPong(Time.time, 1));    
        }
        else
        {
            _light.color = Color.Lerp(_light.color, Color.red, Mathf.PingPong(Time.time, 1));
        }
        
        
        
    }


    // Update is called once per frame
    void Update()
    {
        if (_appOn)
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter >= time)
            {
                GiveMeData();
                _timeCounter = 0;
            }
        }

        if (isPortOpenController2 == true)
        {
            byte[] msg;
            msg = SerialController2.ReadSerialMessage();

            if (msg != null)
            {
                
                
                
                Debug.Log(msg[0].ToString("x2") + msg[1].ToString("x2") + msg[2].ToString("x2"));
                _bPM = Convert.ToInt32(msg[0]);
                _temp = Convert.ToInt32(msg[1]);
                _hum = Convert.ToInt32(msg[2]);
                float beats = 1/_bPM;
                if (_timeCounter >= beats)
                {
                    beat();
                }
                luz(_hum);
                
                bPMText.text = _bPM.ToString();
                tempText.text = _temp.ToString();
                humText.text = _hum.ToString();

            }
        }
        
    }
}
