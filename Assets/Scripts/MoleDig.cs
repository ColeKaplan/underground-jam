using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MoleDig : MonoBehaviour
{
    public RenderTexture maskRT;
    public Texture2D brush;
    public float brushSize = 32f;
    public float moveSpeed = 3f;

    private InputAction move;
    private Vector2 moveInput;
    private bool isDigging;
    private Camera maskCam;
    private Rigidbody2D rb;

    void Awake()
    {
        move = InputSystem.actions.FindAction("Move");
        isDigging = true;
    }

    void OnEnable()
    {
        move.Enable();
    }

    void OnDisable()
    {
        move.Disable();
    }



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject camObj = new GameObject("MaskCam");
        maskCam = camObj.AddComponent<Camera>();
        maskCam.orthographic = true;
        maskCam.orthographicSize = 5;
        maskCam.clearFlags = CameraClearFlags.SolidColor;
        maskCam.backgroundColor = Color.black;
        maskCam.cullingMask = 0;
        maskCam.targetTexture = maskRT;
        maskCam.enabled = false;
    }

    void Update()
    {
        moveInput = move.ReadValue<Vector2>();
        gameObject.transform.position += (Vector3)moveInput * moveSpeed * Time.deltaTime;

        if (isDigging)
        {
            DrawToMask(transform.position);
        }
    }

    void FixedUpdate()
    {
    }

    void DrawToMask(Vector2 worldPos)
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
        Vector2 min = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector2 max = Camera.main.ViewportToWorldPoint(Vector3.one);

        float u = Mathf.InverseLerp(min.x, max.x, worldPos.x) * maskRT.width;
        float v = Mathf.InverseLerp(min.y, max.y, worldPos.y) * maskRT.height;
        return new Vector2(u, v);
    }
}
