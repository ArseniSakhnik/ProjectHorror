using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [NonReorderable]
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string soundName, AudioSource source = null)
    {
        Sound s;

        s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) return;

        if (source != null)                         // если хотим звук от родителя
        {
            source.spatialBlend = 1;
            source.PlayOneShot(s.clip, s.volume);
        }

        else s.source.PlayOneShot(s.clip, s.volume);
    }

}
