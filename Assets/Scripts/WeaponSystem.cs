using System;
using UnityEngine;

public enum PrimaryWeapon
{
    None,
    BlueLaser,
    RedLaser,
    Ballistic
}

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private LayerMask weaponTrackerLayerMask;
    [SerializeField] private LayerMask autoTargetLayerMask;
    [SerializeField] private float autoTargetRadius = 10f;
    [SerializeField] private float autoTargetMaxDistance = 100f;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private PrimaryWeapon primaryWeapon;
    [SerializeField] private PrimaryWeapon[] primaryWeaponInventory;
    
    public GameObject laser;
    public float blueFireRate = 0.5f;
    public float redFireRate = 0.5f;
    
    private Transform _firePoint;
    private float _nextFire = 0.0f;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _firePoint = transform;
        
        SetPrimaryWeapon(primaryWeaponInventory[0]);
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > _nextFire)
        {
            switch (primaryWeapon)
            {
                case PrimaryWeapon.BlueLaser:
                    _nextFire = Time.time + blueFireRate;
                    Shoot(LaserType.Blue);
                    break;
                case PrimaryWeapon.RedLaser:
                    _nextFire = Time.time + redFireRate;
                    Shoot(LaserType.Red);
                    break;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var currentIndex = 0;
            for (var i = 0; i < primaryWeaponInventory.Length; i++)
            {
                if (primaryWeaponInventory[i] == primaryWeapon)
                {
                    currentIndex = i;
                    break;
                }
            }

            currentIndex++;
            if (currentIndex >= primaryWeaponInventory.Length)
            {
                currentIndex = 0;
            }
            SetPrimaryWeapon(primaryWeaponInventory[currentIndex]);
        }

        if (Physics.SphereCast(_camera.transform.position, autoTargetRadius, _camera.transform.forward,
                out var sphereHit, autoTargetMaxDistance, autoTargetLayerMask))
        {
            transform.LookAt(sphereHit.point);
            //convert lookat to lerp
            
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(sphereHit.point), 0.1f);
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }

        // raycast and get screen position of the hit point
        if (Physics.Raycast(transform.position, transform.forward, out var hit, Mathf.Infinity, weaponTrackerLayerMask))
        {
            var screenPoint = _camera.WorldToViewportPoint(hit.point);
            var targetName = hit.transform.name;
            var targetType = TrackedType.Neutral;
            
            if (hit.transform.TryGetComponent(out HitInfo info))
            {
                targetType = info.type;
                targetName = info.name;
            }
            
            hudManager.SetWeaponTrackingInfo(screenPoint, targetType, targetName, hit.distance);
            Debug.Log("Weapon is tracking: " + targetName);
        }
        else
        {
            hudManager.SetWeaponTrackingInfo(Vector2.one * 0.5f, TrackedType.None);
        }
    }

    private void SetPrimaryWeapon(PrimaryWeapon weapon)
    {
        primaryWeapon = weapon;
        switch (primaryWeapon)
        {
            case PrimaryWeapon.None:
                autoTargetRadius = 0f;
                autoTargetMaxDistance = 0f;
                break;
            case PrimaryWeapon.BlueLaser:
                autoTargetRadius = 10f;
                autoTargetMaxDistance = 100f;
                break;
            case PrimaryWeapon.RedLaser:
                autoTargetRadius = 30f;
                autoTargetMaxDistance = 300f;
                break;
        }
    }

    private void Shoot(LaserType laserType)
    {
        var laserInstance = Instantiate(laser, _firePoint.position, _firePoint.rotation);
        var laserComp = laserInstance.GetComponent<Laser>();
        laserComp.Initialize(laserType);
        audioSource.PlayOneShot(laserComp.GetAudioClip());
    }
}
