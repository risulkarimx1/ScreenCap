using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using Zenject;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using UniRx.Async;
using UnityEngine.Serialization;

namespace AAI.VDTSimulator.SytheticImageCapture
{
    [Serializable]
    public struct SimpleTransform
    {
        public Vector3 position;// { get; set; }
        public Quaternion rotation;// { get; set; }
    }

    public class SyntheticImageCaptureSystem : MonoBehaviour
    {
        #region CameraComponents
        // TODO: 
        private Camera _mainCamera;
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Camera _captureCamera;
        private bool isCapturing;
        #endregion
        private SimpleTransform[] capturePoints;

        [Header("Supeer Size: 1=1080p, 2= 4k, 3...")]
        [SerializeField]
        private int superSize = 5;
        
        private int _iterationCount = 0;


        [Header("Camera Offset")]
        [SerializeField] private Vector3 _offset;

        private ImageCapturePointsGenerator imageCapturePointsGenerator;
        [SerializeField]
        [Range(0,1)]
        private float _completionFactor;

        void Awake()

        {

            _mainCamera = Camera.main;
            isCapturing = false;
            _captureCamera.enabled = false;
            imageCapturePointsGenerator = GetComponent<ImageCapturePointsGenerator>();
        }


        public string RootFilePath;
        void Start()
        {

            //filePathName = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/AAI_VDTSIM_Images/Images";
            RootFilePath = Path.Combine(Application.persistentDataPath, "Images");

            if (!Directory.Exists(RootFilePath))
            {
                Debug.LogWarning($"The does exists");
                DirectoryInfo f = Directory.CreateDirectory(RootFilePath);
                Debug.Log($"path is {RootFilePath}");
            }
            else
            {
                Debug.LogWarning($"The folder exists");
                Debug.Log($"path is {RootFilePath}");
            }

            capturePoints = imageCapturePointsGenerator.ToArray();

            _iterationCount = 0;

            var keyboardStream = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.C));

            keyboardStream.Subscribe(_ =>
            {
                _iterationCount = 0;
                Debug.Log($"Capture Pressed");
                string temporaryFolderName = $"{DateTime.Now.Date.Day}" +
                                             $".{DateTime.Now.Date.Month}." +
                                             $"{DateTime.Now.Date.Year}_" +
                                             $"{DateTime.Now.Date.Hour}." +
                                             $"{DateTime.Now.Date.Minute}." +
                                             $"{DateTime.Now.Date.Second}";

                string thisOperationDirectory = Path.Combine(RootFilePath, temporaryFolderName);
                if (!Directory.Exists(thisOperationDirectory))
                {
                    Directory.CreateDirectory(thisOperationDirectory);
                }


                isCapturing = !isCapturing;
                _captureCamera.enabled = isCapturing;
                _mainCamera.enabled = !isCapturing;



                //if (isCapturing)
                //    InvokeRepeating("CameraPlacementInvoke", 0, .25f);
                //else
                //    CancelInvoke("CameraPlacementInvoke");
                IObservable<bool> canSaveImage;
                if (isCapturing)
                {
                    canSaveImage = SaveImage(thisOperationDirectory);
                    canSaveImage.Subscribe((isSaved) => { Debug.Log($"save {isSaved}"); });
                    Debug.Log($"done saving");
                }
                else
                {
                    
                }

                
            });

        }

        public IObservable<bool> SaveImage(string filePath)
        {
            return Observable.FromCoroutine<bool>(
                (observer, cancellationToken) =>
                    CameraPlacementCR(filePath, observer, cancellationToken)
                );
        }

        //
        public IEnumerator CameraPlacementCR(string filePath,IObserver<bool> observer, CancellationToken cancellationToken)
        {
            while (_iterationCount < capturePoints.Length)
            {
                Vector3 targetPosition = capturePoints[_iterationCount].position;
                targetPosition += _offset;
                _cameraHolder.transform.position = targetPosition;
                _cameraHolder.transform.rotation = capturePoints[_iterationCount].rotation;

                _completionFactor = (float)(_iterationCount / capturePoints.Length);
                ScreenCapture.CaptureScreenshot($"{filePath}/{_iterationCount}.jpg", superSize);
                Debug.Log($"Save at {filePath}");
                if (!cancellationToken.IsCancellationRequested)
                {
                    yield return null;
                }

                _iterationCount++;
            }
            
            observer.OnNext(true);
            observer.OnCompleted();
            
        }

        //        void CameraPlacementInvoke()
        //        {
        //            Vector3 targetPosition = capturePoints[_iterationCount].position;
        //            targetPosition += _offset;
        //            _cameraHolder.transform.position = targetPosition;
        //            _cameraHolder.transform.rotation = capturePoints[_iterationCount].rotation;
        //            
        //            _completionFactor = (float)(_iterationCount / capturePoints.Length);
        //            ScreenCapture.CaptureScreenshot($"a/Images/{_iterationCount++}.jpg", superSize);
        //        }
    }
}


