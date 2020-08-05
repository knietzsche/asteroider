using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SoundManager _soundManager;

    public static bool Music
    {
        get
        {
            return _soundManager.Music;
        }
        set
        {
            _soundManager.Music = value;
            PlayerPrefs.SetInt("music", value ? 1 : 0);
        }
    }

    private void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        _soundManager.Music = (PlayerPrefs.GetInt("music", 1) == 1);
    }
}
