using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Serialization;

namespace CameraShake 
{
    public class CameraShaker : MonoBehaviour
    {
        public static CameraShaker Instance;
        public static CameraShakePresets Presets;
        public static bool IsShaking;
        
        [SerializeField]
        private CameraStateMachine stateMachine;
        
        readonly List<ICameraShake> _activeShakes = new List<ICameraShake>();
        
        [SerializeField]
        Transform cameraTransform;
        
        [Range(0,1)]
        [SerializeField]
        public float strengthMultiplier = 1;

        public CameraShakePresets ShakePresets;
        
        public static void Shake(ICameraShake shake)
        {
            if (IsInstanceNull()) { return; }
            Instance.RegisterShake(shake);
        }
        
        public void RegisterShake(ICameraShake shake)
        {
            shake.Initialize(cameraTransform.position, cameraTransform.rotation);
            _activeShakes.Add(shake);
        }
        
        public void SetCameraTransform(Transform transform)
        {
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localEulerAngles = Vector3.zero;
            this.cameraTransform = cameraTransform;
        }

        private void Awake()
        {
            ShakePresets = new CameraShakePresets(this);
            Presets = ShakePresets;
        }

        private void Start()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple CameraShaker instances in scene. Destroying duplicate.");
                Destroy(this);
                return;
            }
            else
            {
                Instance = this;    
            }
            
            IsShaking = false;
            if (cameraTransform == null)
                cameraTransform = transform;
        }

        private void Update()
        {
            if (cameraTransform == null)
            {
                return;
            }
            
            Displacement cameraDisplacement = Displacement.Zero;
            
            for (int i = _activeShakes.Count - 1; i >= 0; i--)
            {
                if (_activeShakes[i].IsFinished)
                {
                    _activeShakes.RemoveAt(i);
                    stateMachine.SwitchState(new CameraFreelookState(stateMachine));
                }
                else
                {
                    stateMachine.SwitchState(new CameraShakeState(stateMachine));
                    _activeShakes[i].Update(Time.deltaTime, cameraTransform.position, cameraTransform.rotation);
                    cameraDisplacement += _activeShakes[i].CurrentDisplacement;
                }
            }
            cameraTransform.localPosition = cameraDisplacement.Position * strengthMultiplier;
            cameraTransform.localRotation = Quaternion.Euler(cameraDisplacement.EulerAngles * strengthMultiplier);
        }

        private static bool IsInstanceNull()
        {
            if (Instance == null)
            {
                Debug.LogError("CameraShaker instance is null. Make sure you have a CameraShaker in your scene.");
                return true;
            }

            return false;
        }
    }
}
