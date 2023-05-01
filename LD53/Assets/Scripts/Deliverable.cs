using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deliverable : MonoBehaviour
{
    private DroppedCargo droppedCargo;

    public CargoType Type;

    public bool roped;
    private Plane plane;

    // Start is called before the first frame update
    void Start()
    {
        droppedCargo = GetComponent<DroppedCargo>();
        plane = GameObject.FindGameObjectWithTag("Player").GetComponent<Plane>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        var camp = other.GetComponent<Camp>();
        if (camp != null) {
            if (droppedCargo != null && camp.receiveGoods(Type)) {
                droppedCargo.Delivered();
            }
            if (roped && plane) {
                if (plane.hasCargo) {
                    camp.receiveGoods(Type);
                    plane.CargoDelivered();
                    return;
                }
            }
        }
    }
}
