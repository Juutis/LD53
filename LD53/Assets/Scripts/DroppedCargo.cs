using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCargo : MonoBehaviour
{

    Rigidbody rb;

    [SerializeField]
    private CargoBox cargoBox;

    public CargoType type;

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop(Rigidbody target, CargoType type) {
        transform.rotation = target.transform.rotation;
        transform.position = target.transform.position;
        rb = GetComponent<Rigidbody>();
        rb.velocity = target.velocity;
        this.type = type;
        cargoBox.SetCargoType(type);
    }

    public void Delivered() {
        Destroy(gameObject);
    }

}
