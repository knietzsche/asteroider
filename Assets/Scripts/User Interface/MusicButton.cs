using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MusicButton : MonoBehaviour
{
    private const string labelPre = "MUSIC ";
    private const string labelPostTrue = "ON";
    private const string labelPostFalse = "OFF";

    private Button _button;
    private Text _text;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(onClick);
        _text = GetComponent<Text>();
    }

    private void Start()
    {
        UpdateButtonLabel();
    }

    private void onClick()
    {
        SettingsManager.Music = !SettingsManager.Music;

        UpdateButtonLabel();
    }

    private void UpdateButtonLabel()
    {
        _text.text = labelPre + (SettingsManager.Music ? labelPostTrue : labelPostFalse);
    }
}
