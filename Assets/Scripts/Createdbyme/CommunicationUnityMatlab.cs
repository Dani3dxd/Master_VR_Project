using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class CommunicationUnityMatlab : MonoBehaviour
{
    String valueBase = "2";
    String valueShoulder = "7";
    String valueElbow = "5";
    String valueWrist1 = "90";
    String valueWrist2 = "45";
    String valueWrist3 = "0";

    internal Boolean socketReady = false;
    TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;
    String Host = "localhost";
    Int32 Port = 55000;
    
    void Start()
    {
        setupSocket();
    }
    void Update()
    {
        SendData();
    }

    public void setupSocket()
    {
        mySocket = new TcpClient(Host, Port);
    }
    void SendData()
    {
        String information = "A" + valueBase + "B" + valueShoulder + "C" + valueElbow + "D" + valueWrist1 + "E" + valueWrist2 + "F" + valueWrist3 + "G";
        try
        {
            Byte[] sendBytes = Encoding.UTF8.GetBytes(information);
            mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length);
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
        }
    }
}
