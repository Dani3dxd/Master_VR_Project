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
    [SerializeField] Button enableBtn;
    [SerializeField] Button disableBtn;

    [Header("Motores")]
    [SerializeField] public bool enableMotors = false;
    [SerializeField] [Range(0, 180)] private int [] motor = new int[6];
    
    void Start()
    {
        RefreshPorts();
        ActivateConnectionButtons();
        EnableMotors();
    }

    public void SendData(string angleMotorData)
    {
        //first write the serial port later read
        if (isConnected)
        {
            try
            {
                if(enableBtn)
                //{
                // Send value data of each angle on UR robot
                
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

    public void EnableMotors()
    {
        enableMotors = !enableMotors;
        enableBtn.gameObject.SetActive(!enableMotors);
        disableBtn.gameObject.SetActive(enableMotors);
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
