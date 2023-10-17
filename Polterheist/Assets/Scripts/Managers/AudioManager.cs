using MEC;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager instance;
    public static AudioManager Instance {
        get { return instance; }
        private set { }
    }
    public AudioMixerGroup mixerGroup;

    public float songTransitionDuration;
    private CoroutineHandle songTransition;
	public Sound[] sounds;

	void Awake() {
        // check if the instance already exists and destroy the new instance if it does
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        // set the instance to this object if it doesn't exist
        instance = this;
        // prevent the instance from being destroyed when loading new scenes
        DontDestroyOnLoad(this.gameObject);

        foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound, float delay = 0) {
		Timing.RunCoroutine(_PlayDelay(sound, delay));
	}

	private IEnumerator<float> _PlayDelay(string sound, float delay) {
		yield return Timing.WaitForSeconds(delay);
		
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null) {
			Debug.LogWarning("Sound: " + name + " not found!");
			yield break;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void Play(string sound) {
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null) {
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void Stop(string sound) {
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null) {
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Stop();
	}

	public AudioSource GetSound(string sound) {
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null) {
			Debug.LogWarning("Sound: " + name + " not found!");
			return null;
		}

		return s.source;
	}
}