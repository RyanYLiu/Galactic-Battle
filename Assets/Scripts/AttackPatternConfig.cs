using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern Config")]
public class AttackPatternConfig : ScriptableObject
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileFiringSpeed = 0.5f;
    [SerializeField] Quaternion firingAngle;
    [SerializeField] Vector3 laserOffset;
    [SerializeField] Vector2 velocity;

    public GameObject GetLaser()
    {
        return laserPrefab;
    }

    public float GetProjectileFiringSpeed()
    {
        return projectileFiringSpeed;
    }

    public Quaternion GetFiringAngle()
    {
        return firingAngle;
    }

    public Vector3 GetLaserOffset()
    {
        return laserOffset;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }
}
