using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoBox : MonoBehaviour
{
    public CargoType Type;

    public Material[] materials;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        SetCargoType(Type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCargoType(CargoType type) {
        if (rend == null) {
            rend = GetComponentInChildren<Renderer>();
        }
        Type = type;
        var mats = rend.materials;
        mats[1] = materials[(int)Type];
        rend.sharedMaterials = mats;
    }
}
