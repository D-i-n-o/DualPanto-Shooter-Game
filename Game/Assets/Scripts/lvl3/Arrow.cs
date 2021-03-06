using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpeechControl.mapToAudio;

public class Arrow : MonoBehaviour
{
    public float range = 100f;
    private Vector3 startPosition;
    //public GameObject HitEffect;

    void Awake()
    {
        startPosition = transform.position;
    }

    void LateUpdate()
    {
        
        if (OutOfRange())
        {
            Destroy(gameObject);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")){
            //GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 5f);
            Destroy(gameObject);
        }
    }

    private bool OutOfRange()
    {
        return range < Vector3.Distance(startPosition, transform.position);
    }


}
