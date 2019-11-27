// Code written by James Berry with help from Daniel Neale(https://github.com/DanielNeale) because he knows how music works

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
    // Volume multiplier (KEEP LOW TO KEEP EARS)
    [Range(0.01f, 5f)]
    public float volume;
    // Minimum value of the initial tone frequency
    public int minRange;
    // Maximum value of the initial tone frequency
    public int maxRange;
    // Temp value for multiplier between first and second frequency (FOR TESTING)
    public float freqModifier;

    // Shows the tones played in the inspector (values change by code, altering them in inspector does nothing)
    [SerializeField]
    private float[] tones;
    private List<float> samples;

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
        tones[0] = Random.Range(minRange, maxRange);
        tones[1] = tones[0] * freqModifier;        
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

        // List to hold the sample values
        samples = new List<float>();
        CreateSineWave(frequency, volume, 1);
        List<float> sineWave = samples;
        samples = new List<float>();
        CreateSquareWave(frequency, volume, 1);
        List<float> squareWave = samples;
        for (int i = 0; i < sineWave.Count; i++)
        {
            samples[i] = sineWave[i] + squareWave[i];
        }

        // Creates an array with the same data as the sample list
        // Since AudioClip.SetData requires an array
        float[] tonesToPlay = new float[samples.Count];
        tonesToPlay = samples.ToArray();

        // Creates the AudioClip that will be returned
        AudioClip audioClip = AudioClip.Create("tone", sampleLength, 1, sampleRate, false);
        audioClip.SetData(tonesToPlay, 0);
        return audioClip;
    }

    /// <summary>
    /// Adds to the "samples" list the sine wave of the frequencies input at the volume input
    /// </summary>
    /// <param name="frequency">Array of frequencies to create the sine wave from</param>
    /// <param name="volume">Multiplicative variable to change the amplitude of the wave and therefore the volume</param>
    /// <param name="noteLength">Total length of the wave</param>
    private void CreateSineWave(float[] frequency, float volume, float noteLength)
    {
        for (int i = 0; i < frequency.Length; i++)
        {
            for (int j = 0; j < sampleRate * noteLength; j++)
            {
                // Finds the values of each point on the sine wave of each tone according to sample rate
                float pointOnWave = Mathf.Sin(2.0f * Mathf.PI * frequency[i] * (j / (float)sampleRate));
                // Adjusts volume and adds it to the list of samples
                samples.Add(pointOnWave * volume);
            }
        }
        return;
    }

    /// <summary>
    /// Adds to the "samples" list the square waveform of the frequencies input at the volume input
    /// </summary>
    /// <param name="frequency">Array of frequencies to create the square wave from</param>
    /// <param name="volume">Multiplicative variable to change the amplitude of the wave and therefore the volume</param>
    /// <param name="noteLength">Total length of the wave</param>
    private void CreateSquareWave(float[] frequency, float volume, float noteLength)
    {
        for (int i = 0; i < frequency.Length; i++)
        {
            for (int j = 0; j < sampleRate * noteLength; j++)
            {
                // Finds the values of each point on the sine wave of each tone according to sample rate
                float pointOnWave = Mathf.Sign(Mathf.Sin(2.0f * Mathf.PI * frequency[i] * (j / (float)sampleRate)));
                // Adjusts volume and adds it to the list of samples
                samples.Add(pointOnWave * volume);
            }
        }
        return;
    }
}
