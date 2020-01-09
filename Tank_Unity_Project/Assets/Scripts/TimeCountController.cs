using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountController : MonoBehaviour
{
    private float startTime;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        text = GetComponent<Text>();
        text.text = "Hello world!";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("Survived for {0} seconds", (int)(Time.time - startTime));
    }
}
