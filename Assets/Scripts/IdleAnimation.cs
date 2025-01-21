using UnityEngine;
using DG.Tweening;

public class IdleAnimation : MonoBehaviour
{
    public float scaleAmount = 1.1f; // How much the object scales up
    public float animationDuration = 1f; // Time for one scale up or down
    public Ease animationEase = Ease.InOutSine; // Smooth easing

    private void Start()
    {
        StartIdleAnimation();
    }

    private void StartIdleAnimation()
    {
        // Start a looping scale animation
        transform.DOScale(Vector3.one * scaleAmount, animationDuration)
            .SetEase(animationEase) // Smooth in/out easing
            .SetLoops(-1, LoopType.Yoyo); // Infinite loop with Yoyo (ping-pong) effect
    }
}
