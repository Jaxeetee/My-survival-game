using UnityEngine;


namespace Weapons{
    public class Shotgun : Gun
    {
        public override void DoDamage()
        {
            base.DoDamage();
            Debug.Log("Shotgun");
        }

        public override void Reload()
        {
            base.Reload();
            Debug.Log("Reload Shotgun");
        }
    }
}
