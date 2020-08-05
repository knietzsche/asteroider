using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : GameboardObject
{
    private const float forceRatio = 50f;

    [SerializeField] private float _lifetime = 1.8f;

    public Action<Bullet> onDestruct;
    
    private DateTime _lifetimeEnd;

    protected override void Update()
    {
        if (DateTime.Now > _lifetimeEnd)
        {
            Destruct();
        }
        
        base.Update();
    }

    public void Fire(Vector2 velocity, float relativeForceRatio)
    {
        _body.AddForce(velocity * forceRatio);
        _body.AddRelativeForce(Vector2.up * relativeForceRatio);

        _lifetimeEnd = DateTime.Now.AddSeconds(_lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destruct();
    }

    private void Destruct()
    {
        onDestruct?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void OnValidate()
    {
         Debug.Assert(_lifetime > 0f);
    }
}
