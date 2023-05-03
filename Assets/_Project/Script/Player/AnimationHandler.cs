using UnityEngine;


/// <summary>
/// As name suggests, handles all animations both for Player and Enemies.
/// Script must be attached where the Animator component is attached
/// </summary>

[RequireComponent (typeof(Animator))]
public class AnimationHandler : MonoBehaviour
{
    private static Animator _animator;

    //use Animator.StringToHash("name of the animation") to call the animation
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public static void SetBool(string animParam,  bool value)
    {
        int hashedParam = Animator.StringToHash(animParam);
        _animator.SetBool(hashedParam, value);
    }

    public static void SetTrigger(string animParam)
    {
        int hashedParam = Animator.StringToHash(animParam);
        _animator.SetTrigger(hashedParam);
    }

    public static void SetFloat(string animParam, float value)
    {
        int hashedParam = Animator.StringToHash(animParam);
        _animator.SetFloat(hashedParam, value);
    }
}
