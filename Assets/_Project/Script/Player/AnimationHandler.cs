using UnityEngine;
/// <summary>
/// As name suggests, handles all animations
/// </summary>
public class AnimationHandler : MonoBehaviour
{
    Animator _animator;

    #region === ANIMATOR STRING HASH ===

    private int isRunningHash = Animator.StringToHash("isRunning");   

    #endregion
    //use Animator.StringToHash("name of the animation") to call the animation
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
    }
}
