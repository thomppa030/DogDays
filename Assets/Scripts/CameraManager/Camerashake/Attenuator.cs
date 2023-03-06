using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CameraShake 
{
    public static class Attenuator
    {
        public static float Strength(StrengthAttenuationParams param, Vector3 sourcePosition, Vector3 cameraPosition)
        {
            Vector3 vec = cameraPosition - sourcePosition;
            float distance = vec.magnitude;
            float strength = Mathf.Clamp01(1 - distance / param.clippingDistance) / param.fallOffScale;
            
            return Power.Evaluate(strength, param.fallOffDegree);
        }

        public static Displacement Direction(Vector3 sourcePosition, Vector3 cameraPosition, Quaternion cameraRotation)
        {
            Displacement direction = Displacement.Zero;
            direction.Position = (cameraPosition - sourcePosition).normalized;
            direction.Position = Quaternion.Inverse(cameraRotation) * direction.Position;

            direction.EulerAngles.x = direction.Position.z;
            direction.EulerAngles.y = direction.Position.x;
            direction.EulerAngles.z = -direction.Position.x;

            return direction;
        }

        [System.Serializable]
        public class StrengthAttenuationParams
        {
            public float clippingDistance = 10;

            public float fallOffScale = 50;

            public Degree fallOffDegree = Degree.Quadratic;

            public Vector3 axesMultiplier = Vector3.one;
        }
    }
}
