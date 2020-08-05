using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource = null;
    [SerializeField] private AudioSource[] _soundSources = null;

    [SerializeField] private AudioClip[] _musicGame = null;
    [SerializeField] private AudioClip[] _musicMenu = null;

    [SerializeField] private AudioClip _asteroidExplode = null;
    [SerializeField] private AudioClip _specialExplode = null;
    [SerializeField] private AudioClip _shipExplode = null;

    public bool Music
    {
        get
        {
            return !_musicSource.mute;
        }
        set
        {
            _musicSource.mute = !value;
        }
    }

    private void Awake()
    {
        UserAction.ChangeScreen += OnChangeScreen;

        GameAction.CollidableColided += OnCollidableColided;
    }

    private void OnChangeScreen(ScreenManager.ScreenType screenType)
    {
        if (screenType == ScreenManager.ScreenType.Game)
        {
            _musicSource.clip = _musicGame[UnityEngine.Random.Range(0, _musicGame.Length)];
            _musicSource.Play();
        }
        if (screenType == ScreenManager.ScreenType.Menu)
        {
            _musicSource.clip = _musicMenu[UnityEngine.Random.Range(0, _musicMenu.Length)];
            _musicSource.Play();
        }
    }

    private void OnCollidableColided(Type type)
    {
        if (type == typeof(Ship))
        {
            PlayOneShot(_shipExplode);
        }
        if (type == typeof(Asteroid))
        {
            PlayOneShot(_asteroidExplode);
        }
        if (type.IsSubclassOf(typeof(Special)))
        {
            PlayOneShot(_specialExplode);
        }
    }

    private void PlayOneShot(AudioClip audioClip)
    {
        foreach (var soundSource in _soundSources)
        {
            if (!soundSource.isPlaying)
            {
                soundSource.clip = audioClip;
                soundSource.Play();
                return;
            }
        }
    }

    private void OnValidate()
    {
        Debug.Assert(_musicSource != null);
        Debug.Assert(_soundSources.Length > 0);
        Debug.Assert(_musicGame.Length > 0);
        Debug.Assert(_musicMenu.Length > 0);
        Debug.Assert(_asteroidExplode != null);
        Debug.Assert(_specialExplode != null);
    }
}
