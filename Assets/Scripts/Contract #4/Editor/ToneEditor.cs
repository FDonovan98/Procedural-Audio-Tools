﻿/*
Author: Harry Donovan
Link To Repository: https://github.com/HDonovan96/Procedural-Audio-Tools
License: MIT
*/

using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class ToneEditor : EditorWindow
{
    int pos = 0;
    float sampleDuration = 1.0f;
    int samplerate = 44100;
    int sampleFrequency = 440;
    int endFrequency;
    string sampleName = "Default";
    AudioClip audioClip;
    bool inflection = false;
    int readCounter;
    GameObject audioHolder = null;
    AudioSource audioSource;

    
    [MenuItem("Window/Tone Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToneEditor>("Tone Editor");
    }

    void OnGUI()
    {
        sampleName = EditorGUILayout.TextField("Sample Name", sampleName);
        sampleFrequency = EditorGUILayout.IntField("Sample Frequency", sampleFrequency);
        inflection = EditorGUILayout.Toggle("Inflection", inflection);
        if (inflection)
        {
            endFrequency = EditorGUILayout.IntField("End Frequency", endFrequency);
        }
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

    void OnDestroy()
    {
        DestroyImmediate(audioHolder);
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

    // Creates the file structure then saves the current audio sample.
    private void SaveTone(AudioClip audioClip)
    {  
        CreateFileStructure();
        SaveWavUtil.Save(sampleName, audioClip);
    }

    // Creates a folder to save any audio clips in.
    private void CreateFileStructure()
    {
        // If the folder 'Assets/Sound' doesn't already exist it is created.
        if (!Directory.Exists("Assets/Sounds"))
        {
            AssetDatabase.CreateFolder("Assets", "Sounds");
        }
        
    }

    private void PlayAudio(AudioClip audioClip)
    {
        if (audioHolder == null)
        {
            audioHolder = new GameObject();
            audioSource = audioHolder.AddComponent<AudioSource>();
        }
         
        audioSource.clip = audioClip;
        audioSource.Play();        
    }

    private AudioClip GenerateTone()
    {
        readCounter = 0;
        AudioClip audioClip = AudioClip.Create(sampleName, (int)(samplerate * sampleDuration), 1, samplerate, false, OnAudioRead, OnAudioSetPosition);

        return audioClip;
    }

    void OnAudioRead(float[] data)
    {
        int count = 0;
        float currentFreq;
        readCounter++;
        while (count < data.Length)
        {
            if (inflection)
            {
                currentFreq = Mathf.Lerp(sampleFrequency, endFrequency, (float)readCounter / (1 + 10 * sampleDuration)); 
            }
            else
            {
                currentFreq = sampleFrequency;
            }
            
            data[count] = Mathf.Sin(2 * Mathf.PI * currentFreq * pos / samplerate);
            pos++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        pos = newPosition;
    }
}
