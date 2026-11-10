using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float parallaxStrength;
    private Vector2 TitikTengahlayar;
    private Vector3 initialPosition;

    void Start()
    {
        TitikTengahlayar = new Vector2(Screen.width / 2f, Screen.height / 2f);
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector2 offset = ((Vector2)Input.mousePosition - TitikTengahlayar) / TitikTengahlayar;
        Vector3 targetPosition = initialPosition + new Vector3(offset.x, offset.y, 0) * parallaxStrength;

        transform.position = targetPosition;
    }
}
