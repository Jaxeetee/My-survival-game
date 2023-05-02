using UnityEngine;

namespace WeaponSystem
{
    /// <summary>
    /// Must have the ff:
    /// TrailConfig
    /// ShootConfig
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "GunConfig", menuName = "Guns/Gun Config", order = 0)]
    public class GunConfigurationScriptableObject : ScriptableObject
    {
        [SerializeField] public string GunName { get; private set; }
        [SerializeField] public GameObject GunPrefab { get; private set; }
        [SerializeField] public Vector3 SpawnPoint { get; private set; }
        [SerializeField] public Vector3 SpawnRotation { get; private set; }

        [SerializeField] public ShootConfiguration ShootConfiguration { get; private set; }
        [SerializeField] public TrailConfigurationScriptableObject TrailConfiguration { get; private set; }


        // Must contain the Following
        // - Shoot()
        // - Reload()
        // - Is Using Projectile or Hitscan()
        // - Has Different Firemodes

    }
}