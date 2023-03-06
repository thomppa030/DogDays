using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraShake 
{
    public struct Displacement
    {
        public Vector3 Position;
        public Vector3 EulerAngles;
        
        public Displacement(Vector3 position, Vector3 eulerAngles)
        {
            this.Position = position;
            this.EulerAngles = eulerAngles;
        }
        
        public Displacement(Vector3 position)
        {
            Position = position;
            EulerAngles = Vector3.zero;
        }

        public static Displacement Zero
        {
            get
            {
                return new Displacement(Vector3.zero, Vector3.zero);
            }
        }
        
        public static Displacement operator +(Displacement a, Displacement b)
        {
            return new Displacement(a.Position + b.Position, a.EulerAngles + b.EulerAngles);
        }
        
        public static Displacement operator -(Displacement a, Displacement b)
        {
            return new Displacement(a.Position - b.Position, a.EulerAngles - b.EulerAngles);
        }
        
        public static Displacement operator -(Displacement a)
        {
            return new Displacement(-a.Position, -a.EulerAngles);
        }
        
        public static Displacement operator *(Displacement a, float b)
        {
            return new Displacement(a.Position * b, a.EulerAngles * b);
        }
        
        public static Displacement operator *(float a, Displacement b)
        {
            return b * a;
        }
        
        public static Displacement operator /(Displacement a, float b)
        {
            return new Displacement(a.Position / b, a.EulerAngles / b);
        }
        
        public static Displacement Scale(Displacement a, Displacement b)
        {
            return new Displacement(Vector3.Scale(a.Position, b.Position), Vector3.Scale(a.EulerAngles, b.EulerAngles));
        }
        
        public static Displacement Lerp(Displacement a, Displacement b, float t)
        {
            return new Displacement(Vector3.Lerp(a.Position, b.Position, t), Vector3.Lerp(a.EulerAngles, b.EulerAngles, t));
        }

        public Displacement ScaleBy(float posScale, float rotScale)
        {
            return new Displacement(Position * posScale, EulerAngles * rotScale);
        }

        public Displacement Normalized
        {
            get
            {
                return new Displacement(Position.normalized, EulerAngles.normalized);
            }
        }
        
        public static Displacement InsideUnitSphere()
        {
                return new Displacement(Random.insideUnitSphere, Random.insideUnitSphere);
        }
    }
}
