using System;
using System.Collections;
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

            source.volume = s.volume;
            source.pitch = s.pitch;
            source.spatialBlend = 1;
            source.PlayOneShot(s.clip, s.volume);
        }

        else s.source.PlayOneShot(s.clip, s.volume);
    }

    public IEnumerator DestroyAfterPlay(float time, AudioSource source)
    {
        yield return new WaitForSeconds(time);
        Destroy(source);
    }

    private void Update()
    {
        
    }

}
