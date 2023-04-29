using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Cargo : MonoBehaviour
{
    [SerializeField]
    private CargoBox cargoBox;

    private float spawnInterval = 10.0f;

    private CargoType type;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CargoType Attach(Transform target) {
        cargoBox.gameObject.SetActive(false);
        Invoke("Spawn", spawnInterval);
        return type;
    }

    public void Spawn() {
        var values = Enum.GetValues(typeof(CargoType));
        type = (CargoType)values.GetValue(Random.Range(0, values.Length));
        cargoBox.gameObject.SetActive(true);
        cargoBox.SetCargoType(type);
    }

}

public enum CargoType {
    FOOD = 0,
    WATER = 1,
    SUPPLIES = 2
}
