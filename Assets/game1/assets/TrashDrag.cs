using UnityEngine;

public class TrashDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool isOverBin = false;
    private TrashBin currentBin;

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        if (isOverBin && currentBin != null)
        {
            currentBin.HandleTrashDrop(gameObject);
        }
        else
        {
            Debug.Log("Trash dropped outside of a bin.");
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<TrashBin>(out TrashBin bin))
        {
            isOverBin = true;
            currentBin = bin;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<TrashBin>(out TrashBin bin))
        {
            if (currentBin == bin)
            {
                isOverBin = false;
                currentBin = null;
            }
        }
    }
}
