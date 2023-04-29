using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Camp : MonoBehaviour
{
    [SerializeField]
    private Renderer flag;

    [SerializeField]
    private Material[] flagMaterials;

    private CargoType wantedType;
    private bool acceptsGoods;

    private float requireIntervalMin = 5.0f;
    private float requireIntervalMax = 10.0f;

    private Plane plane;

    // Start is called before the first frame update
    void Start()
    {
        flag.enabled = false;
        startCooldown();
        plane = GameObject.FindGameObjectWithTag("Player").GetComponent<Plane>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void requireRandomGoods() {
        var values = Enum.GetValues(typeof(CargoType));
        wantedType = (CargoType)values.GetValue(Random.Range(0, values.Length));
        RaiseFlag(wantedType);
        acceptsGoods = true;
    }

    void RaiseFlag(CargoType type) {
        flag.material = flagMaterials[(int)type];
        flag.enabled = true;
    }

    void LowerFlag() {
        flag.enabled = false;
    }

    void receiveGoods(CargoType type) {
        if (acceptsGoods && type == wantedType) {
            Invoke("requireRandomGoods", Random.Range(requireIntervalMin, requireIntervalMax));
            LowerFlag();
            acceptsGoods = false;
        }
    }

    void startCooldown() {
        Invoke("requireRandomGoods", Random.Range(requireIntervalMin, requireIntervalMax));
    }

    void OnTriggerEnter(Collider other) {
        if (!acceptsGoods) return;

        var droppedCargo = other.GetComponent<DroppedCargo>();
        if (droppedCargo != null) {
            receiveGoods(droppedCargo.type);
            droppedCargo.Delivered();
            return;
        }
        var cargoBox = other.GetComponentInChildren<CargoBox>();
        if (cargoBox != null) {
            if (plane.hasCargo) {
                receiveGoods(cargoBox.Type);
                plane.CargoDelivered();
                return;
            }
        }

    }
}
