using JetBrains.Annotations;
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
                case SphereCollider:
                    colliderFaces = GetSphereColliderFaces(collider);
                    break;
                case BoxCollider:
                    colliderFaces = GetBoxColliderFaces(collider);
                    break;
                case MeshCollider:
                    colliderFaces = GetMeshColliderFaces(collider);
                    break;
            }

            return colliderFaces;
        }

        private static ColliderFace[] GetSphereColliderFaces(Collider collider)
        {
            return null;
        }

        private static ColliderFace[] GetBoxColliderFaces(Collider collider)
        {
            BoxCollider boxCollider = (BoxCollider)collider;

            Vector3[] vertices = GetBoxVertices(boxCollider);

            return GetColliderFaces(vertices);
        }

        private static Vector3[] GetBoxVertices(BoxCollider boxCollider)
        { 
            Vector3[] sideOffset = {
                new Vector3(1,0,0),
                new Vector3(-1,0,0),
                new Vector3(0,1,0),
                new Vector3(0,-1,0),
                new Vector3(0,0,1),
                new Vector3(0,0,-1)
            };
            Vector3[] sideVertices = {
                new Vector3(-boxCollider.size.x, 0, -boxCollider.size.z),
                new Vector3(boxCollider.size.x, 0, -boxCollider.size.z),
                new Vector3(-boxCollider.size.x, 0, boxCollider.size.z),
                new Vector3(-boxCollider.size.x, 0, boxCollider.size.z),
                new Vector3(boxCollider.size.x, 0, -boxCollider.size.z),
                new Vector3(boxCollider.size.x, 0, boxCollider.size.z)
            };
            Quaternion[] sideRotations = {
                Quaternion.AngleAxis(90, Vector3.forward),
                Quaternion.AngleAxis(-90, Vector3.forward),
                Quaternion.AngleAxis(0, Vector3.forward),
                Quaternion.AngleAxis(180, Vector3.forward),
                Quaternion.AngleAxis(90, Vector3.right),
                Quaternion.AngleAxis(-90, Vector3.right),
            };
            Vector3 center = boxCollider.transform.localToWorldMatrix * boxCollider.center;

            Vector3[] boxVertices = new Vector3[36];

            for(int sideIndex = 0; sideIndex < 6; sideIndex++)
            {
                for(int vertexIndex = 0; vertexIndex < 6; vertexIndex++)
                {
                    Quaternion localRotation = boxCollider.transform.rotation;
                    Vector3 vertexDirection = sideVertices[vertexIndex] - center;
                    vertexDirection = localRotation * sideRotations[sideIndex] * vertexDirection;

                    boxVertices[sideIndex * 6 + vertexIndex] = 
                        center + sideOffset[sideIndex] + vertexDirection;
                }
            }

            return boxVertices;
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
