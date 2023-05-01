using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public Vector3 rotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, rotation.x * Time.deltaTime);
        transform.Rotate(Vector3.up, rotation.y * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotation.z * Time.deltaTime);
    }
}
