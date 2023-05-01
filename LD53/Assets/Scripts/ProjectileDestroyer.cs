using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision coll) {
        Destroy(transform.parent.gameObject);
    }
}
