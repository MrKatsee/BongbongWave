using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    GameObject player;
    public float speed=3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 calculatePos = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            calculatePos += new Vector3(1, 0, 0);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            calculatePos += new Vector3(-1, 0, 0);

        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.RightArrow))
        {
            calculatePos += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.RightArrow))
        {
            calculatePos += new Vector3(0, 0, 1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.GetComponent<Rigidbody>().AddForce(new Vector3(0,20,0));
        }

        calculatePos *= speed;
        player.GetComponent<Rigidbody>().velocity = calculatePos;

    }
}
