using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class Dig : MonoBehaviour
{
    private Rigidbody2D rb;
    public ContactFilter2D groundFilter; // Set this up in the Inspector to filter by "Ground" layer

    private Collider2D moleCollider;
    public Collider2D[] colliderArc = new Collider2D[5]; // Adjust size as needed
    private Collider2D[] results = new Collider2D[5];
    public GameObject startPos;
    public GameObject endPos;
    public Tilemap tilemap;

    public AudioClip digSfx;

    private float timer = 1000; // Guarantee first dig triggers it.

    void Awake()
    {
    }
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //     {
    //         Tilemap tilemap = other.GetComponent<Tilemap>();
    //         if (tilemap != null)
    //         {

    //         }
    //     }
    // }




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moleCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // int count = moleCollider.Overlap(groundFilter, results);

        // for (int i = 0; i < count; i++)
        // {
        //     print("collider found: " + results[i].name);
        //     Tilemap tilemap = results[i].GetComponent<Tilemap>();
        //     if (tilemap != null)
        //     {
        //         // Dig out tile at mole's center
        //         Vector3Int cellPos = tilemap.WorldToCell(transform.position);
        //         tilemap.SetTile(cellPos, null);
        //     }
        // }
    }

    void FixedUpdate()
    {
        foreach (Collider2D collider in colliderArc)
        {
            int count = collider.Overlap(groundFilter, results);

            for (int i = 0; i < count; i++)
            {
                Collider2D hit = results[i];

                // Check if hit object or its parent is the player
                Transform hitTransform = hit.transform;
                bool isPlayer = hitTransform.CompareTag("Player") || 
                                (hitTransform.parent != null && hitTransform.parent.CompareTag("Player"));

                if (isPlayer)
                    continue; // Skip if it's the player
                Vector3Int cellPos = tilemap.WorldToCell(collider.transform.position);
                tilemap.SetTile(cellPos, null);
                if (timer >= digSfx.length)
                {
                    AudioManager.Instance.PlaySFX(digSfx, false, 0.5f);
                    timer = 0;
                }
            }
        }
        timer += Time.deltaTime;
    }


}
