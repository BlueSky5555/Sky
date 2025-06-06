using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ��͹�����ͤ�����ç��ҧ��
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ��������ͧ��ع�Թ�

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // ��ع���ͧ
        playerBody.Rotate(Vector3.up * mouseX); // ��ع����Ф�
    }
}