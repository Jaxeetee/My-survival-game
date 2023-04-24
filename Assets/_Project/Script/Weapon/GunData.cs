using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 1)]
public class GunData : ScriptableObject
{
    [SerializeField] public GameObject gunPrefab { get; private set; }


    [SerializeField, Min (0.1f)] private float _maxDistance;
    public float maxDistance
    {
        get => _maxDistance;
        set => _maxDistance = value;
    }

    [SerializeField, Min(0f)] private float _baseDamage;
    public float baseDamage
    {
        get => _baseDamage;
        set => _baseDamage = value;
    }

    [SerializeField, Range(1, 999)] private int _bulletsPerMagazine;
    public int bulletsPerMagazine
    {
        get => _bulletsPerMagazine;
        set => _bulletsPerMagazine = value;
    }
    
    [SerializeField, Range(1, 999)] private int _totalReservedAmmo;
    public int totalReservedAmmo
    {
        get => _totalReservedAmmo;
        set => _totalReservedAmmo = value;
    }


    [SerializeField] private Vector3 _spawnPoint;
    [SerializeField] private Vector3 _spawnRotation;


}