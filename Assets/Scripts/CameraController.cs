using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        // Lock the cursor. Makes an application with mouse movement usable.
        // Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // store the input of 'W,A,S,D' / arrow keys to calculate movement in this frame
        float hMovement = Input.GetAxisRaw("Horizontal") * movementSpeed;
        float vMovement = Input.GetAxisRaw("Vertical") * movementSpeed;
        MoveCamera(hMovement, vMovement);
        RotateCamera();
    }

    private void MoveCamera(float hMov, float vMov)
    {
        float heightModifier = 0.0f;
        if (Input.GetKey(KeyCode.Z))
        {
            heightModifier = movementSpeed;
        }

        if (Input.GetKey(KeyCode.X))
        {
            heightModifier = -movementSpeed;
        }
        
        // Move the camera with a simple Translate (called in Update, no physics, use Time.deltaTime)
        transform.Translate(hMov * Time.deltaTime, heightModifier * Time.deltaTime, vMov * Time.deltaTime, Space.Self);
    }

    private void RotateCamera()
    {
        // update the angle of the camera by mouse movement + the set rotation speed
        // Add to the current angle so the camera is updated from its current position
        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles += new Vector3(0.0f, -rotationSpeed * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles += new Vector3(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
        }
        
        //transform.eulerAngles += rotationSpeed * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0.0f);
    }
}
