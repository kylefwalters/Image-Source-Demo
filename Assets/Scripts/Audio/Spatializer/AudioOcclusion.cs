using UnityEngine;

namespace Audio.Spatializer
{
    // Provides raycast-based audio occlusion
    public class AudioOcclusion : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [Tooltip("Volume when audio source is blocked by an object")]
        [SerializeField, Range(0.0f, 0.99f)]
        private float _muffledVolume = 0.5f;

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

            GameObject player = Camera.main.gameObject;
            Vector3 direction = player.transform.position - transform.position;
            RaycastHit hitInfo = new();
            Physics.Raycast(transform.position, direction.normalized, out hitInfo, direction.magnitude);
            float spatialVolume = hitInfo.collider?.gameObject == player ? 1 : _muffledVolume;
            _audioSource.SetSpatializerFloat(0, spatialVolume);
        }
    }
}