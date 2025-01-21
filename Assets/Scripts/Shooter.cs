using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    TMP_Text bulletCountText;
    [SerializeField]
    Image cover_img;
    public Color color;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public int bulletCount = 20;
    private int mySlot;
    private int blockCol;
    public bool isOnWar=false;
    public bool isWaitingtoFire =false;
    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        transform.GetComponent<MeshRenderer>().material.color = color;
        bulletCountText.text = bulletCount.ToString();
    }


    public void StartFight()
    {
      
        if (isOnWar) return;
        for (int i = 0; i < FightSlotManager.Instance.slots.Count; i++)
        {
            if (!FightSlotManager.Instance.slotFilld[i])
            {
                // Move to the target slot position with DoTween
                transform.DOMove(FightSlotManager.Instance.slots[i].position, 1f)
                    .OnComplete(() =>
                    {
                        // Code to execute after the animation completes
                        cover_img.color = color;    
                        isOnWar = true;
                        GridManager.Instance.onCubeDestroy.AddListener(FindMyBlock);
                        StartCoroutine(ShootProjectile());
                    });
                FightSlotManager.Instance.slotFilld[i] = true;
                mySlot = i;
                return;
            }
        }
       

    }


    private void FindMyBlock()
    {
        if (isOnWar && isWaitingtoFire)
        {

            if (FindBlocks() != null)
            {
                isWaitingtoFire =false;
                StartCoroutine(ShootProjectile());
            }
        }
    }

    IEnumerator ShootProjectile()
    {
        Transform targetPos;

        while (bulletCount > 0)
        {
            targetPos = FindBlocks();
            if (targetPos != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

                // Smoothly move the projectile to the target
                yield return MoveProjectileToTarget(projectile.transform, targetPos);

       
                Destroy(projectile); // Destroy projectile after hitting the target
                bulletCount--;
                transform.LookAt(targetPos);
                bulletCountText.text =bulletCount.ToString();
                isWaitingtoFire = false;
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                transform.rotation = Quaternion.identity; // Reset rotation if no targets are available
                isWaitingtoFire = true;
                GridManager.Instance.CheckGameOver();
                break;
            }
        }

        if (bulletCount <= 0)
        {
            FightSlotManager.Instance.slotFilld[mySlot] = false;
            transform.DOMoveX(-10, 2); // Move out of fight slot smoothly
            Destroy(gameObject, 1);
        }

        
    }



    private IEnumerator MoveProjectileToTarget(Transform projectile, Transform target)
    {
        float duration = 0.1f; // Adjust the duration
        float elapsedTime = 0f;

        Vector3 startPosition = projectile.position;
        
        while (elapsedTime < duration)
        {
           
            projectile.position = Vector3.Lerp(startPosition, target.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure the projectile reaches the exact target position
        projectile.position = target.position;

        // Trigger collision logic after reaching the target
        Block block = target.GetComponent<Block>();
        if (block != null)
        {
            _audioSource.Play();
            Vector3 impactDirection = (target.position - projectile.position);
            block.DestroyWithJellyEffect(impactDirection);
            StartCoroutine(GridManager.Instance.ApplyGravity(blockCol));
        }

        Destroy(projectile.gameObject); // Destroy the projectile after hitting the target
    }




    private Transform FindBlocks()
    {
        GameObject blockTransform;

        for (int i = 0; i < GridManager.Instance.cols; i++)
        {
            if(GridManager.Instance.GetBlock(0, i)==null)continue;
            blockTransform = GridManager.Instance.GetBlock(0, i);
            if (blockTransform != null && blockTransform.GetComponent<Block>().blockColor == color)
            {

                Debug.Log(blockTransform.gameObject.name);
                blockCol = i;
                return blockTransform.transform;

            }
        }
        
        return null;
    }

}
