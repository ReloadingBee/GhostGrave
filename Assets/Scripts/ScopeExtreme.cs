using UnityEngine;

public class ScopeExtreme : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(1f, 2.5f)] float scopeFOVMultiplier = 1.5f;
    [SerializeField] [Range(0.5f, 2f)] public float sensitivityMultiplier = 0.5f;
    [SerializeField] scopingMode scopeMode = scopingMode.toggle;

    [Header("Components")]
    Weapon weapon;
    Camera cam;

    bool isScoped = false;

    float normalFOV;
    float normalSensitivity;
    FirstPersonLook look;

    enum scopingMode
    {
        hold,
        toggle
    }

    private void Start()
    {
        cam = Camera.main;
        weapon = gameObject.transform.parent.gameObject.GetComponent<Weapon>();
        if (weapon == null) Debug.LogWarning("Parent object does not have a Weapon component.");
        else
        {
            weapon.onRightClick.AddListener(Scope);
            normalFOV = cam.fieldOfView;
            look = cam.GetComponent<FirstPersonLook>();
            normalSensitivity = look.sensitivity;
        }
    }

    public void Scope()
    {
        if(scopeMode == scopingMode.toggle)
        {
            isScoped = !isScoped;
        }
        else
        {
            isScoped = true;
        }

        UpdateScope();
    }

    void UpdateScope()
    {
        if (isScoped)
        {
            cam.fieldOfView = normalFOV / scopeFOVMultiplier;
            look.sensitivity = normalSensitivity * sensitivityMultiplier;
        }
        else
        {
            cam.fieldOfView = normalFOV;
            look.sensitivity = normalSensitivity;
        }
    }

    private void Update()
    {
        if (scopeMode != scopingMode.hold) return;
        if(Input.GetMouseButtonUp(1))
        {
            isScoped = false;
            UpdateScope();
        }
    }
}
