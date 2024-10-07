using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoServo : MonoBehaviour
{
    [Header("Puerto Serial COM:")]
    [SerializeField] private SerialPort serialPort;
    [SerializeField] private String puertoCOM = "COM16";
    [SerializeField] private int baudRate = 9600;
    [Header("Nivel de Brillo: ")]
    //[SerializeField][Range(0, 180)] private int servo1 = 0;
    [SerializeField] GameObject motor5;
    

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

    public void sendData()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                String angle5 = MapDegrees((int)motor5.transform.localEulerAngles.y).ToString();
                serialPort.WriteLine(angle5);// it receives the angle for servo1 like a string
                Debug.Log("Angulo Enviado: " + angle5);//prints on console the data send it

            }
            catch (Exception e)
            {
                Debug.LogError("Error al enviar datos al Arduino: " + e.Message);
            }
        }
    }

    /*void Update()
    {
        float value = MapDegrees((int)servo.transform.localEulerAngles.y);
        Debug.Log("Angulo euler gameobject:"+value);
        
    }*/

    float MapDegrees(int degrees)
    {
        // Si el valor está entre 270 y 360, mapeamos entre 0 y 90
        if (degrees >= 270 && degrees < 360)
        {
            return Mathf.Lerp(180, 90, Mathf.InverseLerp(270, 360, degrees));
        }
        // Si el valor está entre 0 y 90, mapeamos entre 90 y 180
        else if (degrees >= 0 && degrees <= 90)
        {
            return Mathf.Lerp(90, 0, Mathf.InverseLerp(0, 90, degrees));
        }
        return 0;
    }
    void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.WriteLine("90");
            serialPort.Close();
            Debug.Log("Puerto serial cerrado.");
        }
    }
}
