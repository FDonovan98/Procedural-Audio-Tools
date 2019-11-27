# COMP 120 - Tinkering Audio
[Link to main repository just in case](https://github.com/HDonovan96/Procedural-Audio-Tools)

## Contracts

### Contract - 1, Sound Effect Generation, James Berry

You are currently been tasked with creating a tool which will generate sound
effects, these should consist of sequences of tone as well as sample manipulations. The sound effect should be clearly tied to an action the player makes in the game (e.g., picking up an item, attacking, walking over a trap, or so on).
An element of procedurality is expected to vary the tones.

#### Usage
##### Build
Run the build then press the button
##### Unity Project
Open the project and then open the 'Contract 1' Scene. Run the project and then press the button

### Contract - 4, Ambient Platformers Audio Generation ,Harry Donovan

You need to develop a tool to generate audio for the user interface. Theseshould be short tones to feed back to the player that they are successfullynavigating the user interface and configuring the gameâ€™s settings. This shouldbe somewhat consistent across the interface,  but the tones and samplesshould be modified in a systematic way to indicate success or failure. A varietyof settings should be made available and configurable by a designer.

#### Instructions For Use
The tool can be accessed by selecting the ‘Window’ tab in the unity editor toolbar at the top of the screen and then selecting ‘Tone Editor’. This will open the ‘Tone Editor’ window.

#####Parameters
Sample Name – Sets the name of the audio clip. If the audio clip is saved this will be the filename.
Sample Frequency – The frequency of the audio clip. If ‘Inflection’ is enabled then this will be the starting frequency.
Inflection – When enabled the tone generated will start at the ‘Sample Frequency’ and transition to the ‘End Frequency’ over the length of the audio clip. This transition is not smooth but produces a ‘dial tone’ effect to the transition.
End Frequency – When ‘Inflection’ is enabled sets the frequency the audio clip shall end at. This can be either higher or lower than the ‘Sample Frequency’. When ‘Inflection’ is disabled this property is not displayed as it has no effect.
Sample Duration – The duration of the audio clip in seconds.
Sample Rate – This is the number of samples in the audio track per second. By default this is set to 44,100 and this can normally be left unchanged.
Audio Clip – Displays the last generated audio clip.

#####Buttons
Play Tone – Generates an audio clip from the current settings and then plays it. This does not save the generated audio clip. (Note: A blank game object is generated in order to play the sound. This object is automatically deleted when the window is closed.)
Save Tone – Generates an audio clip from the current settings and saves it in Assets/Resources/Sounds/ as SampleName.wav. This will overwrite an existing audio clip if it shares the same name.
Assign Current Tone To Selected Buttons – Generates a tone from the current settings and saves it (in the same way as the ‘Save Tone’ button). Any game object currently selected will be checked for a button component. Those that contain one will have an audio source component added (if one doesn’t already exist) and the generated clip will be stored here. A function will then be added to the buttons OnClick() parameter to play the audio clip stored in the audio source every time the button is pressed.

## License
The [MIT license](https://choosealicense.com/licenses/mit/) was chosen as it grants the free use of our work to anyone who wants it while still retaining our attribution to the work in any copies made in all uses including commercial. This means that our code can be taken and modified for any use while still acknowledging our contributions to the work.
