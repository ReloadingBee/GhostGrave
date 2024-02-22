using UnityEngine;

public class ScopeExtreme : MonoBehaviour
{
    public Weapon weapon;
    public Camera cam;

    public bool isScoped = false;
    [Range(1f, 2.5f)] public float scopeFOVMultiplier = 1.5f;
    [Range(0.5f, 2f)]public float sensitivityMultiplier = 0.5f;

    float normalFOV;
    FirstPersonLook look;
    float normalSensitivity;


    private void Start()
    {
        weapon.onRightClick.AddListener(Scope);
        normalFOV = cam.fieldOfView;
        look = cam.GetComponent<FirstPersonLook>();
        normalSensitivity = look.sensitivity;
    }

    public void Scope()
    {
        isScoped = !isScoped;
        
        if(isScoped)
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
}
