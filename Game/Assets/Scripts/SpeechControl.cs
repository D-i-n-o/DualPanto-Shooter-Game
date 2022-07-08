using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechIO;
using DualPantoFramework;


public class SpeechControl : MonoBehaviour
{
    private SpeechIn speechRecognition;
    private GameObject player;
    private AudioSource[] audioSources;

    enum mapToAudioSrc{
        DRAW_SWORD,
        DRAW_BOW,
        EQUIP_SHIELD,
        HIT,
        INTRO,
        END
    }
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        speechRecognition = new SpeechIn(onSpeechRecognized);
        speechRecognition.StartListening(new string[] { "Schwert", "Sword", "Bow", "Bogen", "Schild", "Shield", "Pause", "Resume"});
        player = GameObject.Find("Player");
       
    }

    // Update is called once per frame
    void Update()
    {
        //if (GetComponent<GameControl>().HasGameFinished()){
        //    audioSources[(int)mapToAudioSrc.END].Play();
        //}
    }

    public void PlaySound(string soundName)
    {
        switch (soundName){
            case "ARROW HIT": /*audioSources[(int)mapToAudioSrc.HIT].Play();*/ break;
            default: break;
        }
    }

    void onSpeechRecognized(string command)
    {
        Debug.Log(command);
        //if(command == "Bow" || command == "Bogen"){
        //    player.GetComponent<Combat>().SwitchMode((playerMode.Mode)0);
        //    audioSources[(int)mapToAudioSrc.DRAW_BOW].Play();
        //}
        //else if(command == "Sword" || command == "Schwert" ){
        //    audioSources[(int)mapToAudioSrc.DRAW_SWORD].Play();
        //    player.GetComponent<PlayerAction>().SwitchMode((PlayerAction.Mode)1);
        //}
        //else if(command == "Shild" || command == "Schild") {
        //    audioSources[(int)mapToAudioSrc.EQUIP_SHIELD].Play();
        //    player.GetComponent<PlayerAction>().SwitchMode((PlayerAction.Mode)2);
        //}
    }
}
