/*
Author: Harry Donovan
Link To Repository: https://github.com/HDonovan96/Procedural-Audio-Tools
License: MIT
*/

using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using UnityEditor.Events;
using UnityEngine.Events;

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

    // Adds the 'Tone Editor' option into the 'Window' toolbar in the unity editor.
    [MenuItem("Window/Tone Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToneEditor>("Tone Editor");
    }

    void OnGUI()
    {
        // Sets what editable fields are displayed in the window.
        sampleName = EditorGUILayout.TextField("Sample Name", sampleName);
        sampleFrequency = EditorGUILayout.IntField("Sample Frequency", sampleFrequency);
        inflection = EditorGUILayout.Toggle("Inflection", inflection);
        // Only displays the 'End Frequency' field if 'Inflection' is set to true.
        // This is for clarity as 'End Frequency' does nothing when 'Inflection' is false.
        if (inflection)
        {
            endFrequency = EditorGUILayout.IntField("End Frequency", endFrequency);
        }
        // Sample duration is the time the tone lasts in seconds.
        sampleDuration = EditorGUILayout.FloatField("Sample Duration", sampleDuration);
        samplerate = EditorGUILayout.IntField("Sample Rate", samplerate);
        audioClip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", audioClip, typeof(AudioClip), true);

        // Creates a 'Play Tone' button.
        // When pressed generates a tone from the current settings and then plays it out loud.
        if (GUILayout.Button("Play Tone"))
        {
            GenerateTone();
            PlayAudio();
        }

        // Creates a 'Save Tone' button.
        // When pressed generates a tone from the current settings and then saves it.
        // Tone is saved in 'Assets/Resources/Sounds' and 'sampleName is used as the file name.
        if (GUILayout.Button("Save Tone"))
        {
            GenerateTone();
            SaveTone();
        }

        // Creates a 'Assign Current Tone To Selected Buttons' button.
        // When pressed generates a tone from the current settings, then adds an OnClick() event to play the tone to any selected button.
        // If the button doesn't already contain an 'AudioSource' component then one is added.
        if (GUILayout.Button("Assign Current Tone To Selected Buttons"))
        {
            GenerateTone();
            AssignToneToButtons();
        }
    }

    // When the editor window is closed the gameobject used to allow the editor to play sounds is also destroyed.
    void OnDestroy()
    {
        DestroyImmediate(audioHolder);
    }

    // Saves the current tone then gets every gameobject currently selected that has a 'Button' component.
    // If the object already has an 'AudioSource' component then this is fetched, otherwise one is added.
    // The tone that was saved at the beginning of the function is then loaded into the AudioSource clip.
    // A persistent event is then added to the button to play the AudioSource clip on click.
    private void AssignToneToButtons()
    {
        // Saves a tone using the current settings.
        SaveTone();
        // Gets every object currently selected.
        foreach (GameObject obj in Selection.gameObjects)
        {
            Button button = obj.GetComponent<Button>();
            // Ignores any objects that don't have a 'Button' component.
            if (button != null)
            {
                // Gets the 'AudioSource' component from the current gameobject.
                AudioSource objAudioSource = obj.GetComponent<AudioSource>();
                // If no 'AudioSource' component exists one is added.
                // 'PlayOnAwake' is set to false so the sound is only heard when the button is pressed.
                if (obj.GetComponent<AudioSource>() == null)
                {
                    objAudioSource = obj.AddComponent<AudioSource>();
                    objAudioSource.playOnAwake = false;
                }
                // Loads the saved audio clip into the 'AudioSource' clip parameter.
                objAudioSource.clip = Resources.Load<AudioClip>("Sounds/" + sampleName);
                // Adds a presistent listener onto the button to play the clip stored in the 'AudioSource' component.
                UnityEventTools.AddPersistentListener(button.onClick, objAudioSource.Play);

            }
        }
    }

    // Creates the file path then saves the current audio sample.
    private void SaveTone()
    {  
        CreateFileStructure();
        SaveWavUtil.Save(sampleName, audioClip);
    }

    // Creates a folder to save any audio clips in.
    private void CreateFileStructure()
    {
        // If the folder 'Assets/Resources' doesn't already exist it is created.
        if (!Directory.Exists("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        // If the folder 'Assets/Resources/Sounds' doesn't already exist it is created.
        if (!Directory.Exists("Assets/Resources/Sounds"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Sounds");
        }
    }

    // Creates an empty game object to allow the editor to play sounds.
    private void PlayAudio()
    {
        // If the required gameobject doesn't exist already it is created.
        if (audioHolder == null)
        {
            audioHolder = new GameObject();
            // An 'AudioSource' component is needed to play audio so this is added to the empty gameobject.
            audioSource = audioHolder.AddComponent<AudioSource>();
        }
        // The AudioSource clip parameter is set to the last created audioClip and then played.
        audioSource.clip = audioClip;
        audioSource.Play();        
    }

    // Creates an audio clip sampleDuration seconds in length.
    // The generated clips frequency starts at sampleFrequency and ends at endFrequency.
    private void GenerateTone()
    {
        // readCounter is used when 'inflection' is set to true.
        // It is needed for Mathf.Lerp to track how many times 'OnAudioRead' function has been run.
        readCounter = 0;
        audioClip = AudioClip.Create(sampleName, (int)(samplerate * sampleDuration), 1, samplerate, false, OnAudioRead, OnAudioSetPosition);
    }

    // Function is called in 'AudioClip.Create' in 'GenerateTone' function.
    // Generates sound wave of the audio clip.
    void OnAudioRead(float[] data)
    {
        int count = 0;
        float currentFreq;
        readCounter++;
        while (count < data.Length)
        {
            // If inflection is set to true then the frequency is linearly interpolated between sampleFrequency and endFrequency.
            // Otherwise, the frequency is just sampleFrequency.
            if (inflection)
            {
                // The calculation for t is rough and loses accuracy as sampleDuration increases.
                // It is accurate up to around two seconds, and is still accurate enough for longer.
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
    
    // Function is called in 'AudioClip.Create' in 'GenerateTone' function.
    void OnAudioSetPosition(int newPosition)
    {
        pos = newPosition;
    }
}
