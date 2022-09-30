using UnityEngine;
using UnityEngine.InputSystem;

public class Tooltip : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    void Update()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height-1.2f;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
    }
}
