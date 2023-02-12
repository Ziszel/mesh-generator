using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float hMovement = Input.GetAxisRaw("Horizontal") * movementSpeed;
        float vMovement = Input.GetAxisRaw("Vertical") * movementSpeed;
        MoveCamera(hMovement, vMovement);
        RotateCamera();
    }

    private void MoveCamera(float hMov, float vMov)
    {
        transform.Translate(hMov * Time.deltaTime, 0.0f, vMov * Time.deltaTime, Space.Self);
    }

    private void RotateCamera()
    {
        transform.eulerAngles += rotationSpeed * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0.0f);
    }
}
