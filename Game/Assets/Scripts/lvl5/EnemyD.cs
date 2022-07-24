using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using static SpeechControlD.mapToAudio;

public class EnemyD : MonoBehaviour
{
    public float speed = 0.1f;
    public int health = 100;
    public GameObject deathAnimation;
    public float impulseScale = 1f;
    //public AudioSource grunt;

    private GameObject player;
    private Rigidbody enemyRb;
    private PantoHandle lowerHandle;
    private SpeechControlD sfx;


    void Start()
    {

        enemyRb = GetComponent<Rigidbody>();
        sfx = GameObject.Find("GameControl").GetComponent<SpeechControlD>();
        player = GameObject.Find("Player");
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
    }

    void FixedUpdate()
    {
        if (!GameObject.Find("GameControl").GetComponent<GameControlD>().HasGameStarted()) return;
        
        // LVL4: chasing player
        Vector3 lookDirection = player.transform.position - transform.position;
        MoveEnemy(lookDirection);

        if (health <= 0)
        {
            //GameObject.Find("GameControl").GetComponent<SpeechControlD>().PlayClip(ENEMYDEATH);
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);        
        }
    }

    void MoveEnemy(Vector3 direction)
    {
        //enemyRb.AddForce(direction.normalized * speed);
        transform.position += (direction.normalized * speed * Time.fixedDeltaTime);
    }

    void HitImpulse(Vector3 direction)
    {
        transform.position += direction.normalized * impulseScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon")){
            Vector3 impulseDirection = transform.position - other.GetComponent<ArrowA>().GetStartPosition();
            HitImpulse(impulseDirection);
            if (health > 50){
                sfx.PlayClip(HIT);
                health -= 50;
            }
            else{
                health = 0;
                sfx.PlayClip(ENEMYDEATH);
                StartCoroutine(GameObject.Find("GameControl").GetComponent<GameControlD>().RegisterEnemyDeath());
                Destroy(gameObject);
            }
        }
    }
}
