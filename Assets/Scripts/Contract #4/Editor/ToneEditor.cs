using UnityEngine;
using UnityEditor;

public class ToneEditor : EditorWindow
{
    [MenuItem("Window/Tone Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToneEditor>();
    }
}
