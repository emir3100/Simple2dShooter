using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float BulletForce = 20f;
    public float FireRate = 2f;
    public AudioClip BulletSound;
    public Rigidbody2D rb;
    private Vector2 shootDirection;
    private float nextTimeToShoot = 0f;

    private void Update()
    {
        if (Time.time >= nextTimeToShoot)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
                nextTimeToShoot = Time.time + 1f / FireRate;
            }
        }

        //Shoot direction
        shootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shootDirection = (shootDirection - rb.position).normalized;

        //Weapon rotation

        var angle = Mathf.Atan2(shootDirection.x, shootDirection.y) * -Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        transform.rotation = Quaternion.Euler(0f, 0f, angle-90);

        if (rb.gameObject.GetComponent<Player>().FacingRight)
            FlipWeapon(true);
        else
            FlipWeapon(false);
        
    }

    private void Shoot()
    {
        GameManager.Instance.AudioSource.PlayOneShot(BulletSound);
        var bulletInstance = Instantiate(BulletPrefab.GetComponent<Rigidbody2D>(), transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
        bulletInstance.velocity = new Vector2(shootDirection.x * BulletForce, shootDirection.y * BulletForce);
    }

    private void FlipWeapon(bool RightDir)
    {
        var sprite = GetComponent<SpriteRenderer>();
        if (RightDir)
        {
            sprite.flipY = true;
            sprite.flipX = true;
        }
        else
        {
            sprite.flipY = false;
            sprite.flipX = false;
        }
    }
}
