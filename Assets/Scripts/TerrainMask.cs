using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class TerrainMask : MonoBehaviour
{
    public RenderTexture maskTexture;
    public Texture2D readableTexture;
    public float updateRate = 0.5f;

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polyCollider;
    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();
        UpdateCollider();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > updateRate)
        {
            UpdateCollider();
            timer = 0;
        }
    }

    void UpdateCollider()
    {
        RenderTexture.active = maskTexture;

        if (readableTexture == null || 
            readableTexture.width != maskTexture.width || 
            readableTexture.height != maskTexture.height)
        {
            readableTexture = new Texture2D(maskTexture.width, maskTexture.height, TextureFormat.R8, false);
        }

        readableTexture.ReadPixels(new Rect(0, 0, maskTexture.width, maskTexture.height), 0, 0);
        readableTexture.Apply();
        RenderTexture.active = null;

        bool[,] binary = new bool[readableTexture.width, readableTexture.height];

        for (int y = 0; y < readableTexture.height; y++)
        {
            for (int x = 0; x < readableTexture.width; x++)
            {
                binary[x, y] = readableTexture.GetPixel(x, y).r < 0.5f;
            }
        }

        var paths = MarchingSquares.Generate(binary, 1f / readableTexture.width);
        polyCollider.pathCount = paths.Count;
        Debug.Log($"Updating collider with {paths.Count} paths");
        for (int i = 0; i < paths.Count; i++)
        {
            polyCollider.SetPath(i, paths[i]);
        }
    }
}
