using UnityEngine;
using UnityEditor;

public class ToneEditor : EditorWindow
{
    int pos = 0;
    float sampleDuration = 1.5f;
    int samplerate = 44100;
    int sampleFrequency = 440;
    string sampleName;
    
    [MenuItem("Window/Tone Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToneEditor>("Tone Editor");
    }

    void OnGUI()
    {
        sampleName = EditorGUILayout.TextField("Sample Name", sampleName);
        sampleFrequency = EditorGUILayout.IntField("Sample Frequency", sampleFrequency);
        sampleDuration = EditorGUILayout.FloatField("Sample Duration", sampleDuration);
        samplerate = EditorGUILayout.IntField("Sample Rate", samplerate);

        if (GUILayout.Button("Play Tone"))
        {
            PlayAudio();
        }

        if (GUILayout.Button("Save Tone"))
        {
            Debug.Log("This feature is not yet implemented");
        }
    }

    private void PlayAudio()
    {
        AudioClip audioClip = GenerateTone();
        GameObject audioHolder = Instantiate(new GameObject());
        AudioSource audioSource = audioHolder.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
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
            data[count] = Mathf.Sin(2 * Mathf.PI * sampleFrequency * pos / samplerate);
            pos++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        pos = newPosition;
    }
}
