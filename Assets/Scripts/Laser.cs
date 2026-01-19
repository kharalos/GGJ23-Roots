using Alchemy.Inspector;
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
    [SerializeField] private bool enableLogging = true;

    [SerializeField, BoxGroup("Blue")] private float blueSpeed = 10f;
    [SerializeField, BoxGroup("Blue")] private float blueLifeTime = 2f;
    [SerializeField, BoxGroup("Blue")] private int blueDamage = 1;
    [SerializeField, BoxGroup("Blue")] private AudioClip blueLaserSound;
    [SerializeField, BoxGroup("Blue")] private Material blueLaserMaterial;
    
    [SerializeField, BoxGroup("Red")] private float redSpeed = 10f;
    [SerializeField, BoxGroup("Red")] private float redLifeTime = 2f;
    [SerializeField, BoxGroup("Red")] private int redDamage = 1;
    [SerializeField, BoxGroup("Red")] private AudioClip redLaserSound;
    [SerializeField, BoxGroup("Red")] private Material redLaserMaterial;

    private float _speed;
    private float _lifeTime;
    private int _damage;
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
    }
    
    public AudioClip GetAudioClip()
    {
        return _audioClip;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime), Space.Self);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            if(enableLogging)
                Debug.Log($"Laser collided with {other.transform.parent.name}");
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Enemy"))
        {
            if(enableLogging)
                Debug.Log($"Laser hit {other.transform.name}");
            other.GetComponent<Enemy>().Hit(_damage);
            Destroy(gameObject);
        }
    }
}
