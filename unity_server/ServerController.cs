﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class ServerController : MonoBehaviour
{
     public GameObject label;
     public GameObject cube;
     //public JSONNode data;
     public string msgReceive;
     public static Mutex mut = new Mutex();
     
     private const int BUFFER_SIZE = 8192;
     private byte[] receiveBuffer;


     private void Start()
     {
         ThreadStart serverRef = new ThreadStart(InitializeServer);
         receiveBuffer = new byte[BUFFER_SIZE];
         Thread serverThread = new Thread(serverRef);
         serverThread.Start();
     }

     private void InitializeServer()
     {
          Socket serverSocket = new Socket(
               AddressFamily.InterNetwork,
               SocketType.Stream,
               ProtocolType.Tcp);
          IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
          IPEndPoint Point = new IPEndPoint(iPAddress, 8888);
           
          serverSocket.Bind(Point);
           
          serverSocket.Listen(10);
   
          Debug.Log("begin accepting!");
          serverSocket.BeginAccept(AcceptCallBack, serverSocket);   
     }

     private void AcceptCallBack(IAsyncResult ar)
     {
         Socket serverSocket = ar.AsyncState as Socket;
         Socket clientSocket = serverSocket.EndAccept(ar);
 
         string msgSend = "Hello world!";
         byte[] sendBuffer = Encoding.ASCII.GetBytes(msgSend);
         clientSocket.Send(sendBuffer);
 
         clientSocket.BeginReceive(receiveBuffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallBack, clientSocket);
 
         serverSocket.BeginAccept(AcceptCallBack, serverSocket);
     }
 
     private void ReceiveCallBack(IAsyncResult ar)
     {
         Socket clientSocket = null;
         try
         {
             clientSocket = ar.AsyncState as Socket;
             int count = clientSocket.EndReceive(ar);
             mut.WaitOne();
             //data = JSON.Parse(msgReceive);
             msgReceive = Encoding.ASCII.GetString(receiveBuffer, 0, count);
             mut.ReleaseMutex();
             //Debug.Log("fps = " + data["fps"]);
             //Debug.Log("client message length: " + msgReceive.Length);
             //label.GetComponent<TextMesh>().text = msgReceive;
             /*
             cube.GetComponent<Transform>().Translate(
                 new Vector3(0.0f, 1.0f, 0.0f));
             */
             //Debug.Log("after doing something!");
             clientSocket.BeginReceive(receiveBuffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallBack, clientSocket);
         }
         catch (Exception e)
         {
             Debug.Log("error in ReceiveCallBack: " + e);
         }
     }
}