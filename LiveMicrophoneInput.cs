using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class LiveMicrophoneInput : MonoBehaviour {

    AudioSource audio;
    // Use this for initialization
    void Start () {
    
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        
        audio = GetComponent<AudioSource>();   
        audio.clip = Microphone.Start(Microphone.devices[0], true, 1, 22050);
        audio.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audio.Play();
    }
}
