using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plane : MonoBehaviour
{
    [SerializeField]
    private GameObject ropedCargo;
    private Renderer[] ropedCargoGfx;
    [SerializeField]
    private CargoBox ropedCargoBox;
    [SerializeField]
    private Rigidbody ropedCargoObject;

    float minVerticalRotateSpeed = 100.0f;
    float maxVerticalRotateSpeed = 120.0f;
    float minHorizontalRotateSpeed = 75.0f;
    float maxHorizontalRotateSpeed = 90.0f;
    float speed = 15.0f;
    float minSpeed = 10.0f;
    float maxSpeed = 50.0f;

    [SerializeField]
    private DroppedCargo dropPrefab;

    Rigidbody rb;

    public bool hasCargo = false;

    [SerializeField]
    private Slider speedSlider;

    [SerializeField]
    private Button ejectButton;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ropedCargoGfx = ropedCargo.GetComponentsInChildren<Renderer>();
        disableRopedCargo();
    }

    // Update is called once per frame
    void Update()
    {
        var t = speed / maxSpeed;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var horizontalRotateSpeed = Mathf.Lerp(minHorizontalRotateSpeed, maxHorizontalRotateSpeed, t);
        var verticalRotateSpeed = Mathf.Lerp(minVerticalRotateSpeed, maxVerticalRotateSpeed, t);
        var yawRotation = transform.right.y * Mathf.Lerp(5.0f, 20.0f, (1-t));

        transform.Rotate(-Vector3.forward, x * Time.deltaTime * horizontalRotateSpeed * t);
        transform.Rotate(Vector3.right, y * Time.deltaTime * verticalRotateSpeed * t);
        transform.Rotate(-Vector3.up, yawRotation * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) {
            DropCargo();
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            speed += Time.deltaTime * 25.0f;
        } else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
            speed -= Time.deltaTime * 25.0f;
        }
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        speedSlider.value = (speed - minSpeed) / (maxSpeed - minSpeed);

        ejectButton.interactable = hasCargo;
    }

    void FixedUpdate() {
        rb.velocity = transform.forward * speed;
    }
    
    void OnTriggerEnter(Collider other) {
        var cargo = other.GetComponent<Cargo>();
        if (cargo != null) {
            var type = cargo.Attach(transform);
            ropedCargoBox.SetCargoType(type);
            enableRopedCargo();
            hasCargo = true;
        }

    }

    void disableRopedCargo() {
        foreach (var item in ropedCargoGfx)
        {
            item.enabled = false;
        }
    }

    void enableRopedCargo() {
        foreach (var item in ropedCargoGfx)
        {
            item.enabled = true;
        }
    }
    
    public void DropCargo() {
        if (!hasCargo) return;
        var droppedCargo = Instantiate(dropPrefab);
        droppedCargo.Drop(ropedCargoObject, ropedCargoBox.Type);
        disableRopedCargo();
        hasCargo = false;
    }

    public void CargoDelivered() {
        if (!hasCargo) return;
        hasCargo = false;
        disableRopedCargo();
    }

    public void SetSpeed() {
        speed = Mathf.Lerp(minSpeed, maxSpeed, speedSlider.value);
    }
}
