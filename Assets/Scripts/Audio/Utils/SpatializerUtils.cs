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

        public static ColliderFace[] GetColliderFaces(Collider collider)
        {
            if (collider == null) { return null; }

            ColliderFace[] colliderFaces = null;

            switch (collider)
            {
                case BoxCollider:
                    colliderFaces = GetBoxColliderFaces(collider);
                    break;
                case SphereCollider:
                    colliderFaces = GetSphereColliderFaces(collider);
                    break;
                case MeshCollider:
                    colliderFaces = GetMeshColliderFaces(collider);
                    break;
            }

            return colliderFaces;
        }

        private static ColliderFace[] GetBoxColliderFaces(Collider collider)
        {
            return null;
        }

        private static ColliderFace[] GetSphereColliderFaces(Collider collider)
        {
            return null;
        }

        private static ColliderFace[] GetMeshColliderFaces(Collider collider)
        {
            MeshCollider meshCollider = (MeshCollider)collider;
            Mesh sharedMesh = meshCollider.sharedMesh;
            Vector3[] sharedVertices = sharedMesh.vertices;
            ColliderFace[] colliderFaces = new ColliderFace[sharedVertices.Length / 3];

            for(int i = 0; i < sharedVertices.Length; i += 3)
            {
                Vector3[] vertices = { sharedVertices[i], sharedVertices[i + 1], sharedVertices[i + 2] };
                Plane plane = new Plane(vertices[0], vertices[1], vertices[2]);
                ColliderFace colliderFace = new (plane, vertices);
                colliderFaces[i / 3] = colliderFace;
            }

            return colliderFaces;
        }
    }
}
