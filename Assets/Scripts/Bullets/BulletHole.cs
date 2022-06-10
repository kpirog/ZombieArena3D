using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletHole : MonoBehaviour
{
    [SerializeField] private List<Sprite> holeSprites;
    [SerializeField] private float lifeTime = 7f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = holeSprites[Random.Range(0, holeSprites.Count - 1)];
    }
    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }
}
