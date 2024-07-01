using Audio.Utils;
using UnityEngine;

namespace Audio.Spatializer
{
    /// <summary>
    /// Uses the image source technique to calculate reverb and direct sound.
    /// Since it's performance cost scales with the number of faces and it has an exponential complexity, 
    /// this method is not recommended for areas with many faces
    /// </summary>
    public class ImageSourceSpatializer : MonoBehaviour
    {
        // TODO: Use object faces instead of collider
        // TODO: Add method of baking static geometry

        [SerializeField]
        private AudioSource _audioSource;

        [Tooltip("Number of times sound can reflect off an object")]
        [SerializeField, Range(0, 5)]
        private int _reflectionLimit = 3;

        [Tooltip("Volume falloff per reflection")]
        [SerializeField, Range(0.01f, 1.0f)]
        private float _volumeScalar = 1.0f;

        [Tooltip("Reverb gain per reflection")]
        [SerializeField, Range(0.01f, 1.0f)]
        private float _reverbScalar = 1.0f;

        [Tooltip("Max angle a face can have relative to audio/reverb source")]
        [SerializeField, Range(0.0f, 90.0f)]
        private float angleThreshold = 90.0f;

        private void FixedUpdate()
        {
            UpdateAudio();
        }

        private void UpdateAudio()
        {
            if (!_audioSource.isPlaying)
            {
                return;
            }

            //TODO: Run performance test to see if this is faster than running GetObjectsInRadius for each recursive call
            Collider[] colliders = GetObjectsInRadius(_audioSource.maxDistance);
            SpatializerUtils.SetSpatializerFloat(_audioSource, SpatializerIndex.VOLUME, 0.4f);
            ImageSourceReflection(colliders, _audioSource.transform.position, _audioSource.maxDistance);
        }

        private void ImageSourceReflection(Collider[] colliders, Vector3 currentPos, float radius, int currentReflection = 0)
        {

            if (currentReflection == _reflectionLimit)
            { return; }

            // TODO: Run LoS check and account for limited access, diffraction, or no access
            // TODO: run this for every face in collider
            //      getting the faces of a collider will require calculating them in the case of non-mesh colliders,
            //      faces can be returned as Planes, use .ClosestPointOnPlane then constrain result to dimensions of face
            foreach (Collider collider in colliders)
            {
                //float angle = Quaternion.Angle();
                //if (angle >= angleThreshold)
                //    continue;

                Vector3 collisionPoint = collider.ClosestPoint(currentPos);

                Vector3 dir = collisionPoint - currentPos;
                Vector3 ImagesSourcePos = collisionPoint + dir;
                float newRadius = radius - dir.magnitude;
                // TODO: Treat reflected sound as a beam, find way to tell if collider intersects with the beam
                // TODO: Check that beam is not obstructed by other colliders
                ImageSourceReflection(colliders, ImagesSourcePos, newRadius, currentReflection + 1);
            }

            if (PlayerCanHear(currentPos, radius))
            {
                float currentVolume;
                SpatializerUtils.GetSpatializerFloat(_audioSource, SpatializerIndex.VOLUME, out currentVolume);
                float additionalVolume = Mathf.Pow(1 / (currentReflection + 1), 2);
                currentVolume = Mathf.Min(currentVolume + additionalVolume, 1.0f);
                SpatializerUtils.SetSpatializerFloat(_audioSource, SpatializerIndex.VOLUME, currentVolume);
            }
        }

        private bool PlayerCanHear(Vector3 currentPos, float radius)
        {
            Vector3 targetPos = Camera.main.transform.position;
            Vector3 direction = targetPos - currentPos;

            bool canHear = Physics.Raycast(currentPos, direction.normalized, out RaycastHit hitInfo, radius) && hitInfo.collider.gameObject == Camera.main.gameObject;
            return canHear;
        }

        private Collider[] GetObjectsInRadius(float radius)
        {
            return Physics.OverlapSphere(_audioSource.transform.position, radius);
        }
    }
}