using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    GameObject myPengsu;
    public float speed=3;
    // Start is called before the first frame update
    void Start()
    {
        myPengsu = GameObject.FindGameObjectWithTag("Player");
    }

    void MakeBigger()
    {
        this.gameObject.transform.localScale *= 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            myPengsu.transform.Translate(-Time.deltaTime * speed, 0, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            myPengsu.transform.Translate(Time.deltaTime * speed, 0, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            myPengsu.transform.Translate(0, Time.deltaTime * speed, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            myPengsu.transform.Translate(0, -Time.deltaTime * speed, 0, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeBigger();
        }

    }

    
}
