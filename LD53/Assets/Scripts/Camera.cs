using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 origOffset = new Vector3(0, 10.0f,-40f);

    float xSensitivity = 1.0f;
    float ySensitivity = 1.0f;

    float rotX = 0.0f;
    float rotY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1)) {
            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");
            rotX += x;
            rotY += -y;
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
        }

        var xRotation = Quaternion.AngleAxis(xSensitivity * rotX, Vector3.up);
        var yRotation = Quaternion.AngleAxis(ySensitivity * rotY, Vector3.right);

        var offset = xRotation * yRotation * origOffset;

        transform.localPosition = offset;
    }
}
