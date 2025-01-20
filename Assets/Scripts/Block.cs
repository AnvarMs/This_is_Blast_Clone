using DG.Tweening;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Color blockColor;

    public void Initialize(Color color)
    {
        blockColor = color;
        // Apply color and number visually.
        GetComponent<MeshRenderer>().material.color = blockColor;
        // Add text or other indicators for numbers.
    }

    public void DestroyWithJellyEffect()
    {
        GridManager.Instance.onCubeDestroy?.Invoke();
        // Create a DoTween sequence
        Sequence jellySequence = DOTween.Sequence();

        // Add the squish and stretch animation
        jellySequence.Append(transform.DOScale(new Vector3(1.2f, 0.8f, 1.2f), 0.2f) // Squish
            .SetEase(Ease.OutQuad));

        // Add a stretch back animation
        jellySequence.Append(transform.DOScale(new Vector3(0.8f, 1.2f, 0.8f), 0.2f) // Stretch
            .SetEase(Ease.OutQuad));

        // Shrink to zero with a bounce effect
        jellySequence.Append(transform.DOScale(Vector3.zero, 0.3f) // Shrink
            .SetEase(Ease.InBack));

        // Destroy the block after the animation finishes
        jellySequence.OnComplete(() =>
        {
           gameObject.SetActive(false);
            Destroy(gameObject,3);
        });
    }
}
