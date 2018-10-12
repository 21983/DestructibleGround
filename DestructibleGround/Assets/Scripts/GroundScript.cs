using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GroundScript : MonoBehaviour
{
    public Texture2D baseTexture;
    Texture2D cloneTexture;
    SpriteRenderer sr;

    PolygonCollider2D polygonCollider;

    float widthWorld, heightWorld;
    int widthPixel, heightPixel;

    public float WidthWorld
    {
        get
        {
            if (widthWorld == 0)
                widthWorld = sr.bounds.size.x;
            return widthWorld;
        }

    }
    public float HeightWorld
    {
        get
        {
            if (heightWorld == 0)
                heightWorld = sr.bounds.size.y;
            return heightWorld;
        }

    }
    public int WidthPixel
    {
        get
        {
            if (widthPixel == 0)
                widthPixel = sr.sprite.texture.width;

            return widthPixel;
        }
    }
    public int HeightPixel
    {
        get
        {
            if (heightPixel == 0)
                heightPixel = sr.sprite.texture.height;

            return heightPixel;
        }
    }

    void Start()
    {
        cloneTexture = new Texture2D(baseTexture.width, baseTexture.height, TextureFormat.ARGB32, true);
        sr = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        GenerateSprite();
 
        UpdateTexture();
        GenerateSprite();
    }
    
    public void GenerateSprite()
    {
        int mipCount = Mathf.Min(3, baseTexture.mipmapCount);

        for (int mip = 0; mip < mipCount; ++mip)
        {
            Color[] cols = baseTexture.GetPixels(mip);
            cloneTexture.SetPixels(cols, mip);
        }
        cloneTexture.Apply(true);
        UpdateTexture();
    }


    void MakeAHole(CircleCollider2D col)
    {
        Vector2Int c = World2Pixel(col.bounds.center);
        int r = Mathf.RoundToInt(col.bounds.size.x * WidthPixel / WidthWorld);

        int px, nx, py, ny, d;
        for (int i = 0; i <= r; i++)
        {
            d = Mathf.RoundToInt(Mathf.Sqrt(r * r - i * i));
            for (int j = 0; j <= d; j++)
            {
                px = c.x + i;
                nx = c.x - i;
                py = c.y + j;
                ny = c.y - j;

                cloneTexture.SetPixel(px, py, new Color32(0, 0, 0, 0));
                cloneTexture.SetPixel(nx, py, new Color32(0, 0, 0, 0));
                cloneTexture.SetPixel(px, ny, new Color32(0, 0, 0, 0));
                cloneTexture.SetPixel(nx, ny, new Color32(0, 0, 0, 0));
            }
        }
        cloneTexture.Apply();
        UpdateTexture();

        UpdateCollider();
    }

    public void UpdateCollider()
    {
        for (int i = 0; i < polygonCollider.pathCount; i++) polygonCollider.SetPath(i, null);
        polygonCollider.pathCount = sr.sprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            path.Clear();
            sr.sprite.GetPhysicsShape(i, path);
            polygonCollider.SetPath(i, path.ToArray());
        }
    }

    void UpdateTexture()
    {
        sr.sprite = Sprite.Create(cloneTexture,
                            new Rect(0, 0, cloneTexture.width, cloneTexture.height),
                            new Vector2(0.5f, 0.5f),
                            50f
                            );
    }

    Vector2Int World2Pixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;

        var dx = (pos.x - transform.position.x);
        var dy = (pos.y - transform.position.y);

        v.x = Mathf.RoundToInt(0.5f * WidthPixel + dx * (WidthPixel / WidthWorld));
        v.y = Mathf.RoundToInt(0.5f * HeightPixel + dy * (HeightPixel / HeightWorld));

        return v;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Explosion"))
            return;
        if (!collision.GetComponent<CircleCollider2D>())
            return;

        MakeAHole(collision.gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>());
        Destroy(collision.gameObject);
    }

}
