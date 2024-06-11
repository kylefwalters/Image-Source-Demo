using System.Collections.Generic;
using UnityEngine;

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
    private int _reflectionCount = 3;

    [Tooltip("Volume falloff per reflection")]
    [SerializeField, Range(0.01f, 1.0f)]
    private float _volumeScalar = 1.0f;

    [Tooltip("Reverb gain per reflection")]
    [SerializeField, Range(0.01f, 1.0f)]
    private float _reverbScalar = 1.0f;

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
    }

    private void ImageSourceReflection(int currentReflection, Collider[] colliders, Vector3 currentPos, float radius)
    {

        if(currentReflection == _reflectionCount)
        { return; }

        foreach (Collider collider in colliders)
        {
            Vector3 ImagesSourcePos;
            float newRadius;
            //ImageSourceReflection(currentReflection + 1, colliders, ImagesSourcePos, newRadius);
        }
    }

    private Collider[] GetObjectsInRadius(float radius)
    {
        return Physics.OverlapSphere(_audioSource.transform.position, radius);
    }
}
