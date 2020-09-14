using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class HandControl : MonoBehaviour {
    public GameObject palm;
    public GameObject[] fingers;
    public LeapMotionListener leapMotionListener;
    public bool isLeft;

    private void Update() {
        JSONNode data = leapMotionListener.data;
        if (data == null)
            return;
        string handKey = isLeft ? "is_left" : "is_right";
        JSONNode hand = null;
        string[] keys = {"leftmost_hand", "rightmost_hand"};
        foreach (string key in keys)
        {
            if (data.HasKey(key) && data[key][handKey].AsBool)
            {
                hand = data[key];
            }
        }

        if (hand == null)
        {
            setVisible(false);
            return;
        }
        setVisible(true);
        
        var palmPos = hand["palm_position"].AsArray;
        //Debug.Log("palm pos = " + palmPos[0] + ", " + palmPos[1] + ", " + palmPos[2]);
        float x = palmPos[0].AsFloat / 1000.0f;
        float y = palmPos[1].AsFloat / 1000.0f;
        float z = -palmPos[2].AsFloat / 1000.0f;
        //Debug.Log(string.Format("x = {0}, y = {1}, z = {2}", x, y, z));
        palm.transform.localPosition = new Vector3(x, y, z);

        var f = hand["fingers"].AsArray;
        for(int i = 0; i < 5; ++i) {
            var pos = f[i]["bones"][3][1].AsArray;
            float px = pos[0].AsFloat / 1000;
            float py = pos[1].AsFloat / 1000;
            float pz = -pos[2].AsFloat / 1000;
            fingers[i].transform.localPosition = new Vector3(px, py, pz);
        }
    }

    private void setVisible(bool v)
    {
        palm.SetActive(v);
        foreach (GameObject finger in fingers)
        {
            finger.SetActive(v);
        }
    }


}
