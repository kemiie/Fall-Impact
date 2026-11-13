using UnityEngine;

public class CursorLock : MonoBehaviour
{
    void Start()
    {
        SetLock(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetLock(Cursor.lockState != CursorLockMode.Locked);
        }
    }

    void SetLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}