using System.Collections.Generic;
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
                Vector3Int cellPos = tilemap.WorldToCell(collider.transform.position);
                tilemap.SetTile(cellPos, null);

            }
        }
    }


}
