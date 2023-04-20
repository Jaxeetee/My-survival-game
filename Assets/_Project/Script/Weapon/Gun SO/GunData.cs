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

public interface Idamageable
{
    public void damage(float damage);
}

public class Gun {
    string Name;

    protected virtual void SpawnMe()
    {
        Debug.Log("This is a gun");
    }
}

public class Pistol : Gun // INHERITANCE
{
    protected override void SpawnMe()
    {
        Debug.Log("This is a Pistol");
    }
}

public class Rifle : Gun, Idamageable // POLYMORPHISM W/ INTERFACE
{
    protected override void SpawnMe()
    {
        Debug.Log("This is a Rifle");
    }

    private void SpawnMagic() //added shit
    {
        Debug.Log("Wow Magic");
    }

    public void damage(float damage)
    {
        Debug.Log("Emotionaaaal Daaaamaage");
    }
}


