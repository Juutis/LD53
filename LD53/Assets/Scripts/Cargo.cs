using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;


public class Cargo : MonoBehaviour
{
    [SerializeField]
    private CargoBox cargoBox;

    private float spawnInterval = 10.0f;

    private CargoType type;

    public bool Ready = false;
    public bool IsTutorial = false;
    public bool started = false;

    public static int spawnCounter;

    // Start is called before the first frame update
    void Start()
    {
        if (IsTutorial) {
            type = CargoType.FOOD;
        }
        Spawn();
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started && !IsTutorial && !WorldManager.main.Tutorial) {
            Invoke("Spawn", Random.Range(0.0f, 1.0f));
            started = true;
        }
    }

    public CargoType Attach(Transform target) {
        cargoBox.gameObject.SetActive(false);
        Invoke("Spawn", spawnInterval);
        Ready = false;

        if (IsTutorial && WorldManager.main.Tutorial) {
            Invoke("TriggerTutorial", 1.0f);
        }

        return type;
    }

    public void TriggerTutorial() {
        Tutorial.main.Show3();
    }

    private bool firstSpawn = true;

    public void Spawn() {
        if (!IsTutorial || !WorldManager.main.Tutorial) {
            var values = WorldManager.main.camps.Select(it => it.wantedType).Distinct().ToList();
            if (firstSpawn || WorldManager.main.camps.Count == 0) {
                values = new List<CargoType>();
                values.Add(CargoType.FOOD);
                values.Add(CargoType.SUPPLIES);
                values.Add(CargoType.WATER);
            } 
            type = values[spawnCounter % values.Count];
        }
        cargoBox.gameObject.SetActive(true);
        cargoBox.SetCargoType(type);
        Ready = true;
        spawnCounter++;
        firstSpawn = false;
    }

}

public enum CargoType {
    FOOD = 0,
    WATER = 1,
    SUPPLIES = 2
}
