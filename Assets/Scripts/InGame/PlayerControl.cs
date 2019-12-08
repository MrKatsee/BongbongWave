﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    GameObject player;
    public float speed=3;
    public float rotationSpeed=10;
    bool canJump = true;
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 calculatePos = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            calculatePos += -transform.right;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            calculatePos += transform.right;

        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.RightArrow))
        {
            calculatePos += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.RightArrow))
        {
            calculatePos += -transform.forward;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            player.transform.Rotate(new Vector3(0, -Time.deltaTime * rotationSpeed, 0),Space.Self);
        }

        if (Input.GetKey(KeyCode.E))
        {
            player.transform.Rotate(new Vector3(0, Time.deltaTime * rotationSpeed, 0), Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                StartCoroutine(Jump());
            }
        }

        calculatePos *= speed;
        player.GetComponent<Rigidbody>().velocity = calculatePos;

    }

    IEnumerator Jump()
    {
        Debug.Log("Jumping");

        canJump = false;
        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 600, 0));
        yield return new WaitForSeconds(0.5f);
        canJump = true;
        Debug.Log("can Jump");
    }
}
