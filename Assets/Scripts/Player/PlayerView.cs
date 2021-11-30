using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public static Animator _playerAnimator;
    

    void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
    }

    public void WalkAnimation(float speed)
    {
        _playerAnimator.SetBool(LittleKinghtAnim.Moving,true);
        _playerAnimator.SetFloat(LittleKinghtAnim.Velocity, speed);
    }

    public void IdleAnimation()
    {
        _playerAnimator.SetBool(LittleKinghtAnim.Moving,false);
        _playerAnimator.SetFloat(LittleKinghtAnim.Velocity, 0f);   
    }
    
    
}
