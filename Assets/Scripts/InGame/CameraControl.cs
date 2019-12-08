using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라를 제어하는 스크립트
/// </summary>
public class CameraControl : MonoBehaviour
{
    public Transform target;
    //오프셋
    public Vector3 offset;

    Vector3 originalPosition;
    Quaternion originalRotation;

    bool isPlaying = false;
    bool doRotate = true;

    Transform spawnPosition;
    Transform spinPosition;

    private void Start()
    {
        spawnPosition = GameObject.Find("SpawnPosition").transform;
        spinPosition = GameObject.Find("SpinAxis").transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        target.gameObject.SetActive(false);

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        isPlaying = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isPlaying)
            {   // 게임 중이였다면 꺼라
                SpawnManager.instance.Hide();
                transform.SetParent(target.parent);
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                GameObject.Find("Player").GetComponent<PlayerControl>().MoveToSpawnPosition();
                isPlaying = false;
                target.gameObject.SetActive(false);
            }
            else
            {   // 게임을 켜라
                ScoreManager.instance.ResetScore();
                SpawnManager.instance.Show();
                transform.position = target.position - offset;
                transform.rotation = target.rotation;
                isPlaying = true;
                target.gameObject.SetActive(true);
                transform.SetParent(target);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            doRotate = !doRotate;
        }

        if (doRotate && !isPlaying)
        {
            transform.RotateAround(spinPosition.position, Vector3.up , 10 * Time.deltaTime);
        }
    }
    
}