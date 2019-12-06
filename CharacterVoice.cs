using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVoice : MonoBehaviour {

    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public float updateStep = 0.1f;

    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    public float clipLoudness;

    private float[] clipSampleData;

    public bool liveMicrophoneInput;





    void Awake()
    {
        if(!audioSource1)
        {
            Debug.Log("Error : No Audio Source");
        }

        clipSampleData = new float[sampleDataLength];

        

        
    }



    void Start()
    {
        


        if (liveMicrophoneInput)
        {

            LiveMic();
        }
    }

    void LiveMic()
    {
        
        audioSource1.clip = null;

        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        audioSource1 = GetComponent<AudioSource>();

        audioSource1.clip = Microphone.Start(Microphone.devices[0], true, 1, 48000);

        audioSource1.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) {  }
        audioSource1.Play();
    }

    void Update()
    {

        

        //plays the audioclip
        if (Input.GetKeyUp(KeyCode.S))
        {
            
                liveMicrophoneInput = false;
            
            
            audioSource1.Play();

            if(audioSource2 != null)
            {
                audioSource2.Play();
            }
            
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            audioSource1.Stop();
            if (audioSource2 != null)
            {
                audioSource2.Stop();
            }
            LiveMic();
            liveMicrophoneInput = true;
            
        }




        if(audioSource1.clip != null)
        {
            currentUpdateTime += Time.deltaTime;
            if (currentUpdateTime >= updateStep)
            {
                currentUpdateTime = 0f;
                audioSource1.clip.GetData(clipSampleData, audioSource1.timeSamples); // I read 1024 samples, which is about 80ms on a 44khz stereo clip, beginning at the current sample position of the clip.
                clipLoudness = 0f;
                foreach (var sample in clipSampleData)
                {
                    clipLoudness += Mathf.Abs(sample);
                }

                //clipLoudness /= sampleDataLength; // ClipLoudness is what you are looking for
            }
            //Debug.Log(clipLoudness);
            //Debug.Log(audioSource.timeSamples);


        }
        
    }
}
