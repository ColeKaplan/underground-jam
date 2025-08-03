using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class DirtRegion : MonoBehaviour
{
    public RenderTexture maskRT;
    public Texture2D defaultMask; // optional
    private Camera maskCam;

    void Start()
    {
        maskCam = new GameObject("MaskCam_" + name).AddComponent<Camera>();
        maskCam.transform.SetParent(transform);
        maskCam.transform.localPosition = new Vector3(0, 0, -10);
        maskCam.orthographic = true;
        maskCam.orthographicSize = GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        maskCam.cullingMask = 0;
        maskCam.backgroundColor = Color.black;
        maskCam.clearFlags = CameraClearFlags.SolidColor;
        maskCam.targetTexture = maskRT;
        maskCam.enabled = false;
    }

    public void DigAt(Vector2 worldPos, Texture2D brush, float brushSize)
    {
        RenderTexture.active = maskRT;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, maskRT.width, 0, maskRT.height);

        Vector2 uv = WorldToMaskUV(worldPos);
        Rect rect = new Rect(uv.x - brushSize / 2, uv.y - brushSize / 2, brushSize, brushSize);

        Graphics.DrawTexture(rect, brush);
        GL.PopMatrix();
        RenderTexture.active = null;
    }

    Vector2 WorldToMaskUV(Vector2 worldPos)
    {
        var bounds = GetComponent<SpriteRenderer>().bounds;
        float u = Mathf.InverseLerp(bounds.min.x, bounds.max.x, worldPos.x) * maskRT.width;
        float v = Mathf.InverseLerp(bounds.min.y, bounds.max.y, worldPos.y) * maskRT.height;
        return new Vector2(u, v);
    }
}
