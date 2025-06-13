using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float offset;
    public GameObject bullet;
    public Transform shotPoint;

    public Light2D muzzleFlashLight;
    public float flashIntensity;
    public float flashDuration;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public int ammoStock;
    public int ammo;
    public int ammoMax = 30;
    public Text ammoDisplay;

    private Vector2 move;
    private bool facingLeft = true;

    void Update()
    {
        Position();

        if(ammo > 0)
            Shot();

        Reload();

        ammoDisplay.text = ammo + "/" + ammoStock;
    }

    private void Position()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        if (facingLeft == false && Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x < 0)
            Flip();
        else if (facingLeft == true && Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x > 0)
            Flip();
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        Scaler.y *= -1;
        transform.localScale = Scaler;
    }

    public void TakeAmmo(int ammoValue)
    {
        ammoStock += ammoValue;
    }

    private void Reload()
    {
        if(ammo < ammoMax && ammoStock > 0 && Input.GetKeyDown(KeyCode.R))
        {
            int ammoNeed = ammoMax - ammo;
            ammo += Mathf.Min(ammoNeed, ammoStock);
            ammoStock -= Mathf.Min(ammoNeed, ammoStock);
        }
    }

    private void Shot()
    {
        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(bullet, shotPoint.position, transform.rotation);
                timeBtwShots = startTimeBtwShots;
                ammo--;

                StartCoroutine(MuzzleFlash());
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
    private System.Collections.IEnumerator MuzzleFlash()
    {
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.intensity = flashIntensity;
            yield return new WaitForSeconds(flashDuration);
            muzzleFlashLight.intensity = 0;
        }
    }
}
