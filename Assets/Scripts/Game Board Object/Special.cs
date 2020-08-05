using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Special : Shootable
{
    private AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnEnable()
    {
        _audioSource.Play();
    }

    private void OnDestroy()
    {
        GameAction.SpecialDestroyed?.Invoke();
    }

    protected override void Update()
    {
        if (Viewport.IsOutsideViewport(_transform))
        {
            Destroy(gameObject);
        }
        base.Update();
    }
}
