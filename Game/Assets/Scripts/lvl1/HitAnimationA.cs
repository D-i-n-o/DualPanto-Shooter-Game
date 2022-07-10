using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnimationA : MonoBehaviour
{
    // This is a lazy DIY hit animation, but that's okay, DualPanto is targeted at the visually impaired anyway...
    public GameObject[] hitEffects;

    IEnumerator Start()
    {
        foreach (GameObject effect in hitEffects)
        {
            GameObject eff = Instantiate(effect, transform.position, Quaternion.Euler(new Vector3(90f, 0f, 0f)));
            yield return new WaitForSeconds(0.1f);
            Destroy(eff);
        }
        Destroy(gameObject);
    }
}