using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Button shootButton;

    public Transform firePoint;
    private float bulletSpeed = 20f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Shoot();
    }

    private void Shoot()
    {
        
            if (bulletPrefab == null || firePoint == null) return;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Transform playerTransform = GetComponent<Transform>();

            // Determine direction based on player facing direction (flipX)
            float direction = playerTransform.localScale.x > 0 ? 1 : -1;
            rb.linearVelocity = new Vector2(direction * bulletSpeed, 0);

             Vector3 bulletScale = bullet.transform.localScale;
            bulletScale.x = Mathf.Abs(bulletScale.x) * direction; // Ensures it's flipped correctly
            bullet.transform.localScale = bulletScale;
        
    }
}
