using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CanonManager : MonoBehaviour
{

    public static CanonManager Instance { get; private set; }
    private Vector3 baseScale = Vector3.one;
    public GameObject ExplosionParticle;
    public GameObject Parts;
    public SkinnedMeshRenderer mesh;
    public Animator animator;
    public Collider capCollider;
    public CinemachineCollisionImpulseSource impulseSource;
    public int Life = 20;
    public bool dead = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
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
            dead = true;
            ExplosionParticle.gameObject.SetActive(true);

            animator.SetBool("Dying", true);
            capCollider.enabled = false;
          

        }
    }

    public void PauseTheGame()
    {
        StartCoroutine(WaitBeforePause());
    }

    public IEnumerator WaitBeforePause()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
    }
}
