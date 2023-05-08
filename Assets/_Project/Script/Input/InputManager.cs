using PlayerSystem.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : Singleton<InputManager>
    {
        [Tooltip("Selected Map will be active. It is impossible to enable 2 maps")]
        [SerializeField]
        private ActionMapsType defaultMap;




    }
}