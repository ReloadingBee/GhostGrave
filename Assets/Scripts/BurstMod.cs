using UnityEngine;

public class BurstMod : MonoBehaviour
{
    public Weapon weapon;

    public bool isBurstFire;

    private void Start()
    {
        weapon.onRightClick.AddListener(BurstFire);
    }

    public void BurstFire()
    {
        isBurstFire = !isBurstFire;

        if(isBurstFire)
        {
            weapon.bulletsPerShot = 7;
            weapon.isAutoFire = false;
        }
        else
        {
            weapon.bulletsPerShot = 1;
            weapon.isAutoFire = true;
        }        
    }
}
