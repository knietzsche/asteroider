using UnityEngine;

public class Asteroid : Shootable
{
    [SerializeField] protected float _initialSpeedMax = 120f;
    [SerializeField] protected float _initialTorqueMax = 30f;

    private void OnEnable()
    {
        GameAction.UpdateAsteroidCount?.Invoke(true);
    }

    private void OnDisable()
    {
        GameAction.UpdateAsteroidCount?.Invoke(false);
    }

    protected virtual void Start()
    {
        var force = new Vector2(
            Random.Range(-_initialSpeedMax, _initialSpeedMax),
            Random.Range(-_initialSpeedMax, _initialSpeedMax));
        Vector2.ClampMagnitude(force, _initialSpeedMax);
        _body.AddForce(force);

        var torque = Random.Range(-_initialTorqueMax, _initialTorqueMax);
        _body.AddTorque(torque);
    }
}
