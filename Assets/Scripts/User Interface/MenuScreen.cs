using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private Transform _menu = null;
    [SerializeField] private Transform _selector = null;

    private Button[] _options;
    private int _selected = 0;

    private void Awake()
    {
        _options = _menu.GetComponentsInChildren<Button>();
    }

    private void OnEnable()
    {
        UserAction.Select += OnSelect;
        UserAction.Confirm += OnConfirm;
        UserAction.Escape += OnEscape;
    }

    private void OnDisable()
    {
        UserAction.Select -= OnSelect;
        UserAction.Confirm -= OnConfirm;
        UserAction.Escape -= OnEscape;
    }

    private void Start()
    {
        UpdateSelector();
    }

    private void UpdateSelector()
    {
        _selector.SetParent(_options[_selected].transform, false);
    }

    private void OnSelect(bool value)
    {
        _selected = (_options.Length + _selected + (value ? 1 : -1)) % _options.Length;
        UpdateSelector();
    }

    private void OnConfirm()
    {
        _options[_selected].onClick?.Invoke();
    }

    private void OnEscape()
    {
        Application.Quit();
    }

    public void OnClickGameStart()
    {
        UserAction.ChangeScreen?.Invoke(ScreenManager.ScreenType.Game);
    }

    public void OnClickGameQuit()
    {
        UserAction.Escape?.Invoke();
    }

    private void OnValidate()
    {
        Debug.Assert(_selector != null);
        Debug.Assert(_menu != null);
    }
}
