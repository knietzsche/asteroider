using System;
using System.Collections;
using UnityEngine;

public class Ship : Collidable
{
    [SerializeField] private SpriteRenderer _rocket = null;
    [SerializeField] private SpriteRenderer _thruster = null;
    [SerializeField] private AudioSource _thrusterSound = null;
    [SerializeField] private Gun _gun = null;
    [SerializeField] private float _gracePeriodDuration = 1.4f;

    private Collider2D _collider;

    private bool _thrusting;

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        UserAction.Thrust += OnThrust;
        UserAction.Rotate += OnRotate;
        UserAction.Fire += OnFire;
    }

    private void OnDisable()
    {
        UserAction.Thrust -= OnThrust;
        UserAction.Rotate -= OnRotate;
        UserAction.Fire -= OnFire;
    }

    public void Initialize()
    {
        StartCoroutine(GracePeriod());
    }

    protected override void Update()
    {
        base.Update();

        if (!_thruster.enabled && _thrusting)
        {
            _thrusterSound.Play();
        }

        _thruster.enabled = _thrusting;
        if (_thrusting)
        {
            _thrusting = false;
        }
    }

    private void OnDestroy()
    {
        GameAction.ShipDestroyed?.Invoke();
    }

    private void OnThrust(float force)
    {
        if (GameManager.gameState == GameManager.GameState.running)
        {
            _body.AddRelativeForce(Vector2.up);
            _thrusting = true;
        }
    }

    private void OnRotate(float value)
    {
        if (GameManager.gameState == GameManager.GameState.running)
        {
            _body.freezeRotation = true;
            transform.Rotate(Vector3.back, value);
            _body.freezeRotation = false;
        }
    }

    private void OnFire()
    {
        if (GameManager.gameState == GameManager.GameState.running)
        {
            _gun.RequestFire();
        }
    }

    private IEnumerator GracePeriod()
    {
        _collider.enabled = false;

        var threshold = DateTime.Now.AddSeconds(_gracePeriodDuration);
        while (DateTime.Now < threshold)
        {
            _rocket.enabled = !_rocket.enabled;

            yield return new WaitForSeconds(.1f);
        }
        _rocket.enabled = true;

        _collider.enabled = true;
    }

    private void OnValidate()
    {
        Debug.Assert(_gun != null);
        Debug.Assert(_rocket != null);
        Debug.Assert(_thruster != null);
        Debug.Assert(_thrusterSound != null);
    }
}
