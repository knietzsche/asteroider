using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collidable : GameboardObject
{
    [SerializeField] private GameboardObject _debrisPrefab = null;
    [SerializeField] private int _debrisCount = 0;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (_debrisPrefab != null)
        {
            for (int i = 0; i < _debrisCount; i++)
            {
                Instantiate(_debrisPrefab, _transform.position, _transform.rotation, _transform.parent);
            }
        }

        GameAction.CollidableColided?.Invoke(GetType());
        Destroy(gameObject);
    }
}
