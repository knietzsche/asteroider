using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            UserAction.Thrust?.Invoke(1f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            UserAction.Rotate?.Invoke(1.4f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            UserAction.Rotate?.Invoke(-1.4f);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UserAction.Confirm?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UserAction.Confirm?.Invoke();
            UserAction.Fire?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UserAction.Select?.Invoke(false);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            UserAction.Select?.Invoke(true);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            UserAction.PauseGame?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Pause))
        {
            UserAction.PauseGame?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UserAction.Escape?.Invoke();
        }
    }
}
