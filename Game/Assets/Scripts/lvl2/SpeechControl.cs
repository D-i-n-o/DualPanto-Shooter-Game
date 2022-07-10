using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechIO;
using DualPantoFramework;
using static Combat.combatMode;




public class SpeechControl : MonoBehaviour
{
    public enum mapToAudio
    {
        EQUIP_SWORD,
        EQUIP_BOW,
        EQUIP_SHIELD,
        ARROW_SHOT,
        DRYFIRE,
        RELOAD,
        HIT,
        ENEMYDEATH,
        INTRO1,
        INTRO2,
        INTRO3,
        LEVEL_CLEAR,
        VICTORY,
        GOODJOB,
        STARTJINGLE,
        GAMEOVER,
        PLAYERDEATH,
    }
    private AudioSource[] audioSrc;
    private SpeechIn speechRecognition;
    private GameObject player;

    void Start()
    {
        audioSrc = GetComponents<AudioSource>();          // order of inspector (reliable)
        speechRecognition = new SpeechIn(onSpeechRecognized);
        speechRecognition.StartListening(new string[] { "Schwert", "Sword", "Bow", "Bogen", "Schild", "Shield", "Reload", "Nachladen"});
        player = GameObject.Find("Player");
       
    }

    

    public IEnumerator PlayEndClips()
    {
        speechRecognition.StopListening();
        yield return new WaitForSeconds(PlayClip(mapToAudio.GOODJOB));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(PlayClip(mapToAudio.VICTORY));
        yield return new WaitForSeconds(PlayClip(mapToAudio.LEVEL_CLEAR) + 1);
        Destroy(gameObject);
    }


    // play clips triggered by non-speech-input events and return its length in seconds
    public float PlayClip(mapToAudio soundName)
    {
        Debug.Log(soundName);
        audioSrc[(int)soundName].Play();
        return audioSrc[(int)soundName].clip.length;
    }

    
    void onSpeechRecognized(string command)
    {
        switch (command) {
            case "Bow": case "Bogen":
                player.GetComponent<Combat>().SwitchMode(Combat.combatMode.LONG_RANGE);
                audioSrc[(int)mapToAudio.EQUIP_BOW].Play();
                break;
            case "Sword": case "Schwert":
                audioSrc[(int)mapToAudio.EQUIP_SWORD].Play();
                player.GetComponent<Combat>().SwitchMode(Combat.combatMode.CLOSE_RANGE);
                break;
            case "Shild": case "Schild":
                audioSrc[(int)mapToAudio.EQUIP_SHIELD].Play();
                player.GetComponent<Combat>().SwitchMode(Combat.combatMode.SHIELD);
                break;
            case "Reload": case "Nachladen":
                audioSrc[(int)mapToAudio.RELOAD].Play();
                player.GetComponent<Combat>().Reload();
                break;
            case "Burst": case "Burst-Fire": break;
            default: break;
        }
    }
}
