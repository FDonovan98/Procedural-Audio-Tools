using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class ToneEditor : EditorWindow
{
    int pos = 0;
    float sampleDuration = 1.5f;
    int samplerate = 44100;
    int sampleFrequency = 440;
    string sampleName;
    AudioClip audioClip;

    
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
        audioClip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", audioClip, typeof(AudioClip), true);

        if (GUILayout.Button("Play Tone"))
        {
            audioClip = GenerateTone();
            PlayAudio(audioClip);
        }

        if (GUILayout.Button("Save Tone"))
        {
            audioClip = GenerateTone();
            SaveTone(audioClip);
        }

        if (GUILayout.Button("Assign Current Tone To Selected Buttons"))
        {
            Debug.Log("This feature is not yet implemented");
        }
    }

    // private void AssignToneToButtons()
    // {
    //     SaveTone();
    //     foreach (GameObject obj in Selection.gameObjects)
    //     {
    //         Button button = obj.GetComponent<Button>();
    //         if (button != null)
    //         {
    //             button.onClick.AddListener(() => {})
    //         }
    //     }
    // }

    private void SaveTone(AudioClip audioClip)
    {  
        CreateFileStructure();
        AssetDatabase.CreateAsset(audioClip, "Assets/Sounds/" + sampleName);
    }

    private void CreateFileStructure()
    {
        if (!Directory.Exists("Assets/Sounds"))
        {
            AssetDatabase.CreateFolder("Assets", "Sounds");
        }
        
    }

    private void PlayAudio(AudioClip audioClip)
    {
        GameObject audioHolder = new GameObject();
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
