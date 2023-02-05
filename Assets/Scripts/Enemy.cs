using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Blue,
    Red,
    Yellow
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Renderer renderer;
    
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material yellowMaterial;
    
    [SerializeField] private AudioClip deathSound;
    
    [SerializeField] private float blueOrbitSpeed = 1f;
    [SerializeField] private float redOrbitSpeed = 1f;
    [SerializeField] private float yellowOrbitSpeed = 1f;
    
    [SerializeField] private int blueHealth = 1;
    [SerializeField] private int redHealth = 1;
    [SerializeField] private int yellowHealth = 1;

    private Transform _target;
    private string _targetTag;
    private float _orbitSpeed;
    private int _health;

    public void Initialize(EnemyType type, Transform target, string planet)
    {
        enemyType = type;
        _target = target;
        _targetTag = planet;
        switch (enemyType)
        {
            case EnemyType.Blue:
                renderer.material = blueMaterial;
                _orbitSpeed = blueOrbitSpeed;
                _health = blueHealth;
                break;
            case EnemyType.Red:
                renderer.material = redMaterial;
                _orbitSpeed = redOrbitSpeed;
                _health = redHealth;
                break;
            case EnemyType.Yellow:
                renderer.material = yellowMaterial;
                _orbitSpeed = yellowOrbitSpeed;
                _health = yellowHealth;
                break;
        }
    }
    
    private void Update()
    {
        transform.RotateAround(_target.position, Vector3.up, _orbitSpeed * Time.deltaTime);
    }

    public void Hit(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            FindObjectOfType<EnemyManager>().EnemyDestroyed(_targetTag);
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Destroy(gameObject);
        }
    }
}
