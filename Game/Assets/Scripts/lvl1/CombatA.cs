using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpeechControlA.mapToAudio;

public class CombatA : MonoBehaviour
{
    public enum combatMode { LONG_RANGE, CLOSE_RANGE, SHIELD };
    // Start is called before the first frame update
    public GameObject ArrowPrefab;
    public Transform firePoint;
    public float velocity = 1.5f;
    public uint firerate = 15;       // interpreted as rounds per minute
    public uint ammunition = 30;
    public uint healthPoints = 100;

    protected float nextFire;
    protected SpeechControlA speech;
    protected combatMode playerMode;

    void Start()
    {
        speech = GameObject.Find("GameControl").GetComponent<SpeechControlA>();
        nextFire = Time.time;
        playerMode = combatMode.LONG_RANGE;
    }

    private void Update()
    {
        //if (healthPoints <= 0){
        //    PlayerDeath();
        //}
    }

    private void FixedUpdate()
    {
        if (!GameObject.Find("GameControl").GetComponent<GameControlA>().HasGameStarted()) return;
        if (nextFire < Time.time)
        {
            nextFire = Time.time + (60f / firerate);
            switch (playerMode)
            {
                case combatMode.LONG_RANGE: FireCrossbow(); break;
                case combatMode.CLOSE_RANGE: SwingSword(); break;
                case combatMode.SHIELD: UseShield(); break;
                default: break;
            }
        }
    }

    protected void FireCrossbow()
    {
        if(ammunition > 0){
            Vector3 aimDirection = GetComponent<PlayerMovementA>().GetAimDirection();
            // Instantiate arrow to firepoint and adjust arrow rotation to player rotation
            GameObject arrow = Instantiate(ArrowPrefab, firePoint.position, Quaternion.Euler(transform.eulerAngles));

            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.AddForce(aimDirection * velocity, ForceMode.Impulse);
            speech.PlayClip(ARROW_SHOT);
            ammunition--;
        }
        else{
            speech.PlayClip(DRYFIRE);
            Reload(); // for lvl 1 purposes, reload immediately
        }
       
    }

    public void Reload()
    {
        ammunition = 30;
    }

    protected void UseShield()
    {
        //TODO Instantiate 
    }

    protected void SwingSword()
    {
        // TODO: Instantiate Sword and play swing sound
    }

    protected void PlayerDeath()
    {
        //speech.PlayClip(PLAYERDEATH);
        Destroy(gameObject);
    }

    // called by SpeechControl after user voice input command
    public void SwitchMode(combatMode mode)
    {
        playerMode = mode;
    }

    //private void OnTriggerEnter(Collider other)
    //{
        
    //}
}
