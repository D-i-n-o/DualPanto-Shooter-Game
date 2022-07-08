using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float range = 200f;
    private Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }

    void Start()
    {
        Vector3 rotateBy = GameObject.Find("Player").GetComponent<Transform>().eulerAngles;
        transform.eulerAngles = rotateBy;
    }

    void Update()
    {
        
        if (OutOfRange())
        {
            Destroy(gameObject);
        }
    }
    //public GameObject HitEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy")){
            GameObject.Find("GameControl").GetComponent<SpeechControl>().PlaySound("ARROW HIT");
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
