using UnityEngine;

public class HandleCursorVisibility : MonoBehaviour
{
    // Used to handle cursor visibility
    public bool ShowCursor = true;

    public bool AlwaysShowCursor = false;

    // Handles cursor locked state
    public CursorLockMode lockMode = CursorLockMode.Confined;

    void Start()
    {
        // Handles cursor visibility
        Cursor.visible = ShowCursor;

        // Handles if cursor is locked or free
        Cursor.lockState = lockMode;
    }

    private void Update()
    {
        if (Cursor.visible == false & AlwaysShowCursor)
        {
            // Always shows cursor if AlwaysShowCursor is ticked
            Cursor.visible = true;
        }
    }

}
