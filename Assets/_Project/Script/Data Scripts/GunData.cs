using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 1)]
public class GunData : ScriptableObject
{
    [SerializeField] public GameObject gunPrefab { get; private set; }


    [SerializeField] public float _maxDistance;
    public float maxDistance { get { return _maxDistance; } }

    [SerializeField] private float _baseDamage;
    public float baseDamage{ get { return _baseDamage; } }

    [SerializeField, Range(1, 999)] private int _bulletsPerMagazine;
    public int bulletsPerMagazine { get; private set; }
    
    [SerializeField, Range(1, 999)] private int _totalReservedAmmo;
    public int totalReservedAmmo { get; private set; }


    [SerializeField] private Vector3 _spawnPoint;
    [SerializeField] private Vector3 _spawnRotation;


}


