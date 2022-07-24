using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using static SpeechControlA.mapToAudio;

public class EnemyA : MonoBehaviour
{
    public float speed = 0.5f;
    public int health = 100;
    public GameObject deathAnimation;
    //public AudioSource grunt;

    private GameObject player;
    private Rigidbody enemyRb;
    private PantoHandle lowerHandle;
    private SpeechControlA sfx;


    void Start()
    {

        enemyRb = GetComponent<Rigidbody>();
        sfx = GameObject.Find("GameControl").GetComponent<SpeechControlA>();
        player = GameObject.Find("Player");
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
    }

    void LateUpdate()
    {
        // LVL1: stationary opponent

        //Vector3 lookDirection = player.transform.position - transform.position;
        //enemyRb.AddForce(lookDirection.normalized * speed);
        //MoveEnemy(lookDirection);

        if (health <= 0)
        {
            //GameObject.Find("GameControl").GetComponent<SpeechControlA>().PlayClip(ENEMYDEATH);
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);        
        }
    }

    void MoveEnemy(Vector3 direction)
    {
        transform.position += (direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon")){
            if (health > 50){
                sfx.PlayClip(HIT);
                health -= 50;
            }
            else{
                health = 0;
                sfx.PlayClip(ENEMYDEATH);
                GameObject.Find("GameControl").GetComponent<GameControlA>().RegisterEnemyDeath();
            }
        }
    }
}
