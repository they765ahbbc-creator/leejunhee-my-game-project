using UnityEditor;
using UnityEngine;

namespace ShootemUp
{
    [CustomEditor(typeof(ScrollingBackground))]
    public class ScollingBackgroundEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ScrollingBackground scrollingBackground = (ScrollingBackground)target;

            if (GUILayout.Button("Preview"))
            {
                scrollingBackground.DestroyPreviewBackgroundsInEditor();
                scrollingBackground.CreatePreviewBackgrounds();
            }

            if(GUILayout.Button("Delete Preview"))
            {
                scrollingBackground.DestroyPreviewBackgroundsInEditor();
            }
        }
    }
}
