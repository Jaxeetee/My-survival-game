using UnityEngine;

namespace WeaponSystem
{
    /// <summary>
    /// Must have the ff:
    /// TrailRenderer
    /// AnimationCurve
    /// Material
    /// Duration (float)
    /// MinVertexDistance (float)
    /// Gradient
    /// 
    /// Miss Distance (float)
    /// Simulation Speed (float)
    /// </summary>

    [CreateAssetMenu(fileName = "TrailConfig", menuName = "Guns/Trail Config", order = 2)]
    public class TrailConfigurationScriptableObject : ScriptableObject
    {

    }
}