using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AAI.VDTSimulator.SytheticImageCapture
{
    /// <summary>
    /// Custom Editor for Image Capture Point Generator
    /// Custom Inspector of: <see cref="ImageCapturePointsGenerator"/>
    /// </summary>
    [CustomEditor(typeof(ImageCapturePointsGenerator))]
    public class ImageCapturePointsGeneratorCustomInspector : Editor
    {
        private ImageCapturePointsGenerator _pointGeneratorTarget;
        private SerializedProperty traversalObjectParent; 

        private void OnEnable()
        {
            _pointGeneratorTarget = (ImageCapturePointsGenerator)target;
            traversalObjectParent = serializedObject.FindProperty("traversalObjectParent");
            
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            if (GUILayout.Button("Generate List"))
            {
                _pointGeneratorTarget.GeneratePoints();
            }
            if (GUILayout.Button("Clear List"))
            {
                if (traversalObjectParent == null)
                {
                    Debug.Log($" its null");
                }
                else
                {
                    Debug.Log($"there is something here");
                    Debug.Log(traversalObjectParent.hasChildren);
                }
                _pointGeneratorTarget.ClearPoints();
            }
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}


