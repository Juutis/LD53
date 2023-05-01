using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    [SerializeField]
    private GameObject pipe;

    private Rigidbody player;

    private bool readyToFire = true;

    private Vector3 targetDir = Vector3.forward;

    [SerializeField]
    private Projectile projectile;

    [SerializeField]
    private ParticleSystem fx;

    [SerializeField]
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    Vector3 targetPos;

    // Update is called once per frame
    void Update()
    {
        targetPos = predictedPosition(player.position, transform.position, player.velocity, Projectile.projSpeed);
        if (Vector3.Distance(transform.position, player.position) < 1000) {
            if (readyToFire) {
                readyToFire = false;
                Invoke("Fire", 2.5f);
            }
            var dir = targetPos - transform.position;
            pipe.transform.forward = Vector3.RotateTowards(pipe.transform.forward, dir, Time.deltaTime * 2, 0.0f);
            dir.y = 0.0f;
            transform.forward = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * 2, 0.0f);
        }
    }

    public void Fire() {
        readyToFire = true;
        if (Vector3.Distance(transform.position, player.position) > 1000) {
            return;
        }
        var proj = Instantiate(projectile);
        proj.transform.position = fx.transform.position;
        proj.Launch();
        fx.Play();
        audio.Play();
    }

    private Vector3 predictedPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed)
     {
         Vector3 displacement = targetPosition - shooterPosition;
         float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
         //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
         if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
         {
             Debug.Log("Position prediction is not feasible.");
             return targetPosition;
         }
         //also Sine Formula
         float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
         return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
     }


}
