//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Firing : MonoBehaviour
//{
//    // Start is called before the first frame update
//    public float velocity = 10f;
//    public float range = 10f;

//    private Vector3 direction = Vector3.zero;
//    private Vector3 startPosition;
//    private float rotateBy;
//    private bool initialized;

//    void Awake()
//    {
//        while (!initialized) ;
//    }
//    void Start()
//    {
//        startPosition = transform.position;
//        rotateBy = Vector3.Angle(direction, Vector3.back);
//        transform.eulerAngles = new Vector3(90f, rotateBy, 0f);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        transform.position += direction * velocity * 0.1f;
//        gameObject.SetActive(OutOfRange(range));
//    }

//    public void AwakeAndInit(Vector3 pDirection, float pVeloctiy = 10f, float pRange = 10f)
//    {
//        direction = pDirection; velocity = pVeloctiy; range = pRange;
//        initialized = true;
//    }

//    bool OutOfRange(float range)
//    {
//        return range < Vector3.Distance(startPosition, transform.position);
//    }


//    private void OnTriggerEnter(Collider other)
//    {
//        if(other.CompareTag("Enemy"))
//        {
//            GameObject.Find("GameControl").GetComponent<SpeechControl>().PlaySound("ARROW HIT");
//            gameObject.SetActive(false);
//        }
//    }
//}
