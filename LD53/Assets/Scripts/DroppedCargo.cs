using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCargo : MonoBehaviour
{

    Rigidbody rb;

    [SerializeField]
    private CargoBox cargoBox;

    public CargoType type;
    private Deliverable deliv;

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody>();
        deliv = GetComponent<Deliverable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop(Rigidbody target, CargoType type) {
        rb = GetComponent<Rigidbody>();
        deliv = GetComponent<Deliverable>();
        transform.rotation = target.rotation;
        transform.position = target.position;
        rb.velocity = target.velocity;
        rb.rotation = target.rotation;
        rb.position = target.position;
        this.type = type;
        cargoBox.SetCargoType(type);
        deliv.Type = type;
        rb.angularVelocity = target.angularVelocity;
    }

    public void Delivered() {
        Destroy(gameObject);
    }

}
