using UnityEngine;
using UnityEditor;

namespace ShootemUp
{
    [CustomEditor(typeof(LevelCamera))]
    public class LevelCameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelCamera cam = (LevelCamera)target;

            if(GUILayout.Button("Initialize camera"))
            {
                cam.Initalize();
            }
        }
    }
}
