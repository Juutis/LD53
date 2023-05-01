using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static float projSpeed = 300.0f;

    private Vector3 targetPos;



    // Start is called before the first frame update
    void Start()
    {

    }

    public void Kill() {
        Destroy(gameObject);
    }

    public void Launch() {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        targetPos = predictedPosition(player.position, transform.position, player.velocity, projSpeed);
        GetComponent<Rigidbody>().velocity = (targetPos - transform.position).normalized * projSpeed;
        Invoke("Kill", 5.0f);
        GetComponent<Rigidbody>().MovePosition(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision coll) {
        Destroy(gameObject);
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
