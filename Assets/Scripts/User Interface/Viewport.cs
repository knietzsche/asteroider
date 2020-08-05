using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Viewport : MonoBehaviour
{
    public static Vector3 screenMin;
    public static Vector3 screenMax;

    private static new Camera camera;
    private static float width;
    private static float height;

    private void Awake()
    {
        camera = GetComponent<Camera>();

        screenMin = camera.ViewportToWorldPoint(Vector3.zero);
        screenMax = camera.ViewportToWorldPoint(Vector3.one);
        width = screenMax.x - screenMin.x;
        height = screenMax.y - screenMin.y;
    }

    public static void WrapPosition(Transform transform)
    {
        var position = transform.position;

        if (transform.position.x < screenMin.x)
        {
            position += Vector3.right * width;
        }
        if (transform.position.x > screenMax.x)
        {
            position += Vector3.left * width;
        }

        if (transform.position.y < screenMin.y)
        {
            position += Vector3.up * height;
        }
        if (transform.position.y > screenMax.y)
        {
            position += Vector3.down * height;
        }

        transform.position = position;
    }

    public static bool IsOutsideViewport(Transform transform)
    {
        return transform.position.x < screenMin.x ||
            transform.position.x > screenMax.x;
    }

    public static Vector3 GetRandomPositionInside(float safeDistance)
    {
        Vector3 position;

        do
        {
            position = new Vector3(
            Random.Range(screenMin.x, screenMax.x),
            Random.Range(screenMin.y, screenMax.y),
            0);
        } while (Vector3.Distance(position, Vector3.zero) < safeDistance);

        return position;
    }

    public static Vector3 GetRandomPositionOnEdge()
    {
        return new Vector3(
            Random.Range(0, 2) == 0 ? screenMin.x : screenMax.x,
            Random.Range(screenMin.y, screenMax.y),
            0);
    }
}
