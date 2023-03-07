using System.Collections;
using System.Collections.Generic;
using CameraShake;
using UnityEngine;

namespace CameraShake 
{
    public class BounceShake : ICameraShake
    {
        readonly Params _params;
        readonly AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        readonly Vector3? _sourcePosition = null;
        
        float attenuation = 1;
        Displacement direction;
        Displacement previousWaypoint;
        Displacement currentWaypoint;
        private int bounceIndex;
        private float time;

        public BounceShake(Params parameters, Vector3? sourcePosition = null)
        {
            this._sourcePosition = sourcePosition;
            this._params = parameters;
            Displacement rnd = Displacement.InsideUnitSphere();
            direction = Displacement.Scale(rnd, _params.AxesMultiplier).Normalized;
        }

        public BounceShake(Params parameters, Displacement initialDirection, Vector3? sourcePosition = null)
        {
            this._sourcePosition = sourcePosition;
            this._params = parameters;
            direction = Displacement.Scale(initialDirection, _params.AxesMultiplier).Normalized;
        }
        
        public Displacement CurrentDisplacement { get; private set; }
        public bool IsFinished { get; private set; }
        
        public void Initialize(Vector3 cameraPosition, Quaternion cameraRotation)
        {
            attenuation = _sourcePosition == null
                ? 1
                : Attenuator.Strength(_params.attenuation, _sourcePosition.Value, cameraPosition);
            currentWaypoint = attenuation * direction.ScaleBy(_params.PositionStrength, _params.RotationStrength);
        }

        public void Update(float deltaTime, Vector3 cameraPosition, Quaternion cameraRotation)
        {
            if (time < 1)
            {
                time += deltaTime * _params.Frequency;
                if (_params.Frequency == 0) time = 1;

                CurrentDisplacement = Displacement.Lerp(previousWaypoint, currentWaypoint,
                    moveCurve.Evaluate(time));
            }
            else
            {
                time = 0;
                CurrentDisplacement = currentWaypoint;
                previousWaypoint = currentWaypoint;
                bounceIndex++;
                if (bounceIndex > _params.NumBounces)
                {
                    IsFinished = true;
                    return;
                }

                Displacement rnd = Displacement.InsideUnitSphere();
                direction = -direction
                            + _params.randomness * Displacement.Scale(rnd, _params.AxesMultiplier).Normalized;
                direction = direction.Normalized;
                float decayValue = 1 - (float)bounceIndex / _params.NumBounces;
                currentWaypoint = decayValue * decayValue * attenuation
                                  * direction.ScaleBy(_params.PositionStrength, _params.RotationStrength);
            }
        }
        
    [System.Serializable]
    public class Params
    {
        public float PositionStrength = 0.05f;
        
        public float RotationStrength = 0.1f;
        
        public Displacement AxesMultiplier = new Displacement(Vector3.one, Vector3.forward);
        
        public float Frequency = 25;
        
        public int NumBounces = 5;
        
        [Range(0,1)]
        public float randomness = 0.5f;

        public Attenuator.StrengthAttenuationParams attenuation;
    }
    }
    

}
