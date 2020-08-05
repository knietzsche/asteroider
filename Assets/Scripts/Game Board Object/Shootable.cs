using UnityEngine;

public class Shootable : Collidable
{
    [SerializeField] private int _score = 0;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ship"))
        {
            GameAction.AddScore?.Invoke(_score);
        }

        base.OnCollisionEnter2D(collision);
    }
}
