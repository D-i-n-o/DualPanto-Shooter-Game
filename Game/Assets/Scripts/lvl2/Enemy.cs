using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using static SpeechControl.mapToAudio;

public class Enemy : MonoBehaviour
{
    public float speed = 2.5f;
    public int health = 100;
    public GameObject deathAnimation;
    public AudioSource grunt;

    private static GameObject player;
    private Rigidbody enemyRb;
    private static SpeechControl sfx;
    private float loopTime;


    void Start()
    {

        enemyRb = GetComponent<Rigidbody>();
        sfx = GameObject.Find("GameControl").GetComponent<SpeechControl>();
        player = GameObject.Find("Player");
        loopTime = Time.time;
    }

    void FixedUpdate(){
        MoveInASquare(2f);
    }

    void LateUpdate()
    {
        if (health <= 0)
        {
            //GameObject.Find("GameControl").GetComponent<SpeechControl>().PlayClip(ENEMYDEATH);
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void MoveInASquare(float squareSize)
    {
        if (loopTime + squareSize >= Time.time)
        {
            //enemyRb.AddForce(Vector3.right * speed * Time.fixedDeltaTime);
            MoveEnemy(Vector3.right);
        }
        else if (loopTime + 2 * squareSize >= Time.time)
        {
            //enemyRb.AddForce(Vector3.forward * speed * Time.fixedDeltaTime);
            MoveEnemy(Vector3.forward);
        }
        else if (loopTime + 3 * squareSize >= Time.time)
        {
            MoveEnemy(Vector3.left);
        }
        else if (loopTime + 4 * squareSize >= Time.time)
        {
            MoveEnemy(Vector3.back);
        }
        else
        {
            loopTime = Time.time;
        }
    }

    private void HuntPlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        enemyRb.AddForce(lookDirection.normalized * speed);
        MoveEnemy(lookDirection);
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
                GameObject.Find("GameControl").GetComponent<GameControl>().RegisterEnemyDeath();
            }
        }
    }
}
