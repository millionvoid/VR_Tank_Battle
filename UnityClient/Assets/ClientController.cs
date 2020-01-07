using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using Leap;
using Leap.Unity;
using Object = System.Object;

class SampleListener
{
    private ClientController client;
    public void setClient(ClientController _client)
    {
        client = _client;
    }
    public void OnServiceConnect(object sender, ConnectionEventArgs args)
    {
        Debug.Log("Service Connected");
    }

    public void OnConnect(object sender, DeviceEventArgs args)
    {
        Debug.Log("Connected");
    }

    private static string getJsonByObject(System.Object obj)
    {
        //实例化DataContractJsonSerializer对象，需要待序列化的对象类型
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //实例化一个内存流，用于存放序列化后的数据
        MemoryStream stream = new MemoryStream();
        //使用WriteObject序列化对象
        serializer.WriteObject(stream, obj);
        //写入内存流中
        byte[] dataBytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(dataBytes, 0, (int)stream.Length);
        //通过UTF8格式转换为字符串
        return Encoding.UTF8.GetString(dataBytes);
    }

    private static Object getObjectByJson(string jsonString, Object obj)
    {
        //实例化DataContractJsonSerializer对象，需要待序列化的对象类型
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //把Json传入内存流中保存
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        // 使用ReadObject方法反序列化成对象
        return serializer.ReadObject(stream);
    }
    public void OnFrame(object sender, FrameEventArgs args)
    {
        Frame frame = args.frame;
        //Debug.Log(String.Format("Frame id: {0}, timestamp: {1}, hands: {2}", frame.Id, frame.Timestamp, frame.Hands.Count));
        client.send(getJsonByObject(frame));
        //var v = new Vector(1.0f, 1.0f, 1.0f);
        //Debug.Log(getJsonByObject(frame));
        //String s = getJsonByObject(v);
        //Debug.Log(getObjectByJson(s, new Vector()).ToString());


        //Debug.Log(root["id"]);
        //Debug.Log(root.ToString());
        //Debug.Log("Frame Available." + Time.time.ToString());
        //Debug.Log(frame.Serialize.Length.ToString());
    }
}
public class ClientController : MonoBehaviour
{
    // Start is called before the first frame update
    private Controller controller;
    private SampleListener listener;
    private Socket clientSocket;
    void Start()
    {
        controller = new Controller ();
        listener = new SampleListener();
        listener.setClient(this);
        controller.Connect += listener.OnServiceConnect;
        controller.Device += listener.OnConnect;
        controller.FrameReady += listener.OnFrame;
        
        clientSocket = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp);
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777));
    }

    public void send(String s)
    {
        byte[] dataSend = Encoding.ASCII.GetBytes(s);
        clientSocket.Send(dataSend);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
