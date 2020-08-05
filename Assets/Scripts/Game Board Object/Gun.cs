using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab = null;
    [SerializeField] private float _forceRatio = 200;
    [SerializeField] private float _cooldown = .2f;

    private Queue<Bullet> _bulletQueue = new Queue<Bullet>();
    private Rigidbody2D _body;
    private Transform _gameBoard;
    private AudioSource _audioSource;
    private DateTime _then;
    private int _counter;

    private void Awake()
    {
        _body = GetComponentInParent<Rigidbody2D>();
        _gameBoard = _body.transform.parent;
        _audioSource = GetComponent<AudioSource>();
        _then = DateTime.MinValue;
    }

    private void Update()
    {
        if (_counter > 0)
        {
            var now = DateTime.Now;
            if ((now - _then).TotalSeconds > _cooldown)
            {
                _counter = 0;
                ExecuteFire();
                _then = now;
            }
        }
    }

    private void ExecuteFire()
    {
        Bullet bullet;

        if (_bulletQueue.Count == 0)
        {
            bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation, _gameBoard);
            bullet.gameObject.layer = gameObject.layer;
            bullet.onDestruct += OnBulletDestruct;
        }
        else
        {
            bullet = _bulletQueue.Dequeue();
            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.gameObject.SetActive(true);
        }

        bullet.Fire(_body.velocity, _forceRatio);
        _audioSource.Play();
    }

    private void OnBulletDestruct(Bullet bullet)
    {
        _bulletQueue.Enqueue(bullet);
    }

    public void RequestFire()
    {
        _counter += 1;
    }

    private void OnValidate()
    {
        Debug.Assert(_bulletPrefab != null);
    }
}
