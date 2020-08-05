using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GameboardObject : MonoBehaviour
{
    protected Transform _transform;
    protected Rigidbody2D _body;

    protected virtual void Awake()
    {
        _transform = transform;
        _body = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        Viewport.WrapPosition(_transform);
    }   
}
