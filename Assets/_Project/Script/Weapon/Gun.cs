using UnityEngine;


namespace Weapons{
    public class Gun : Weapon
    {
        [SerializeField]
        protected bool _isUsingProjectile;

        [SerializeField]
        protected float _fallofDamageMultiplier; // Higher the value, the higher the fall of damage is

        [SerializeField]
        protected float _reservedAmmoCount;

        [SerializeField]
        protected float _totalRounds;

        [SerializeField]
        protected float _maxDistance; // must show this when _isUsingProjectile is used

        public override void DoDamage()
        {
            //Shoot Gun 
        }

        public virtual void Reload()
        {
            //Reload Gun
        }

        protected float CalculateFallOfDamage(float hitDistance)
        {
            return _baseDamage / (1 * hitDistance * _fallofDamageMultiplier);
        }
    }
}
