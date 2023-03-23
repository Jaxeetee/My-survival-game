using UnityEngine;


namespace Weapons{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [SerializeField]
        protected float _baseDamage;

        public float baseDamage
        {
            get => _baseDamage;
            set => _baseDamage = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public abstract void DoDamage();
    }
}
