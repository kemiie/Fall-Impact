using UnityEngine;
using UnityEngine.EventSystems;

public class CursorLock : MonoBehaviour
{
    public static CursorLock Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance != null &&
                UIManager.Instance.IsPanelOpen())
            {
                UIManager.Instance.CloseCurrentPanel();
            }
            else
            {
                UnlockCursor();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            if (UIManager.Instance == null ||
                !UIManager.Instance.IsPanelOpen())
            {
                LockCursor();
            }
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}