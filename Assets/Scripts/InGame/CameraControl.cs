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
    //줌
    private static float currentZoom = 10;
    public float pitch = 1;
    public float zoomSpeed = 0.4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    //회전
    public static float yawSpeed = 10f;
    private float currentYaw = 0;

    Vector3 originalPosition;
    Quaternion originalRotation;

    bool isPlaying = false;

    private void Start()
    {
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
            {
                transform.SetParent(target.parent);
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                isPlaying = false;
                target.gameObject.SetActive(false);
            }
            else
            {
                transform.position = target.position - offset;
                transform.rotation = target.rotation;
                isPlaying = true;
                target.gameObject.SetActive(true);
                transform.SetParent(target);
            }
        }
    }

    // 카메라 제어할 명령어들은 LateUpdate에
    void LateUpdate()
    {
        //offset+Zoom을 적용한 카메라
        //transform.position = target.position - offset * currentZoom;
        //Player를 향해서 카메라 방향 조절 
        //transform.LookAt(target.position + Vector3.up * pitch);
        //카메라를 플레이어의 position을 기준으로 회전시킨다
        //transform.RotateAround(target.position, Vector3.up, currentYaw);
    }

    public void OnZoomInClicked()
    {
        currentZoom -= zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }


    public void OnZoomOutClicked()
    {
        currentZoom += zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    public void OnYawLClicked()
    {
        currentYaw -= yawSpeed;
    }

    public void OnYawRClicked()
    {
        currentYaw += yawSpeed;
    }
}