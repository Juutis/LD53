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

    [SerializeField]
    private AudioSource engine;

    [SerializeField]
    private AudioSource sounds;

    private float minVerticalRotateSpeed = 100.0f;
    private float maxVerticalRotateSpeed = 120.0f;
    private float minHorizontalRotateSpeed = 75.0f;
    private float maxHorizontalRotateSpeed = 90.0f;
    private float speed = 15.0f;
    private float minSpeed = 10.0f;
    private float maxSpeed = 100.0f;

    [SerializeField]
    private DroppedCargo dropPrefab;

    Rigidbody rb;

    public bool hasCargo = false;

    [SerializeField]
    private Slider speedSlider;

    [SerializeField]
    private Button ejectButton;

    [SerializeField]
    private Splasher splasher;

    [SerializeField]
    private ParticleSystem explosion;

    private bool alive = true;

    [SerializeField]
    private GameObject planeMesh;

    private int layerMask;

    private float curSpeed;

    private Deliverable deliv;

    [SerializeField]
    private AudioClip pickUp;
    
    [SerializeField]
    private AudioClip drop;
    
    [SerializeField]
    private AudioClip boom;

    private Quaternion forward;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ropedCargoGfx = ropedCargo.GetComponentsInChildren<Renderer>();
        disableRopedCargo();
        layerMask = LayerMask.GetMask("Default", "Ground");
        curSpeed = speed;
        deliv = GetComponentInChildren<Deliverable>();

        forward = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) return;

        if (Time.timeScale < 0.5f) return;

        var t = (curSpeed - minSpeed) / (maxSpeed - minSpeed);
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        if(!WorldManager.main.invertControls) y = -y;

        var horizontalRotateSpeed = Mathf.Lerp(minHorizontalRotateSpeed, maxHorizontalRotateSpeed, t);
        var verticalRotateSpeed = Mathf.Lerp(minVerticalRotateSpeed, maxVerticalRotateSpeed, t);
        var yawRotation = planeMesh.transform.right.y * Mathf.Lerp(10.0f, 25.0f, (1-t));

        forward = forward * Quaternion.AngleAxis(x * Time.deltaTime * horizontalRotateSpeed, transform.forward);
        forward = forward * Quaternion.AngleAxis(y * Time.deltaTime * verticalRotateSpeed, transform.right);
        forward = forward * Quaternion.AngleAxis(yawRotation * Time.deltaTime, transform.up);
        

        planeMesh.transform.Rotate(-Vector3.forward, x * Time.deltaTime * horizontalRotateSpeed);
        planeMesh.transform.Rotate(Vector3.right, y * Time.deltaTime * verticalRotateSpeed);
        planeMesh.transform.Rotate(-Vector3.up, yawRotation * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) {
            DropCargo();
        }

        if (Input.GetKey(KeyCode.V)) {
            speed += Time.deltaTime * 50.0f;
        } else if (Input.GetKey(KeyCode.C)) {
            speed -= Time.deltaTime * 50.0f;
        }
        var mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        speed += mouseScroll * 50.0f;

        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        speedSlider.value = (speed - minSpeed) / (maxSpeed - minSpeed);

        curSpeed = Mathf.MoveTowards(curSpeed, speed, Time.deltaTime * 30.0f);

        ejectButton.interactable = hasCargo;
        //rb.MoveRotation(transform.rotation);

        engine.pitch = Mathf.Lerp(0.9f, 1.5f, t);
    }

    void FixedUpdate() {
        if (!alive) return;
        rb.velocity = planeMesh.transform.forward * curSpeed;
    }
    
    void OnTriggerEnter(Collider other) {
        var cargo = other.GetComponent<Cargo>();
        if (cargo != null && cargo.Ready && !hasCargo) {
            var type = cargo.Attach(transform);
            ropedCargoBox.SetCargoType(type);
            deliv.Type = type;
            enableRopedCargo();
            hasCargo = true;
            sounds.PlayOneShot(pickUp);
        }
        if (other.tag == "Water" && alive) {
            DropCargo();
            Kill(false);
        }
        if (other.tag == "CannonBall" && alive) {
            DropCargo();
            var fx = Instantiate(explosion);
            fx.transform.position = transform.position;
            planeMesh.SetActive(false);
            Kill(true);
            sounds.PlayOneShot(boom);
        }

    }

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Death" && alive) {
            DropCargo();
            var fx = Instantiate(explosion);
            fx.transform.position = transform.position;
            planeMesh.SetActive(false);
            Kill(false);
            sounds.PlayOneShot(boom);
        }
    }

    Vector3 respawnPos;
    Vector3 respawnDir;

    void Kill(bool byCannonBall) {
        alive = false;
        var dir = planeMesh.transform.forward;
        dir.y = 0.0f;
        respawnPos = transform.position;
        float setBack = 30.0f;
        if (byCannonBall) setBack = 500.0f;
        respawnPos += -dir.normalized * setBack;
        respawnPos.y = 75.0f;

        RaycastHit hit;

        if (Physics.Raycast(respawnPos + Vector3.up * 1000.0f, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            respawnPos = hit.point + Vector3.up * 75.0f;
        }
        
        respawnDir = dir;
        rb.useGravity = true;
        Invoke("Respawn", 2.0f);
        engine.Stop();
    }

    void Respawn() {
        alive = true;
        planeMesh.SetActive(true);
        planeMesh.transform.forward = respawnDir;
        transform.position = respawnPos;
        rb.MovePosition(transform.position);
        disableRopedCargo();
        rb.MoveRotation(transform.rotation);
        rb.useGravity = false;
        speed = 15.0f;
        curSpeed = speed;
        engine.Play();
    }

    void disableRopedCargo() {
        foreach (var item in ropedCargoGfx)
        {
            item.enabled = false;
        }
        splasher.Enabled = false;
    }

    void enableRopedCargo() {
        foreach (var item in ropedCargoGfx)
        {
            item.enabled = true;
        }
        splasher.Enabled = true;
    }
    
    public void DropCargo() {
        if (!hasCargo) return;
        var droppedCargo = Instantiate(dropPrefab);
        droppedCargo.Drop(ropedCargoObject, ropedCargoBox.Type);
        disableRopedCargo();
        hasCargo = false;
        sounds.PlayOneShot(drop);
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
