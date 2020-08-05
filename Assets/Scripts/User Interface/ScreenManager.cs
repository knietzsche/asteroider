using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] GameObject _menuScreen = null;
    [SerializeField] GameObject _gameScreen = null;

    private void Awake()
    {
        UserAction.ChangeScreen += OnChangeScreen;
    }

    private void OnChangeScreen(ScreenType screenType)
    {
        _gameScreen.SetActive(screenType == ScreenType.Game);
        _menuScreen.SetActive(screenType == ScreenType.Menu);
    }

    private void OnValidate()
    {
        Debug.Assert(_menuScreen != null);
        Debug.Assert(_gameScreen != null);
    }

    public enum ScreenType
    {
        Menu, Game
    }
}
