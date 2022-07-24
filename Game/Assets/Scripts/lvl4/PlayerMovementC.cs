using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoFramework;

public class PlayerMovementC : MonoBehaviour
{ 
    private Rigidbody playerRb;
    private PantoHandle upperHandle;
    private Vector3 aimDirection = new Vector3(0, 0, -1f);

    async void Awake()
    {
        upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        playerRb = GetComponent<Rigidbody>();
        //await upperHandle.SwitchTo(gameObject, 5f);
    }

    void Update()
    {
        if (!GameObject.Find("GameControl").GetComponent<GameControlC>().HasGameStarted()) return;
        MovePlayer();
    }

    void FixedUpdate()
    {
        if (!GameObject.Find("GameControl").GetComponent<GameControlC>().HasGameStarted()) return;
        aimDirection = RotateAndAim();
    }
    void MovePlayer()
    {
        Vector3 moveTo = (upperHandle.HandlePosition(transform.position));
        transform.position = new Vector3(moveTo.x, 0.0f, moveTo.z);
        //Debug.Log(transform.forward);
    }

    Vector3 RotateAndAim()
    {
        //Debug.Log("Aiming...");
        float aimRotation = upperHandle.GetRotation();
        transform.eulerAngles = new Vector3(90f, aimRotation, 0f);

        // needs to be adjusted to object rotation
        Vector3 temp = Quaternion.AngleAxis(aimRotation + 45f, new Vector3(0f, 0f, 1f)) * new Vector3(1f, 1f, 0f);
        return (new Vector3(temp.x, 0f, temp.y * -1));
    }

    void UseSword()
    {

    }

    void UseShield()
    {

    }

    public Vector3 GetAimDirection()
    {
        return aimDirection;
    }


    void Fire()
    {

    }
}
