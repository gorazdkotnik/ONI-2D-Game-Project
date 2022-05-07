using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
  [SerializeField] AudioMixer audioMixer;
  [SerializeField] Sound[] sounds;

  public static AudioManager instance;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);

    SceneManager.sceneLoaded += OnSceneLoaded;

    foreach (Sound s in sounds)
    {
      s.source = gameObject.AddComponent<AudioSource>();

      s.source.clip = s.clip;

      s.source.volume = s.volume;
      s.source.pitch = s.pitch;
      s.source.loop = s.loop;
      s.source.outputAudioMixerGroup = s.group;
    }
  }

  void Start()
  {
    audioMixer.SetFloat("volume", PlayerPrefs.HasKey("volume") ? PlayerPrefs.GetFloat("volume") : 0f);
  }

  public void Play(string name)
  {
    Sound s = Array.Find(sounds, sound => sound.name == name);

    if (s != null && s.source != null) s.source.Play();
  }

  public void Stop(string name)
  {
    if (sounds == null) return;

    Sound s = Array.Find(sounds, sound => sound.name == name);

    if (s != null && s.source != null) s.source.Stop();
  }

  public void StopAll()
  {
    Array.ForEach(sounds, sound => sound.source.Stop());
  }

  public bool isPlaying(string name)
  {
    Sound s = Array.Find(sounds, sound => sound.name == name);

    if (s != null) return s.source.isPlaying;
    return false;
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    StopAll();
  }
}
