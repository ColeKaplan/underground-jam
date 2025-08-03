using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour
{
    [SerializeField] private List<Sprite> availableSprites;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("Error: No SpriteRenderer component found on this GameObject.", this);
            this.enabled = false;
            return;
        }
    }

    void Start()
    {
        AssignRandomSprite();
    }

    public void AssignRandomSprite()
    {
        if (availableSprites != null && availableSprites.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSprites.Count);
            spriteRenderer.sprite = availableSprites[randomIndex];
        }
        else
        {
            Debug.LogWarning("The 'availableSprites' list is empty. No sprite was assigned.", this);
        }
    }
}
