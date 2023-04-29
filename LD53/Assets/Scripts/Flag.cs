using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    Transform cam;
    Cloth cloth;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cloth = GetComponent<Cloth>();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = cam.position - transform.position;
        dir.y = 0.0f;
        dir = Vector3.Cross(dir, Vector3.up);
        cloth.externalAcceleration = dir.normalized * 100.0f;
    }
}
