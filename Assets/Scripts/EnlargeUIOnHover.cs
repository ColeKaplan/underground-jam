using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnlargeUIOnHover : MonoBehaviour
{
    [SerializeField] private Vector3 hoverScale = new Vector3(1.5f, 1.5f, 1.5f);
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void _OnHover()
    {
        transform.localScale = hoverScale;
    }

    public void _OnHoverExit()
    {
        transform.localScale = originalScale;
    }
}
