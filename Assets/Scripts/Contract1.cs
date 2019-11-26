using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract1 : MonoBehaviour
{
    [SerializeField]
    private float sampleDurationSecs;
    [SerializeField]
    private int sampleRate;
    private float totalTime;

    [SerializeField]
    private float[] tones;

    private AudioSource audioSource;
    private AudioClip outAudioClip;

    public void PlayTone()
    {
        audioSource = GetComponent<AudioSource>();
        outAudioClip = CreateToneAudioClip(tones);
        audioSource.PlayOneShot(outAudioClip);
    }

    public void CoinPickupTone()
    {
        tones = new float[2];
        tones[0] = Random.Range(1000, 1200);
        tones[1] = tones[0] * 3f;        
        PlayTone();
    }

    private AudioClip CreateToneAudioClip(float[] frequency)
    {
        totalTime = 0;
        int sampleLength = Mathf.RoundToInt(sampleRate * sampleDurationSecs * frequency.Length);
        float maxValue = 1f / 4f;

        var audioClip = AudioClip.Create("tone", sampleLength, 1, sampleRate, false);

        List<float> samples = new List<float>();

        for (int i = 0; i < frequency.Length; i++)
        {
            for (var j = 0; j < sampleRate; j++)
            {
                float s = Mathf.Sin(2.0f * Mathf.PI * frequency[i] * ((float)j / (float)sampleRate));
                float v = s * maxValue;
                samples.Add(v);
            }
        }

        float[] tonesToPlay = new float[samples.Count];

        for (int s = 0; s < tonesToPlay.Length; s++)
        {
            tonesToPlay[s] = samples[s];
        }
        
        audioClip.SetData(tonesToPlay, 0);
        return audioClip;
    }
}
