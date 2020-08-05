using UnityEngine;

public class Bonus : Special
{
    [SerializeField] protected float _initialForceRatio = 4f;
    [SerializeField] protected float _initialTorqueMax = 10f;

    protected override void OnEnable()
    {
        base.OnEnable();
        AddInitialForce();
        AddInitialTorque();
    }

    protected void AddInitialForce()
    {
        var destination = new Vector2(
            -_transform.position.x,
            Random.Range(Viewport.screenMin.y, Viewport.screenMax.y));

        var force = (destination - (Vector2) _transform.position) * _initialForceRatio;
        _body.AddForce(force);
    }

    protected void AddInitialTorque()
    {
        var torque = Random.Range(-_initialTorqueMax, _initialTorqueMax);
        _body.AddTorque(torque);
    }
}
