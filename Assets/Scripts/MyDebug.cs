using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyDebug : MonoBehaviour
{
    private static Text text;

    private void Awake()
    {
        //text = GameObject.Find("Debug").GetComponent<Text>();
        
    }

    public static void Log(string o)
    {
        text.text = o;
    }

}
