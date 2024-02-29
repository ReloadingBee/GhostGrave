using UnityEngine;

public class ScopeExtreme : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Multiplier for the field of view when scoped.")]
    [SerializeField] [Range(1f, 2.5f)] float FOVMultiplier = 1.5f;

    [Tooltip("Multiplier for the sensitivity when scoped.")]
    [SerializeField] [Range(0.5f, 2f)] float sensitivityMultiplier = 0.5f;
    
    [Tooltip("Speed of the zoom animation in seconds.")]
    [SerializeField] [Range(0f, 0.2f)] float animationSpeedSeconds = 0.5f;

    [Space(10)]
    [Tooltip("The mode of scoping: toggle or hold.")]
    [SerializeField] ScopingMode scopeMode = ScopingMode.toggle;

    [Header("Components")]
    Weapon weapon;
    Camera cam;

    [Header("Other")]
    bool isScoped;
    float normalFOV;
    float normalSensitivity;
    FirstPersonLook look;
    float zoomValue;
    float targetZoomValue;

    enum ScopingMode
    {
        hold,
        toggle
    }

    void Start()
    {
        cam = Camera.main;
        weapon = gameObject.GetComponent<Weapon>();
        if (weapon != null)
        {
            weapon.onRightClick.AddListener(Scope);

            if (cam != null)
            {
                normalFOV = cam.fieldOfView;
                look = cam.GetComponent<FirstPersonLook>();
                normalSensitivity = look.sensitivity;
                zoomValue = normalFOV;
                targetZoomValue = normalFOV;
                return;
            }
            Debug.LogError("Camera is null");
            return;
        }
        Debug.LogError("Parent object does not have a Weapon component.");
    }

    public void Scope()
    {
        switch (scopeMode)
        {
            case ScopingMode.toggle:
                isScoped = !isScoped;
                break;
            case ScopingMode.hold:
                isScoped = true;
                break;
        }
        targetZoomValue = isScoped ? normalFOV / FOVMultiplier : normalFOV;
    }

    void Update()
    {
        zoomValue = Mathf.Lerp(zoomValue, targetZoomValue, Time.deltaTime / animationSpeedSeconds);
        cam.fieldOfView = zoomValue;
        
        look.sensitivity = isScoped ? normalSensitivity * sensitivityMultiplier : normalSensitivity;
        
        if (scopeMode == ScopingMode.hold && !Input.GetMouseButton(1))
        {
            isScoped = false;
            targetZoomValue = normalFOV;
        }
    }
}
