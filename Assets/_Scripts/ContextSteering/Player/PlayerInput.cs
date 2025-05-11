using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;
    public UnityEvent OnDash;

    public UnityEvent OnSpeedBoosterConsumed;
    public UnityEvent OnStrengthBoosterConsumed;
    public UnityEvent OnHealthBoosterConsumed;

    private void Start()
    {
        
    }

    private void Update()
    {
        OnMovementInput?.Invoke(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        OnPointerInput?.Invoke(GetPointerInput());
        if (Input.GetMouseButtonDown(0))
            OnAttack?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space))
            OnDash?.Invoke();

        if (Input.GetKeyDown(KeyCode.Q))
            OnSpeedBoosterConsumed?.Invoke();

        if (Input.GetKeyDown(KeyCode.C))
            OnStrengthBoosterConsumed?.Invoke();

        if (Input.GetKeyDown(KeyCode.E))
            OnHealthBoosterConsumed?.Invoke();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("RoomContent");
        }
            
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
