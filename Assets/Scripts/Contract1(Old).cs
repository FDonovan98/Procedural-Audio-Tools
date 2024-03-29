﻿// Code written by James Berry with help from Daniel Neale(https://github.com/DanielNeale) because he knows how music works

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract1 : MonoBehaviour
{
    // Total length of the full audio clip
    private float totalLength;
    private int sampleLength;
    // Samples per second
    [SerializeField]
    private int sampleRate;
    // Volume multiplier (KEEP LOW TO KEEP EARS)
    [Range(0.01f, 5f)]
    public float volume;

    // Number of tones in the damage effect
    private int DAMAGE_COUNT = 4;
    // Frequency of first damage tone
    private int DAMAGE_FREQUENCY_MIN = 30;
    private int DAMAGE_FREQUENCY_MAX = 170;

    // Number of tones in the coin pickup effect
    private int COIN_PICKUP_COUNT = 4;
    // Frequency of first coin pickup tone
    private int COIN_PICKUP_FREQUENCY_MIN = 1000;
    private int COIN_PICKUP_FREQUENCY_MAX = 2000;

    // Array of tones to play (just to see it in the inspector. Code will change it regardless of what the inspector says)
    [SerializeField]
    private float[] tones;
    // Array of the lengths of the tones (also makes no difference with value changes in inspector)
    private float[] toneLengths;
    private List<float> samples;
    private List<float> newSamples;

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
        outAudioClip = CreateToneAudioClip(tones, toneLengths);
        // Plays tone once
        audioSource.PlayOneShot(outAudioClip);
    }

    /// <summary>
    /// Generates an AudioClip within a range
    /// Run when "Player Takes Damage" button is pressed
    /// </summary>
    public void DamageTone()
    {
        totalLength = 0;
        tones = new float[DAMAGE_COUNT];
        toneLengths = new float[DAMAGE_COUNT];
        tones[0] = Random.Range(DAMAGE_FREQUENCY_MIN, DAMAGE_FREQUENCY_MAX);
        tones[1] = tones[0] * .5f;
        tones[2] = tones[0] * .25f;
        tones[3] = tones[0] * .15f;
        toneLengths[0] = 0.08f;
        toneLengths[1] = 0.03f;
        toneLengths[2] = 0.03f;
        toneLengths[3] = 0.05f;
        foreach (float time in toneLengths)
        {
            totalLength += time;
        }
        PlayTone();
    }

    /// <summary>
    /// Generates an AudioClip within a range
    /// Run when "Player Collects Coin" button is pressed
    /// </summary>
    public void CoinPickupTone()
    {
        totalLength = 0;
        tones = new float[COIN_PICKUP_COUNT];
        toneLengths = new float[COIN_PICKUP_COUNT];
        tones[0] = Random.Range(COIN_PICKUP_FREQUENCY_MIN, COIN_PICKUP_FREQUENCY_MAX);
        tones[1] = tones[0] * 3f;
        tones[2] = tones[0] * 7f;
        toneLengths[0] = 0.08f;
        toneLengths[1] = 0.03f;
        toneLengths[2] = 0.03f;
        foreach (float time in toneLengths)
        {
            totalLength += time;
        }
        PlayTone();
    }

    /// <summary>
    /// Using an array input of tone frequencies, will create a sine wave pure tone
    /// </summary>
    /// <param name="frequency">Array of tones (as float values) to create a clip from</param>
    /// <returns>AudioClip containing a sequence of tones that can be played through the AudioSource</returns>
    private AudioClip CreateToneAudioClip(float[] frequency, float[] toneLengths)
    {
        samples = new List<float>();

        // Computes length of sample
        sampleLength = Mathf.RoundToInt(sampleRate * totalLength);
        // Resets list
        newSamples = new List<float>();
        CreateSineWave(frequency, volume, toneLengths);
        List<float> sineWave = newSamples;
        newSamples = new List<float>();
        CreateSquareWave(frequency, volume, toneLengths);
        List<float> squareWave = newSamples;
        for (int j = 0; j < newSamples.Count; j++)
        {
            newSamples[j] = sineWave[j] + squareWave[j];
            samples.Add(newSamples[j]);
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
    private void CreateSineWave(float[] frequency, float volume, float[] noteLength)
    {
        for (int i = 0; i < frequency.Length; i++)
        {
            for (int j = 0; j < sampleRate * noteLength[i]; j++)
            {
                // Finds the values of each point on the sine wave of each tone according to sample rate
                float pointOnWave = Mathf.Sin(2.0f * Mathf.PI * frequency[i] * (j / (float)sampleRate));
                // Adjusts volume and adds it to the list of samples
                newSamples.Add(pointOnWave * volume);
            }
        }
    }

    /// <summary>
    /// Adds to the "samples" list the square waveform of the frequencies input at the volume input
    /// </summary>
    /// <param name="frequency">Array of frequencies to create the square wave from</param>
    /// <param name="volume">Multiplicative variable to change the amplitude of the wave and therefore the volume</param>
    /// <param name="noteLength">Total length of the wave</param>
    private void CreateSquareWave(float[] frequency, float volume, float[] noteLength)
    {
        for (int i = 0; i < frequency.Length; i++)
        {
            for (int j = 0; j < sampleRate * noteLength[i]; j++)
            {
                // Finds the values of each point on the sine wave of each tone according to sample rate
                float pointOnWave = Mathf.Sign(Mathf.Sin(2.0f * Mathf.PI * frequency[i] * (j / (float)sampleRate)));
                // Adjusts volume and adds it to the list of samples
                newSamples.Add(pointOnWave * volume);
            }
        }
        return;
    }
}
