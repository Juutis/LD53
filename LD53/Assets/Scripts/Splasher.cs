using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splasher : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem splash;

    private bool ready = true;

    public bool Enabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Water") {
            Splash();
        }
    }

    public void OnTriggerStay(Collider other) {
        if (other.tag == "Water") {
            Splash();
        }
    }

    private void Splash() {
        if (ready && Enabled) {
            var fx = Instantiate(splash);
            var pos = transform.position;
            pos.y = 0.0f;
            fx.transform.position = pos;
            ready = false;
            Invoke("Rearm", 0.1f);
        }
    }

    void Rearm() {
        ready = true;
    }
}
