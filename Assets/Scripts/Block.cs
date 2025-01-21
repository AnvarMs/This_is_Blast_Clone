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

    public void DestroyWithJellyEffect(Vector3 impactDirection)
    {
        GridManager.Instance.onCubeDestroy?.Invoke();
        // Normalize the impact direction
        impactDirection.Normalize();

        // Calculate the squash and stretch scales based on the impact direction
        Vector3 squishScale = new Vector3(1.0f - Mathf.Abs(impactDirection.x) * 0.2f, 0.8f, 1.0f - Mathf.Abs(impactDirection.z) * 0.2f);
        Vector3 stretchScale = new Vector3(1.0f + Mathf.Abs(impactDirection.x) * 0.2f, 1.2f, 1.0f + Mathf.Abs(impactDirection.z) * 0.2f);

        // Create a DoTween sequence
        Sequence jellySequence = DOTween.Sequence();

        // Add the squish animation (Shorter duration)
        jellySequence.Append(transform.DOScale(squishScale, 0.05f) // Squish
            .SetEase(Ease.OutQuad));

        // Add the stretch animation (Shorter duration)
        jellySequence.Append(transform.DOScale(stretchScale, 0.05f) // Stretch
            .SetEase(Ease.OutQuad));

        jellySequence.Append(transform.DOScale(squishScale, 0.05f) // Squish
           .SetEase(Ease.OutQuad));

        jellySequence.Append(transform.DOScale(stretchScale, 0.05f) // Stretch
           .SetEase(Ease.OutQuad));


        jellySequence.Append(transform.DOScale(new Vector3(0.1f,0.1f,0.1f), 0.05f) // Shrink
            .SetEase(Ease.InBack));

        // Shrink to zero with a bounce effect (Shorter duration)
        jellySequence.Append(transform.DOScale(Vector3.zero, 0.05f) // Shrink
            .SetEase(Ease.InBack));

        // Rotate the object slightly for added effect (Shorter duration)
        jellySequence.Join(transform.DORotate(new Vector3(impactDirection.z * 15f, 0, -impactDirection.x * 15f), 0.2f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.OutBack));

        // Destroy the block after the animation finishes
        jellySequence.OnComplete(() =>
        {
            gameObject.SetActive(false); // Optionally disable instead of destroy
            Destroy(gameObject, 3); // Cleanup
        });
    }


}
