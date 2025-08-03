using UnityEngine;

public class SinusoidalMovement : MonoBehaviour
{
    [SerializeField] private float displacementAmount = 0.25f;
    private Vector2 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector2 newPosition = initialPosition;
        newPosition.y = initialPosition.y + displacementAmount * Mathf.Sin(Time.time);
        transform.position = newPosition;
    }
}
