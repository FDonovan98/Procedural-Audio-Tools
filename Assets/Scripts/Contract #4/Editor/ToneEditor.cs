using UnityEngine;
using UnityEditor;

public class ToneEditor : EditorWindow
{
    private int pos = 0;
    public float sampleDuration = 1.5f;
    public int samplerate = 44100;
    public int frequency = 440;
    public string sampleName;
    
    [MenuItem("Window/Tone Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToneEditor>();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Press me"))
        {
            PlayAudio();
        }
    }

    private void PlayAudio()
    {
        AudioClip audioclip = GenerateTone();
        GameObject audioHolder = Instantiate(new GameObject());
        AudioSource audioSource = audioHolder.AddComponent<AudioSource>();
        audioSource.clip = audioclip;
        audioSource.Play();
    }

    private AudioClip GenerateTone()
    {
        AudioClip audioClip = AudioClip.Create(sampleName, (int)(samplerate * sampleDuration), 1, samplerate, true, OnAudioRead, OnAudioSetPosition);

        return audioClip;
    }

    void OnAudioRead(float[] data)
    {
        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sin(2 * Mathf.PI * frequency * pos / samplerate);
            pos++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        pos = newPosition;
    }
}
