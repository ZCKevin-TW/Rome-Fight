using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration = .2f;

    private SpriteRenderer rend;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        originalMaterial = rend.material;
    }

    public void StartFlash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        rend.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        rend.material = originalMaterial;
        flashRoutine = null;
    }

}
