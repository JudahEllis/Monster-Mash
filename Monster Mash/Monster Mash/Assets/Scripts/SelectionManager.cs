using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    private GameObject selectedPrefab;

    // private GameObject icon;

    private void Awake()
    {
        //icon = GameObject.FindGameObjectWithTag("DragIcon");
        //icon.SetActive(false);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetSelectedPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;

        //icon.SetActive(prefab != null);
    }

    // Use this selectedPrefab in other parts of your game as needed
    // For example, you can instantiate it in the 3D environment based on player input
    public void InstantiateSelectedPrefab(Vector3 position)
    {
        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, position, Quaternion.identity);
        }
    }

    public GameObject GetSelectedPrefab()
    {
        return selectedPrefab;
    }

    public void MoveIcon()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; // Set the z-coordinate according to the camera's position

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Canvas canvas = FindObjectOfType<Canvas>();
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

        Vector2 offset = new Vector2(canvasSize.x / -2f, canvasSize.y / -2f);

        //icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(viewportPosition.x * canvasSize.x, viewportPosition.y * canvasSize.y) + offset;
    }
}