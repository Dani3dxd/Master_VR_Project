using System;
using System.Collections.Generic;
using System.IO.Ports;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArduinoController : MonoBehaviour
{
    
    SerialPort arduinoPort;
    [Header("Puerto Serial COM")]
    [SerializeField] private List<String> connectionPorts;
    [SerializeField] private int baudRate = 115200;
    [SerializeField] public bool isConnected = false;

    [Header("Buttons")]
    [SerializeField] TMP_Dropdown dropList;
    [SerializeField] Button connectBtn;
    [SerializeField] Button disconnectBtn;

    [Header("Nivel")]
    [SerializeField] [Range(0, 180)] private int [] motor = new int[6];
    /*[SerializeField][Range(0, 180)] private int motor1 = 0;
    [SerializeField][Range(0, 180)] private int motor2 = 0;
    [SerializeField][Range(0, 180)] private int motor3 = 0;
    [SerializeField][Range(90, 180)] private int motor4 = 180;
    [SerializeField][Range(0, 180)] private int motor5 = 90;
    [SerializeField][Range(0, 360)] private int motor6 = 0;*/

    //[SerializeField] bool sendInf = false;



    void Start()
    {
        RefreshPorts();
        ActivateConnectionButtons();
    }

    public void SendData(string angleMotorData)
    {
        //first write the serial port later read
        if (isConnected)
        {
            try
            {
                //if(sendInf)
                //{
                // Send value data of each angle on UR robot
                //string angleMotorData = string.Join("*", motor) + "\n";      
                arduinoPort.WriteLine(angleMotorData);
                Debug.Log($"Enviando ángulo: {angleMotorData}");
          
                //}
                //sendInf = false;

            }
            catch (Exception e)
            {
                Debug.LogError("Error al enviar datos al Arduino: " + e.Message);
            }
        }
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            try
            {
                string dataReceived = arduinoPort.ReadLine(); //Read a line from serial port after send data
                Debug.Log($"Dato recibido: {dataReceived}");
            }
            catch (TimeoutException)
            {
                // Ignorar el timeout si no hay datos disponibles
            }
        }
        
    }

    public void RefreshPorts()
    {
        connectionPorts = new List<String> { };
        foreach (string port in SerialPort.GetPortNames())
        {
            connectionPorts.Add(port);
        }
        dropList.ClearOptions();
        dropList.AddOptions(connectionPorts);
    }

    public void AttemptConnection()
    {
        arduinoPort = new SerialPort(dropList.captionText.text, baudRate);
        try
        {
            arduinoPort.Open();
            arduinoPort.WriteTimeout = 500;
            arduinoPort.ReadTimeout = 10;

            Debug.Log("Puerto serial abierto correctamente.");
            isConnected = true;
        }
        catch (Exception e)
        {
            Debug.LogError("No se pudo abrir el puerto serial: " + e.Message);
        }
        ActivateConnectionButtons();
    }

    public void Disconnect()
    {
        //arduinoPort.WriteLine("0,0,0,180,90,0");
        isConnected = false;
        ActivateConnectionButtons();
        arduinoPort.Close();
        Debug.Log("Puerto serial cerrado correctamente.");
    }

    private void ActivateConnectionButtons()
    {
        connectBtn.gameObject.SetActive(!isConnected);
        disconnectBtn.gameObject.SetActive(isConnected);
    }

    void OnApplicationQuit()
    {
        if (arduinoPort.IsOpen)
        {
            arduinoPort.Close();
            Debug.Log("Puerto serial cerrado.");
        }
    }
}
