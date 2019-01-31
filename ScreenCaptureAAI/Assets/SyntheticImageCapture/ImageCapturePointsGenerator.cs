using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AAI.VDTSimulator.SytheticImageCapture
{

    /// <summary>
    /// Ressponsible for Generating Image Capture Positions either from Hierarchy and OSI Data
    /// Custom Inspector: <see cref="ImageCapturePointsGeneratorCustomInspector"/>
    /// </summary>
    public class ImageCapturePointsGenerator : MonoBehaviour
    {   //TODO: Will be replaced from json file system
        #region ObjectHolder : Exposed Parameters
        [Header("Selection Factor (Every Xth Object will be selected")]
        [SerializeField]
        private int selectionFactor = 5;
        [SerializeField] private Transform traversalObjectParent;
        #endregion

        [SerializeField]
        private List<SimpleTransform> traversalPoints;

        public int Count { get; private set; }

       

        public void GeneratePoints()// Get called from Editor to generate the array
        {
            traversalPoints.Clear();
            int count = 0;
            int selectionCount = 0;
            foreach (Transform points in traversalObjectParent)
            {
                if (points.GetComponent<BoxCollider>() != null)
                {
                    if (count % selectionFactor == 0)
                    {
                        traversalPoints.Add(new SimpleTransform()
                        {
                            position = points.position,
                            rotation = points.rotation
                        });
                        selectionCount++;
                    }
                    count++;
                }
            }

            Count = selectionCount;
        }

        public void ClearPoints()// Called from Custom Editor
        {
            traversalPoints.Clear();
        }

        public SimpleTransform[] ToArray()
        {
            return traversalPoints.ToArray();
        }
    }
}


