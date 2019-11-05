using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AudioTinker : MonoBehaviour {
    private AudioSource audioSource;
    private AudioClip[] outAudioClip;
    
    
    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        
        // Play generated puretone
        //outAudioClip = CreateToneAudioClip(1500);

        string audioPath = "Audio";
        outAudioClip = Resources.LoadAll<AudioClip>(audioPath);

        PlayOutAudio(0);
    }
    

    // Public APIs
    public void PlayOutAudio(int index) {
        audioSource.PlayOneShot(outAudioClip[index]);    
    }


    public void StopAudio() {
        audioSource.Stop();
    }
    
    
    // Private 
    private AudioClip CreateToneAudioClip(int frequency) {
        int sampleDurationSecs = 5;
        int sampleRate = 44100;
        int sampleLength = sampleRate * sampleDurationSecs;
        float maxValue = 1f / 4f;
        
        var audioClip = AudioClip.Create("tone", sampleLength, 1, sampleRate, false);
        
        float[] samples = new float[sampleLength];
        for (var i = 0; i < sampleLength; i++) {
            float s = Mathf.Sin(2.0f * Mathf.PI * frequency * ((float) i / (float) sampleRate));
            float v = s * maxValue;
            samples[i] = v;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }

}