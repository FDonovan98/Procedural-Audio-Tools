using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract1 : MonoBehaviour
{
    // How long each tone will play (currently not working)
    [SerializeField]
    private float sampleDurationSecs;
    // Samples per second
    [SerializeField]
    private int sampleRate;

    // Shows the tones played in the inspector (values change by code, altering them in inspector does nothing)
    [SerializeField]
    private float[] tones;

    // Holds the component that plays the audio
    private AudioSource audioSource;
    // Final audio tone sequence to play
    private AudioClip outAudioClip;

    private void Awake()
    {
        // Finds the AudioSource Component
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Gets a new tone and plays it
    /// </summary>
    public void PlayTone()
    { 
        outAudioClip = CreateToneAudioClip(tones);
        // Plays tone once
        audioSource.PlayOneShot(outAudioClip);
    }

    /// <summary>
    /// Generates random tones (Currently hardcoded as 2)
    /// Run when button is pressed
    /// </summary>
    public void CoinPickupTone()
    {
        // Array of frequencies to parse into audio tones
        tones = new float[2];
        //For the sake of testing
        //these frequencies are hardcoded such that
        //one is 3 times more than the other which is chosen
        //randomly. This is so that they are different enough
        //that I can tell that it's working properly
        tones[0] = Random.Range(1000, 1200);
        tones[1] = tones[0] * 3f;        
        PlayTone();
    }


    /// <summary>
    /// Using an array input of tone frequencies, will create a sine wave pure tone
    /// </summary>
    /// <param name="frequency">Array of tones (as float values) to create a clip from</param>
    /// <returns>AudioClip containing a sequence of tones that can be played through the AudioSource</returns>
    private AudioClip CreateToneAudioClip(float[] frequency)
    {
        // Computes length of sample
        int sampleLength = Mathf.RoundToInt(sampleRate * sampleDurationSecs * frequency.Length);
        // Volume multiplier (KEEP LOW TO KEEP EARS)
        float volume = .25f;

        // List to hold the sample values
        List<float> samples = new List<float>();

        // Nested for loops find each point on a sound wave for each frequency parsed in
        for (int i = 0; i < frequency.Length; i++)
        {
            for (int j = 0; j < sampleRate; j++)
            {
                // Finds the values of each point on the sine wave of each tone according to sample rate
                float pointOnWave = Mathf.Sin(2.0f * Mathf.PI * frequency[i] * ((float)j / (float)sampleRate));
                // Adjusts volume and adds it to the list of samples
                samples.Add(pointOnWave * volume);
            }
        }

        // Creates an array with the same data as the sample list
        float[] tonesToPlay = new float[samples.Count];
        tonesToPlay = samples.ToArray();

        // Creates the AudioClip that will be returned
        AudioClip audioClip = AudioClip.Create("tone", sampleLength, 1, sampleRate, false);
        audioClip.SetData(tonesToPlay, 0);
        return audioClip;
    }
}
