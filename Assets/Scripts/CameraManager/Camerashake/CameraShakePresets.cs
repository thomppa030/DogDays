using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraShake 
{
    public class CameraShakePresets : MonoBehaviour
    {
        readonly CameraShaker _shaker;
        
        public CameraShakePresets(CameraShaker shaker)
        {
            _shaker = shaker;
        }

        public void ShortShake2D(
            float positionStrength = 0.08f,
            float rotationStrength = 0.1f,
            float freq = 25,
            int numBounces = 5)
        {   
            BounceShake.Params shakeParams = new BounceShake.Params
            {
                PositionStrength = positionStrength,
                RotationStrength = rotationStrength,
                Frequency = freq,
                NumBounces = numBounces
            };
        }
        
        public void ShortShake3D(float strength = 0.3f, float freq = 25, int numBounces = 5)
        {
            BounceShake.Params shakeParams = new BounceShake.Params
            {
                AxesMultiplier = new Displacement(Vector3.zero, new Vector3(1,1,0.4f)),
                RotationStrength = strength,
                Frequency = freq,
                NumBounces = numBounces
            };
            _shaker.RegisterShake(new BounceShake(shakeParams));
        }

        public void Explosion2D(
            float positionStrength = 1f,
            float rotationStrength = 3f,
            float duration = 0.5f)
        {
            PerlinShake.NoiseMode[] modes = new[]
            {
                new PerlinShake.NoiseMode(8, 1),
                new PerlinShake.NoiseMode(20, 0.3f),
            };

            Envelope.EnvelopeParams envelopeParams = new Envelope.EnvelopeParams();
        }
    }
} 
