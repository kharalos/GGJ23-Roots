using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public GameObject laser;
    public float blueFireRate = 0.5f;
    public float redFireRate = 0.5f;
    private Transform _firePoint;
    private float _blueNextFire = 0.0f;
    private float _redNextFire = 0.0f;

    private void Start()
    {
        _firePoint = transform;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > _blueNextFire)
        {
            _blueNextFire = Time.time + blueFireRate;
            Shoot(LaserType.Blue);
        }
        else if (Input.GetButton("Fire2") && Time.time > _redNextFire)
        {
            _redNextFire = Time.time + redFireRate;
            Shoot(LaserType.Red);
        }
    }

    private void Shoot(LaserType laserType)
    {
        var laserInstance = Instantiate(laser, _firePoint.position, _firePoint.rotation);
        var laserComp = laserInstance.GetComponent<Laser>();
        laserComp.Initialize(laserType);
    }
}
