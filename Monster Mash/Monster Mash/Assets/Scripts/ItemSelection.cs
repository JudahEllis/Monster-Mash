using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    public GameObject correspondingPrefab;
    public Image selectionIndicator;

    private bool isSelected;

    private void Start()
    {
        selectionIndicator.enabled = false;
    }

    private void Update()
    {
        if (isSelected)
        {
            // Perform any desired visual feedback for the selected item
            // For example, you can change the color of the selectionIndicator image
            selectionIndicator.color = Color.green;
            SelectionManager.Instance.MoveIcon();
        }
        else
        {
            // Reset the visual feedback for non-selected items
            selectionIndicator.color = Color.white;
        }
    }

    public void SelectItem()
    {
        isSelected = true;
        // Set the selected prefab in a central selection manager script
        FindObjectOfType<TransformManager>().TurnOffUI();
        SelectionManager.Instance.SetSelectedPrefab(correspondingPrefab);
    }

    public void OnMouseDown()
    {
        SelectItem();
        //print("select!");
    }
}