using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{    
    public float horizontalSpeed = 1f; // velocidad de rotacion horizontal
    public float verticalSpeed = 1f; // velocidad de rotacion vertical

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90); //Mantiene la rotacion de la camara dentro de un rango de 180 grados

        cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }
}
