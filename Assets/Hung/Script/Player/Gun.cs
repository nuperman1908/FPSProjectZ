using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Gun Setting
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public ParticleSystem flash;
    public GameObject impactEffect;
    public GameObject bulletObject;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }
    
    void Shoot()
    {
        flash.Play();
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            
            GameObject impaceGo = Instantiate(impactEffect, hit.point,Quaternion.LookRotation(hit.normal));
            Destroy(impaceGo,2f);

            //shoot a bullet and destroy it after 2s
            GameObject bullet = Instantiate(bulletObject, flash.transform.position,Quaternion.LookRotation(hit.normal));
            Vector3 direction = (hit.point - bullet.transform.position).normalized;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(direction * 5f, ForceMode.VelocityChange);
            Destroy(bullet, 2f);

        }
    }
}
