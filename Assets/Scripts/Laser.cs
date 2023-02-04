using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum LaserType
{
    Red,
    Blue
}

public class Laser : MonoBehaviour
{
    [SerializeField] private LaserType laserType;
    [SerializeField] private Renderer laserRenderer;

    [SerializeField, BoxGroup("Blue")] private float blueSpeed = 10f;
    [SerializeField, BoxGroup("Blue")] private float blueLifeTime = 2f;
    [SerializeField, BoxGroup("Blue")] private float blueDamage = 1f;
    [SerializeField, BoxGroup("Blue")] private AudioClip blueLaserSound;
    [SerializeField, BoxGroup("Blue")] private Material blueLaserMaterial;
    
    [SerializeField, BoxGroup("Red")] private float redSpeed = 10f;
    [SerializeField, BoxGroup("Red")] private float redLifeTime = 2f;
    [SerializeField, BoxGroup("Red")] private float redDamage = 1f;
    [SerializeField, BoxGroup("Red")] private AudioClip redLaserSound;
    [SerializeField, BoxGroup("Red")] private Material redLaserMaterial;

    private float _speed;
    private float _lifeTime;
    private float _damage;
    private AudioClip _audioClip;
    
    public void Initialize(LaserType type)
    {
        laserType = type;
        switch (laserType)
        {
            case LaserType.Red:
                _speed = redSpeed;
                _lifeTime = redLifeTime;
                _damage = redDamage;
                _audioClip = redLaserSound;
                laserRenderer.material = redLaserMaterial;
                break;
            case LaserType.Blue:
                _speed = blueSpeed;
                _lifeTime = blueLifeTime;
                _damage = blueDamage;
                _audioClip = blueLaserSound;
                laserRenderer.material = blueLaserMaterial;
                break;
        }
        Destroy(gameObject, _lifeTime);
        AudioSource.PlayClipAtPoint(_audioClip, transform.position);
    }

    private void FixedUpdate()
    {
        //move laser local forward
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime), Space.Self);
    }
    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         other.GetComponent<Enemy>().TakeDamage(damage);
    //         Destroy(gameObject);
    //     }
    // }
}
