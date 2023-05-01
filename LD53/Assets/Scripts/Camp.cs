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

    public CargoType wantedType;
    private bool acceptsGoods;

    private float requireIntervalMin = 2.0f;
    private float requireIntervalMax = 5.0f;

    private Plane plane;

    [SerializeField]
    private ParticleSystem fire;

    [SerializeField]
    private ParticleSystem love;

    public bool IsTutorial = false;
    public bool started = false;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip success;
    [SerializeField]
    private AudioClip fail;

    public static int spawnCounter;

    // Start is called before the first frame update
    void Start()
    {
        flag.enabled = false;
        plane = GameObject.FindGameObjectWithTag("Player").GetComponent<Plane>();
        WorldManager.main.RegisterCamp(this);
        if (IsTutorial) {
            wantedType = CargoType.FOOD;
            requireRandomGoods();
            started = true;
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!started && !IsTutorial && !WorldManager.main.Tutorial) {
            startCooldown();
            started = true;
        }
    }

    void requireRandomGoods() {
        if (!IsTutorial) {
            var values = Enum.GetValues(typeof(CargoType));
            wantedType = (CargoType)values.GetValue(spawnCounter++ % values.Length);
        }
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

    public bool receiveGoods(CargoType type) {
        if (!acceptsGoods) return false;

        if (acceptsGoods && type == wantedType) {
            LowerFlag();
            acceptsGoods = false;
            fire.Stop();
            love.Play();
            WorldManager.main.CampCompleted(this);
            audioSource.PlayOneShot(success);

            if (IsTutorial && WorldManager.main.Tutorial) {
                Invoke("TriggerTutorial", 1.0f);
            }
        }
        else {
            audioSource.PlayOneShot(fail);
        }
        return true;
    }

    public void TriggerTutorial() {
        Tutorial.main.Show4();
    }

    void startCooldown() {
        Invoke("requireRandomGoods", Random.Range(requireIntervalMin, requireIntervalMax));
    }

    void OnTriggerEnter(Collider other) {

        var cargoBox = other.GetComponentInChildren<CargoBox>();
        if (cargoBox != null) {

        }

    }
}
