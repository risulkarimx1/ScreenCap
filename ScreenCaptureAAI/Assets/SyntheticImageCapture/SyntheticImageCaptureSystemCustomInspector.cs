using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AAI.VDTSimulator.SytheticImageCapture;
using UnityEditor;
using UnityEngine;

namespace AAI.VDTSimulator.SytheticImageCapture
{
    /// <summary>
    /// Custom Editor for Image Capture Point Generator
    /// Custom Inspector of: <see cref="SyntheticImageCaptureSystem"/>
    /// </summary>
    [CustomEditor(typeof(SyntheticImageCaptureSystem))]
    public class SyntheticImageCaptureSystemCustomInspector : Editor
    {
        private SyntheticImageCaptureSystem syntheticImageTarget;
        private void OnEnable()
        {
            syntheticImageTarget = (SyntheticImageCaptureSystem)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (GUILayout.Button("Take Snap"))
            {
                
            }
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}


