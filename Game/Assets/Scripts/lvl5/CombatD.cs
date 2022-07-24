using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoFramework;
using static SpeechControlD.mapToAudio;

public class CombatD : MonoBehaviour
{
    public enum combatMode { LONG_RANGE, CLOSE_RANGE, SHIELD };
    // Start is called before the first frame update
    public GameObject ArrowPrefab;
    public Transform firePoint;
    public float velocity = 1.5f;
    public uint firerate = 10;       // interpreted as rounds per minute
    public uint ammunition = 30;
    public uint healthPoints = 100;
    public float recoilScale = 0.5f;

    protected float nextFire;
    protected PantoHandle upperHandle;
    protected SpeechControlD speech;
    protected combatMode playerMode;

    void Start()
    {
        upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        speech = GameObject.Find("GameControl").GetComponent<SpeechControlD>();
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
        if (!GameObject.Find("GameControl").GetComponent<GameControlD>().HasGameStarted()) return;
        if (nextFire < Time.time)
        {
            nextFire = Time.time + (60f / firerate);
            switch (playerMode)
            {
                case combatMode.LONG_RANGE: StartCoroutine(FireCrossbow()); break;
                case combatMode.CLOSE_RANGE: SwingSword(); break;
                case combatMode.SHIELD: UseShield(); break;
                default: break;
            }
        }
    }

    async Task Recoil(Vector3 direction)
    {
        if (recoilScale <= 0) return;
        //float rotation = upperHandle.GetRotation();
        await upperHandle.MoveToPosition(transform.position + (direction.normalized * recoilScale * (-1)), 99f, true);
        //upperHandle.Rotate(rotation);
    }

    private

    IEnumerator FireCrossbow()
    {
        if(ammunition > 0){
            Vector3 aimDirection = GetComponent<PlayerMovementD>().GetAimDirection();
            Task recoil = Recoil(aimDirection); 

            // Instantiate arrow to firepoint and adjust arrow rotation to player rotation
            GameObject arrow = Instantiate(ArrowPrefab, firePoint.position, Quaternion.Euler(transform.eulerAngles));
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.AddForce(aimDirection * velocity, ForceMode.Impulse);

            yield return new WaitUntil(() => recoil.IsCompleted);
            speech.PlayClip(ARROW_SHOT);
            ammunition--;
        }
        else{
            speech.PlayClip(DRYFIRE);
            Reload(); // for lvl " purposes, reload immediately
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
