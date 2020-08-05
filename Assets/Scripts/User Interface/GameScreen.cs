using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    [SerializeField] private Image[] _lifes = null;
    [SerializeField] private Sprite[] _numerals = null;
    [SerializeField] private Image[] _digits = null;
    [SerializeField] private Text _gameOver = null;
    [SerializeField] private Text _paused = null;

    private void OnEnable()
    {
        InterfaceAction.ClearGameScreen += OnClearGameScreen;
        InterfaceAction.UpdateScore += OnUpdateScore;
        InterfaceAction.UpdateLife += OnUpdateLife;
        InterfaceAction.UpdateGamePaused += OnUpdateGamePaused;
        InterfaceAction.UpdateGameOver += OnUpdateGameOver;

        UserAction.Confirm += OnConfirm;
        UserAction.Escape += OnEscape;
    }

    private void OnDisable()
    {
        InterfaceAction.ClearGameScreen -= OnClearGameScreen;
        InterfaceAction.UpdateScore -= OnUpdateScore;
        InterfaceAction.UpdateLife -= OnUpdateLife;
        InterfaceAction.UpdateGamePaused -= OnUpdateGamePaused;
        InterfaceAction.UpdateGameOver -= OnUpdateGameOver;

        UserAction.Confirm -= OnConfirm;
        UserAction.Escape -= OnEscape;
    }

    private void OnClearGameScreen()
    {
        OnUpdateLife(0);
        OnUpdateScore(0);
        _paused.enabled = false;
        _gameOver.enabled = false;
    }

    private void OnUpdateLife(int value)
    {
        for (int i = 0; i < _lifes.Length; i++)
        {
            _lifes[i].enabled = (value > i);
        }
    }

    private void OnUpdateScore(int value)
    {
        _digits[0].sprite = _numerals[value % 10];

        for (int i = 1; i < _digits.Length; i++)
        {
            var number = Mathf.Pow(10f, i);
            _digits[i].enabled = (value >= number);
            var index = ((int) ((value / number) % number)) % _numerals.Length;
            _digits[i].sprite = _numerals[index % _numerals.Length];
        }
    }

    private void OnUpdateGamePaused(bool value)
    {
        _paused.enabled = value;
    }

    private void OnUpdateGameOver()
    {
        _gameOver.enabled = true;
    }

    private void OnEscape()
    {
        UserAction.ChangeScreen?.Invoke(ScreenManager.ScreenType.Menu);
    }

    private void OnConfirm()
    {
        if (GameManager.gameState == GameManager.GameState.ended)
        {
            UserAction.ChangeScreen?.Invoke(ScreenManager.ScreenType.Menu);
        }
    }

    private void OnValidate()
    {
        Debug.Assert(_digits.Length > 0);
        Debug.Assert(_numerals.Length == 10);
        Debug.Assert(_paused != null);
        Debug.Assert(_gameOver != null);
    }
}
