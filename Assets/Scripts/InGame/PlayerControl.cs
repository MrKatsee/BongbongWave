﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    GameObject player;
    public float speed=3;
    public float rotationSpeed=10;
    bool canJump = true;

    Transform spawnPosition;
    Vector3 calculatePos;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = GameObject.Find("SpawnPosition").transform;
        canJump = true;
        player = GameObject.FindGameObjectWithTag("Player");

        calculatePos = new Vector3(0, 0, 0);
    }


    // Update is called once per frame
    void Update()
    {
        calculatePos.x = 0;
        calculatePos.y = 0;
        calculatePos.z = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
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

        calculatePos *= speed;
        player.GetComponent<Rigidbody>().velocity = calculatePos;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                StartCoroutine(Jump());
            }
        }

    }

    IEnumerator Jump()
    {
        Debug.Log("Jumping");

        canJump = false;
        for (int i = 0; i < 20; i++)
        {
            player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 150, 0));
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.8f);
        canJump = true;
        Debug.Log("can Jump");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            MoveToSpawnPosition();
        }
    }

    public void MoveToSpawnPosition()
    {
        player.transform.position = spawnPosition.position;
        player.transform.rotation = spawnPosition.rotation;
        ScoreManager.instance.ResetScore();
    }
}
