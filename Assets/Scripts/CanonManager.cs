using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanonManager : MonoBehaviour
{
    private Vector3 baseScale = Vector3.one;
    public GameObject ExplosionParticle;
    public GameObject Parts;
    public SkinnedMeshRenderer mesh;
    public Collider capCollider;
    public int Life = 20;
    // Start is called before the first frame update
    private void Start()
    {
        baseScale = transform.localScale;
    }

    public void OnHit()
    {
        Life--;
        transform.DOKill();
        transform.localScale = baseScale;
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 1, 0.2f);
        if (Life < 0)
        {

            ExplosionParticle.gameObject.SetActive(true);
            mesh.enabled = false;
            Parts.SetActive(false);
            capCollider.enabled = false;
            StartCoroutine(WaitBeforePause());
        }
    }

    public IEnumerator WaitBeforePause()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
    }
}
