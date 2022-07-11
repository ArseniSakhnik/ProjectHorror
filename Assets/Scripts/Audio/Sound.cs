using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.5f, 3f)]
    public float pitch;

    public AudioSource source;

    public bool loopable;
}
