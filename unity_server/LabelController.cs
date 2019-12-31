using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using SimpleJSON;
using UnityEngine;

public class LabelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.GetComponent<TextMesh>().text = "Goodbye world!";
        JSONNode data = null;
        ServerController serverController = GameObject.Find("Server").GetComponent<ServerController>();
        Mutex mut = ServerController.mut;
        mut.WaitOne();
        data = JSON.Parse(serverController.msgReceive);
        mut.ReleaseMutex();    
        //JSONNode data = GameObject.Find("Server").GetComponent<ServerController>().data;
        if (data != null)
        {
            gameObject.GetComponent<TextMesh>().text = "count = " + data["hand_count"];
        }

        //Debug.Log("fps in Unity = " + 1.0f / Time.deltaTime);
    }
}
