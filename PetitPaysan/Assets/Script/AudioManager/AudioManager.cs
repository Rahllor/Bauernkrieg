using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public List<Sound> sounds;

    public static AudioManager instance;
    public AudioMixer vaudioMixer;


    // Use this for initialization
    void Awake () {

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        

        foreach(Sound s in sounds)
        {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		#if UNITY_WEBGL
			Debug.Log("AudioMixer Not Supported");
		#else
			if (s.output != null || s.output != "")
			{
				//Debug.Log(vaudioMixer.FindMatchingGroups(s.output)[0]);
				s.source.outputAudioMixerGroup = vaudioMixer.FindMatchingGroups(s.output)[0];
			}
		#endif
        }

        Debug.Log("Array: " + sounds.Count);
        Debug.Log("AudioManager loaded complete !");
    }
	
	// Update is called once per frame
	void Start () {
        Debug.Log("Array: " + sounds.Count);
	}

    public void Play(string name)
    {
        Debug.Log(name);

        if (sounds.Count <= 0)
        {
            Debug.Log("No sound currently set in this game !");
            return;
        }

        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //Debug.Log(s.name + " VS " + name);

        s.source.Play();
    }

    public void Stop(string name)
    {
        if (sounds.Count <= 0)
        {
            Debug.Log("No sound currently set in this game !");
            return;
        }

        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        if(sounds.Count <= 0)
        {
            Debug.Log("No sound currently set in this game !");
            return;
        }

        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }

    }

    public void AddSound(Sound s)
    {
		s.source = gameObject.AddComponent<AudioSource>();

        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;

		#if UNITY_WEBGL
            Debug.Log("AudioMixer Not Supported");
		#else
        if (s.output != null && s.output != "")
        {
			//Debug.Log(vaudioMixer.FindMatchingGroups(s.output)[0]);
            s.source.outputAudioMixerGroup = vaudioMixer.FindMatchingGroups(s.output)[0];
        }
		#endif
		
		sounds.Add(s);
    }


}
