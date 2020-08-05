using System;
using UnityEngine;

public class Particle : GameboardObject
{
    [SerializeField] protected float _initialSpeedMax = 40f;
    [SerializeField] protected float _initialTorqueMax = 30f;
    [SerializeField] private float _lifetime = 2.4f;

    private DateTime _lifetimeEnd;

    protected override void Update()
    {
        if (DateTime.Now > _lifetimeEnd)
        {
            Destroy(gameObject);
        }

        base.Update();
    }

    private void Start()
    {
        var force = new Vector2(
            UnityEngine.Random.Range(-_initialSpeedMax, _initialSpeedMax),
            UnityEngine.Random.Range(-_initialSpeedMax, _initialSpeedMax));
        Vector2.ClampMagnitude(force, _initialSpeedMax);
        _body.AddForce(force);

        var torque = UnityEngine.Random.Range(-_initialTorqueMax, _initialTorqueMax);
        _body.AddTorque(torque);

        _lifetimeEnd = DateTime.Now.AddSeconds(_lifetime);
    }
}
