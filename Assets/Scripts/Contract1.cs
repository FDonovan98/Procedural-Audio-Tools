/*
 * Author: James Berry
 * Link To Repository: https://github.com/HDonovan96/Procedural-Audio-Tools
 * License: MIT
 */

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
    // Minimum value of the initial tone frequency
    public int minRange;
    // Maximum value of the initial tone frequency
    public int maxRange;
    // Temp value for multiplier between first and second frequency (FOR TESTING)
    public float freqModifier;

    // Total number of tones for each sound effect
    private int coinToneCount = 3;
    private int coinToneMin = 3000;
    private int coinToneMax = 3600;

    // Array of tones to play (just to see it in the inspector. Code will change it regardless of what the inspector says)
    [SerializeField]
    private float[] tones;
    // Array of the lengths of the tones (also makes no difference with value changes in inspector)
    private float[] toneLengths;

    // List to hold the lists of samples for joining
    private List<List<float>> sampleSets;

    // Lists to hold separate sample values before combining them into one output
    private List<float> sineSamples;
    private List<float> squareSamples;

    // The final list of samplles to be compiled into an audio clip
     private List<float> outSamples;

    // Holds the component that plays the audio
    private AudioSource audioSource;

    private void Awake()
    {
        // Finds the AudioSource Component
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Gets a new tone and plays it
    /// </summary>
    public void PlayTone(AudioClip toPlay)
    {
        // Plays tone once
        audioSource.PlayOneShot(toPlay);
    }

    /// <summary>
    /// Generates random tones (Currently hardcoded as 2)
    /// Run when button is pressed
    /// </summary>
    public void CoinPickupTone()
    {
        totalLength = 0;
        tones = new float[coinToneCount];
        toneLengths = new float[coinToneCount];

        // Sets tone frequency values based on an inital random value
        tones[0] = Random.Range(coinToneMin, coinToneMax);
        tones[1] = tones[0] * .8f;
        tones[2] = tones[0] * .3f;

        // Sets tone length values
        toneLengths[0] = .2f;
        toneLengths[1] = .5f;
        toneLengths[2] = .4f;

        // Sums the total length of the audio clip
        foreach (float time in toneLengths)
        {
            totalLength += time;
        }

        // Empties lists used
        outSamples = new List<float>();
        sineSamples = new List<float>();
        squareSamples = new List<float>();

        for (int i = 0; i < tones.Length; i++)
        {
            CreateSineWave(tones[i], volume, toneLengths[i]);
            CreateSquareWave(tones[i], volume, toneLengths[i]);
        }


        sampleSets.Add(sineSamples);
        sampleSets.Add(squareSamples);

        JoinAudio(sampleSets);

        PlayTone(CreateToneAudioClip());
    }


    /// <summary>
    /// Creates an audioclip from the outSamples list
    /// </summary>
    /// <returns>AudioClip</returns>
    private AudioClip CreateToneAudioClip()
    {
        // Computes length of sample
        sampleLength = Mathf.CeilToInt(sampleRate * totalLength);

        // Creates an array with the same data as the sample list
        // Since AudioClip.SetData requires an array
        float[] tonesToPlay = new float[outSamples.Count];
        tonesToPlay = outSamples.ToArray();

        // Creates the AudioClip that will be returned
        AudioClip audioClip = AudioClip.Create("tone", sampleLength, 1, sampleRate, false);
        audioClip.SetData(tonesToPlay, 0);
        return audioClip;
    }

    /// <summary>
    /// Adds to the "sineSamples" list the sine wave of the frequencies input at the volume input
    /// </summary>
    /// <param name="frequency">Array of frequencies to create the sine wave from</param>
    /// <param name="volume">Multiplicative variable to change the amplitude of the wave and therefore the volume</param>
    /// <param name="noteLength">Total length of the wave</param>
    private void CreateSineWave(float frequency, float volume, float noteLength)
    {
        for (int j = 0; j < sampleRate * noteLength; j++)
        {
            // Finds the values of each point on the sine wave of each tone according to sample rate
            float pointOnWave = Mathf.Sin(2.0f * Mathf.PI * frequency * (j / noteLength));
            // Adjusts volume and adds it to the list of samples
            sineSamples.Add(pointOnWave * volume);
        }
    }

    /// <summary>
    /// Adds to the "squareSamples" list the square waveform of the frequencies input at the volume input
    /// </summary>
    /// <param name="frequency">Array of frequencies to create the square wave from</param>
    /// <param name="volume">Multiplicative variable to change the amplitude of the wave and therefore the volume</param>
    /// <param name="noteLength">Total length of the wave</param>
    private void CreateSquareWave(float frequency, float volume, float noteLength)
    {
        for (int j = 0; j < sampleRate * noteLength; j++)
        {
            // Finds the values of each point on the sine wave of each tone according to sample rate
            float pointOnWave = Mathf.Sign(Mathf.Sin(2.0f * Mathf.PI * frequency * (j / noteLength)));
            // Adjusts volume and adds it to the list of samples
            squareSamples.Add(pointOnWave * volume);
        }
    }

    /// <summary>
    /// Takes all lists in the toneLists parameter and sums them into one list
    /// </summary>
    /// <param name="toneLists">Lists of tones to sum</param>
    private void JoinAudio(List<List<float>> toneLists)
    {
        for (int i = 0; i < toneLists.Count; i++)
        {
            // Prevents setting values of a list that doesn't have a value
            if (i == 0)
            {
                for (int j = 0; j < toneLists[i].Count; j++)
                {
                    outSamples.Add(toneLists[i][j]);
                }
            }

            // Adds each individual value for the tones
            else if (i != 0)
            {
                for (int j = 0; j < toneLists[i].Count; j++)
                {
                    outSamples[i] += toneLists[i][j];
                }
            }
        }
    }
}
