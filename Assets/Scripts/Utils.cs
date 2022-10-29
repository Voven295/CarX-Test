using UnityEngine;

namespace TowerDefence
{
    public class Utils
    {
        public static System.Numerics.Vector3 FromUnityToNumerics(Vector3 vector)
        {
            return new System.Numerics.Vector3(vector.x, vector.y, vector.z);
        }
        
        public static Vector3 FromNumericsToUnity(System.Numerics.Vector3 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
        
        public static System.Numerics.Quaternion FromUnityToNumerics(Quaternion quaternion)
        {
            return new System.Numerics.Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }
        
        public static Quaternion FromNumericsToUnity(Quaternion quaternion)
        {
            return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }
        
        public static System.Numerics.Vector3 Lerp(System.Numerics.Vector3 a, System.Numerics.Vector3 b, float t)
        {
            return FromUnityToNumerics(Vector3.Lerp(FromNumericsToUnity(a), FromNumericsToUnity(b), t));
        }
    }
}