using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Scroll Speed")]
    [SerializeField] private float scrollSpeedX = 0.5f;
    [SerializeField] private float scrollSpeedY = 0.25f;
    
    [Header("Movement Range")]
    [SerializeField] private float rangeX = 2f;
    [SerializeField] private float rangeY = 1f;

    private Vector3 startPosition;
    private float timeOffset;

    private void Start()
    {
        startPosition = transform.position;
        timeOffset = 0f;
    }

    private void Update()
    {
        timeOffset += Time.deltaTime;
        
        float offsetX = Mathf.Sin(timeOffset * scrollSpeedX) * rangeX;
        float offsetY = Mathf.Sin(timeOffset * scrollSpeedY * 0.7f) * rangeY;
        
        transform.position = startPosition + new Vector3(offsetX, offsetY, 0f);
    }
}