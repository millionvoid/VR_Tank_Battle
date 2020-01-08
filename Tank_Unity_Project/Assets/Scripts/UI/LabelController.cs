using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class LabelController : MonoBehaviour {
    public Text text;
    public LeapMotionListener leapMotionListener;


    // Update is called once per frame
    void Update() {
        //gameObject.GetComponent<TextMesh>().text = "Goodbye world!";
        //JSONNode data = null;
        //Mutex mut = LeapMotionListener.mut;
        //mut.WaitOne();
        //data = JSON.Parse(leapMotionListener.msgReceive);
        //mut.ReleaseMutex();
        ////JSONNode data = GameObject.Find("Server").GetComponent<ServerController>().data;
        //if (data != null) {
        //    text.text = "count = " + data["hand_count"];
        //}

        //Mutex mut = LeapMotionListener.mut;
        //mut.WaitOne();
        JSONNode data = leapMotionListener.data;
        if (data != null) {
            text.text = "id = " + data["id"];
        }
        //mut.ReleaseMutex();

        //Debug.Log("fps in Unity = " + 1.0f / Time.deltaTime);
    }
}
