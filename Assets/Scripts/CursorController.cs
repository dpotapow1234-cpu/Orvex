using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Start()
    {
        // Скрыть курсор
        Cursor.visible = false;

        // Заблокировать курсор в центре экрана (полезно для управления камерой в играх от 1-го/3-го лица)
        Cursor.lockState = CursorLockMode.Locked;
    }
}
