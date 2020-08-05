using System;
using UnityEngine;

public class Enemy : Special
{
    [SerializeField] private float _speed = 80f;
    [SerializeField] private Transform _pivot = null;
    [SerializeField] private Gun _gun = null;
    [SerializeField] private float _fireInterval = 1f;
    [SerializeField] private float _changeDirectionWaitMax = 5f;

    private DateTime _fireThen;
    private DateTime _changeDirectionThen;
    private bool _direction;

    protected override void OnEnable()
    {
        base.OnEnable();

        _fireThen = DateTime.Now.AddSeconds(_fireInterval);

        _direction = _transform.position.x < 0f;
        _body.velocity = _direction ? Vector2.left : Vector2.right;
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        Vector2 force;
        do
        {
            force = new Vector2(
                UnityEngine.Random.Range(_direction ? 0 : -1, _direction ? 2 : 1),
                UnityEngine.Random.Range(-1, 2));
        } while (force == Vector2.zero || Vector2.Dot(force, _body.velocity) == 1);
        force *= _speed;
        Vector2.ClampMagnitude(force, _speed);

        _body.velocity = Vector2.zero;
        _body.AddForce(force);

        _changeDirectionThen = DateTime.Now.AddSeconds(
            UnityEngine.Random.Range(0f, _changeDirectionWaitMax));
    }

    private void Fire()
    {
        var angle = UnityEngine.Random.Range(0f, 360f);
        _pivot.Rotate(Vector3.forward, angle);
        _gun.RequestFire();

        _fireThen = DateTime.Now.AddSeconds(_fireInterval);
    }

    protected override void Update()
    {
        base.Update();
        var now = DateTime.Now;
        if (now > _changeDirectionThen)
        {
            ChangeDirection();
        }
        if (now > _fireThen)
        {
            Fire();
        }
    }
}
