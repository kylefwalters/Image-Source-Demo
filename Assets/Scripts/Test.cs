using System.Runtime.InteropServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    private void FixedUpdate()
    {
        UpdateAudio();
    }

    private void UpdateAudio()
    {
        if(!_audioSource.isPlaying)
        {
            return;
        }

        GameObject player = Camera.main.gameObject;
        Vector3 direction = player.transform.position - transform.position;
        RaycastHit hitInfo = new();
        Physics.Raycast(transform.position, direction.normalized, out hitInfo, direction.magnitude);
        float spatialVolume = hitInfo.collider.gameObject == player ? 1 : 0;
        bool successful = _audioSource.SetSpatializerFloat(0, spatialVolume);
    }
}
