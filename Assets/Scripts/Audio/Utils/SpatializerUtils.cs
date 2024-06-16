using UnityEngine;

namespace Audio.Utils
{
    public enum SpatializerIndex
    {
        VOLUME = 0
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
