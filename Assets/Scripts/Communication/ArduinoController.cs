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
    [SerializeField] bool isConnected = false;

    [Header("Buttons")]
    [SerializeField] TMP_Dropdown dropList;
    [SerializeField] Button connectBtn;
    [SerializeField] Button disconnectBtn;

    [Header("Nivel")]
    [SerializeField][Range(0, 255)] private int brightness1 = 0;
    [SerializeField][Range(0, 255)] private int brightness2 = 0;
    

    void Start()
    {
        RefreshPorts();
    }

    void Update()
    {

        if (isConnected)
        {
            try
            {
                // Enviar valores de brillo al Arduino
                string brightnessData = brightness1.ToString() + "," + brightness2.ToString();
                arduinoPort.WriteLine(brightnessData);
                Debug.Log("Enviando brillo: " + brightnessData);

            }
            catch (Exception e)
            {
                Debug.LogError("Error al enviar datos al Arduino: " + e.Message);
            }
        }
        connectBtn.gameObject.SetActive(!isConnected);
        disconnectBtn.gameObject.SetActive(isConnected);
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

            Debug.Log("Puerto serial abierto correctamente.");
            isConnected = true;
        }
        catch (Exception e)
        {
            Debug.LogError("No se pudo abrir el puerto serial: " + e.Message);
        }
    }

    public void Disconnect()
    {
        isConnected = false;
        arduinoPort.Close();
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
