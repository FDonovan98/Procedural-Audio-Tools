using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract1 : MonoBehaviour
{
    [SerializeField]
    private int frequency;
    [SerializeField]
    private int sampleDurationSecs;
    [SerializeField]
    private int sampleRate;

    private AudioSource audioSource;
    private AudioClip outAudioClip;

    public void PlayTone()
    {
        audioSource = GetComponent<AudioSource>();
        outAudioClip = CreateToneAudioClip();
        audioSource.PlayOneShot(outAudioClip);
    }

    private AudioClip CreateToneAudioClip()
    {
        int sampleLength = sampleRate * sampleDurationSecs;
        float maxValue = 1f / 4f;

        var audioClip = AudioClip.Create("tone", sampleLength, 1, sampleRate, false);

        float[] samples = new float[sampleLength];
        for (var i = 0; i < sampleLength; i++)
        {
            float s = Mathf.Sin(2.0f * Mathf.PI * frequency * ((float)i / (float)sampleRate));
            float v = s * maxValue;
            samples[i] = v;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }
}
