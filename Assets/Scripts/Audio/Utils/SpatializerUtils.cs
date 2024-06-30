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
            BoxCollider boxCollider = (BoxCollider)collider;
            Vector3[] vertices = new Vector3[36];
            Vector3 center = boxCollider.transform.localToWorldMatrix * boxCollider.center;

            // TODO: turn into reusable method
            // Top
            vertices[0] = center + new Vector3(-boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z);
            vertices[1] = center + new Vector3(boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z);
            vertices[2] = center + new Vector3(-boxCollider.size.x, boxCollider.size.y, boxCollider.size.z);
            vertices[3] = center + new Vector3(-boxCollider.size.x, boxCollider.size.y, boxCollider.size.z);
            vertices[4] = center + new Vector3(boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z);
            vertices[5] = center + new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z);
            //for (int i = 0; i < 6; i++)
            //{
            //    int index = i * 6;
            //    vertices[i] = ;
            //}

            return GetColliderFaces(vertices);
        }

        private static ColliderFace[] GetSphereColliderFaces(Collider collider)
        {
            return null;
        }

        private static ColliderFace[] GetMeshColliderFaces(Collider collider)
        {
            MeshCollider meshCollider = (MeshCollider)collider;
            Vector3[] sharedVertices = meshCollider.sharedMesh.vertices;

            return GetColliderFaces(sharedVertices);
        }

        private static ColliderFace[] GetColliderFaces(Vector3[] vertices)
        {
            ColliderFace[] colliderFaces = new ColliderFace[vertices.Length / 3];

            for (int i = 0; i < vertices.Length; i += 3)
            {
                Vector3[] faceVertices = { vertices[i], vertices[i + 1], vertices[i + 2] };
                Plane plane = new Plane(faceVertices[0], faceVertices[1], faceVertices[2]);
                ColliderFace colliderFace = new(plane, faceVertices);
                colliderFaces[i / 3] = colliderFace;
            }

            return colliderFaces;
        }
    }
}
