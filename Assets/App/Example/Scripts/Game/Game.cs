using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace FrameworkDesign.Example
{

    public class Game : MonoBehaviour
    {
        [Header("AR Foundation")]

        /// <summary>
        /// The active ARSessionOrigin used in the example.
        /// </summary>
        public ARSessionOrigin SessionOrigin;

        /// <summary>
        /// The ARSession used in the example.
        /// </summary>
        public ARSession SessionCore;

        /// <summary>
        /// The ARCoreExtensions used in the example.
        /// </summary>
        public ARCoreExtensions Extensions;

        /// <summary>
        /// The active ARAnchorManager used in the example.
        /// </summary>
        public ARAnchorManager AnchorManager;

        /// <summary>
        /// The active ARPlaneManager used in the example.
        /// </summary>
        public ARPlaneManager PlaneManager;

        /// <summary>
        /// The active ARRaycastManager used in the example.
        /// </summary>
        public ARRaycastManager RaycastManager;

        [Header("UI")]

        /// <summary>
        /// The home page to choose entering hosting or resolving work flow.
        /// </summary>
        public GameObject HomePage;
        /// <summary>
        /// The AR screen which displays the AR view, hosts or resolves cloud anchors,
        /// and returns to home page.
        /// </summary>
        public GameObject ARView;

        [HideInInspector]
        /// <summary>
        /// The key name used in PlayerPrefs which indicates whether the start info has displayed
        /// at least one time.
        /// </summary>
        private const string _hasDisplayedStartInfoKey = "HasDisplayedStartInfo";
        public void Awake()
        {
            //Awake是在脚本实例化的时候运行的，且在整个脚本生命周期只运行一次。而Start是在物体第一次enable的时候运行的，且在整个脚本生命周期只运行一次。
            // Lock screen to portrait.
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.orientation = ScreenOrientation.Portrait;

            // Enable Persistent Cloud Anchors sample to target 60fps camera capture frame rate
            // on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
            SwitchToHomePage();
        }
        public void SwitchToHomePage()
        {
            if (PlayerPrefs.HasKey(_hasDisplayedStartInfoKey))
            {
               // SwitchToARView();
               // return;
            }
            ResetAllViews();
            HomePage.SetActive(true);
        }
        /// <summary>
        /// Switch to AR view, and disable all other screens.
        /// </summary>
        public void SwitchToARView()
        {
            ResetAllViews();
            PlayerPrefs.SetInt(_hasDisplayedStartInfoKey, 1);
            ARView.SetActive(true);
            SetPlatformActive(true);
        }
        private void ResetAllViews()
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            SetPlatformActive(false);
            ARView.SetActive(false);
            HomePage.SetActive(false);
        }
        private void SetPlatformActive(bool active)
        {
            SessionOrigin.gameObject.SetActive(active);
            SessionCore.gameObject.SetActive(active);
            Extensions.gameObject.SetActive(active);
        }
        // Start is called before the first frame update
        //----事件注册和触发
        void Start()
        {

            GameStartEvent.Register(onGameStart);
            GameCloseEvent.Register(onGameClose);
            Logger.Log("Game-Start");
        }

        private void onGameStart()
        {
            Logger.Log("Game-onGameStart");
            SwitchToARView();
            //transform.Find("TriAxesWithDebugText").gameObject.SetActive(true);
        }
        private void onGameClose()
        {
            Logger.Log("Game-onGameClose");
            SwitchToHomePage();
            
        }

        private void OnDestroy()
        {
            GameStartEvent.UnRegister(onGameStart);
        }
        //----事件注册和触发
    }
}
