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
        string handKey = isLeft ? "leftmost_hand" : "rightmost_hand";
        if (data == null || !data.HasKey(handKey)) {
            return;
        }
        var palmPos = data[handKey]["palm_position"].AsArray;
        //Debug.Log("palm pos = " + palmPos[0] + ", " + palmPos[1] + ", " + palmPos[2]);
        float x = palmPos[0].AsFloat / 1000;
        float y = palmPos[1].AsFloat / 1000 + 2;
        float z = -palmPos[2].AsFloat / 1000;
        palm.transform.position = new Vector3(x, y, z);

        var f = data[handKey]["fingers"].AsArray;
        for(int i = 0; i < 5; ++i) {
            var pos = f[i]["bones"][3][1].AsArray;
            float px = pos[0].AsFloat / 1000;
            float py = pos[1].AsFloat / 1000 + 2;
            float pz = -pos[2].AsFloat / 1000;
            fingers[i].transform.position = new Vector3(px, py, pz);
        }
    }


}
