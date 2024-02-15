using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}
