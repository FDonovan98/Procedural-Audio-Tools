using UnityEngine;

public class ToneGenerator : MonoBehaviour
{
    private int position;
    public float sampleDuration = 1.5f;
    public int samplerate = 44100;
    public int frequency = 440;
    public string sampleName;

    public void OnButtonPress()
    {
        AudioClip myClip = AudioClip.Create(sampleName, (int)(samplerate * sampleDuration), 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        AudioSource aud = GetComponent<AudioSource>();
        aud.clip = myClip;
        aud.Play();
    }

    void OnAudioRead(float[] data)
    {
        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate);
            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
}
