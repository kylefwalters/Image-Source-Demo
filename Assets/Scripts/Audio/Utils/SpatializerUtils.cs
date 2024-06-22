using UnityEngine;

namespace Audio.Utils
{
    public enum SpatializerIndex
    {
        VOLUME = 0
    }

    public class ColliderFace
    {
        private Plane _plane;
        private Vector3[] _vertices = new Vector3[3];

        public Plane GetPlane => _plane;
        public Vector3[] GetVertices => _vertices;

        public ColliderFace(Plane plane, Vector3[] vertices)
        {
            _plane = plane;
            _vertices = vertices;
        }
    }

    public static class SpatializerUtils
    {
        public static bool SetSpatializerFloat(AudioSource audioSource, SpatializerIndex index, float value)
        {
            return audioSource.SetSpatializerFloat((int)index, value);
        }

        public static bool GetSpatializerFloat(AudioSource audioSource, SpatializerIndex index, out float value)
        {
            return audioSource.GetSpatializerFloat((int)index, out value);
        }
    }
}
