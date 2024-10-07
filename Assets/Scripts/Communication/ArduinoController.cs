using System;
using System.IO.Ports;
using Unity.VisualScripting;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    [Header("Puerto Serial COM:")]
    [SerializeField] private SerialPort serialPort;
    [SerializeField] private String puertoCOM = "COM15";
    [SerializeField] private int baudRate = 9600;
    [Header("Nivel de Brillo: ")]
    [SerializeField][Range(0, 255)] private int brightness1 = 0;
    [SerializeField][Range(0, 255)] private int brightness2 = 0;

    void Start()
    {
        // Inicializa el puerto serial
        serialPort = new SerialPort(puertoCOM, baudRate);
        serialPort.ReadTimeout = 500;
        serialPort.WriteTimeout = 500;
        try
        {
            serialPort.Open();
            Debug.Log("Puerto serial abierto correctamente.");
        }
        catch (Exception e)
        {
            Debug.LogError("No se pudo abrir el puerto serial: " + e.Message);
        }
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                // Enviar valores de brillo al Arduino
                string brightnessData = brightness1.ToString() + "," + brightness2.ToString();
                serialPort.WriteLine(brightnessData);
                Debug.Log("Enviando brillo: " + brightnessData);

            }
            catch (Exception e)
            {
                Debug.LogError("Error al enviar datos al Arduino: " + e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Puerto serial cerrado.");
        }
    }
}
