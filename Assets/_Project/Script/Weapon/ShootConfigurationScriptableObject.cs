using UnityEngine;

namespace WeaponSystem
{
    /// <summary>
    /// Must know the ff:
    /// If Hitscan or Projectile
    ///     If Hitscan
    ///     - show and get maxDistance
    ///     - show and get LayerMask
    ///     If Projectile
    ///     - show and get bulletPrefab. Must have a Bullet Script
    ///     
    /// float fireRate
    /// Vector3 Spread
    /// 
    /// </summary>

    [CreateAssetMenu(fileName = "ShootConfig", menuName = "Guns/Shoot Config", order = 1)]
    public class ShootConfiguration : ScriptableObject
    {

    }
}