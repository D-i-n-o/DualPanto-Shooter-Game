using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ArrowPrefab;
    public Transform firePoint;
    public float velocity = 1.5f;
    public uint firerate = 60;       // interpreted as rounds per minute
    public uint ammunition = 20;

    private float nextFire;
    
    private combatMode playerMode;

    public enum combatMode { LONG_RANGE, CLOSE_RANGE, SHIELD };

    void Awake()
    {
        nextFire = Time.time;
        playerMode = combatMode.LONG_RANGE;
    }

    void  FixedUpdate()
    {
        if(nextFire < Time.time && ammunition > 0)
        {
            nextFire = Time.time + (60f / firerate);
            switch (playerMode){
                    case combatMode.LONG_RANGE: FireCrossbow(); break;
                    default: break;
            }
        }
    }

    void FireCrossbow()
    {
        Vector3 aimDirection = GetComponent<PlayerMovement>().GetAimDirection();
        // Instantiate arrow to firepoint and adjust arrow rotation to player rotation
        GameObject arrow = Instantiate(ArrowPrefab, firePoint.position, Quaternion.Euler(new Vector3(90f, 0f, 0f)));

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(aimDirection * velocity, ForceMode.Impulse);
        ammunition--;
    }

    //public void AwakeAndInit(Vector3 pDirection, float pVeloctiy = 10f, float pRange = 10f)
    //{
    //    direction = pDirection; velocity = pVeloctiy; range = pRange;
    //    initialized = true;
    //}

    //bool OutOfRange(float range)
    //{
    //    return range < Vector3.Distance(startPosition, transform.position);
    //}





    // called by SpeechControl after user voice input command
    public void SwitchMode(combatMode mode)
    {
        playerMode = mode;
    }
}
