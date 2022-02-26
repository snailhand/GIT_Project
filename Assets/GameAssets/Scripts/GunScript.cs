using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject shotPrefab;
    public Transform[] gunPoints;
    public float fireRate;

    bool firing;
    float fireTimer;
    Transform targetTransform;

    int gunPointIndex;

    void Update()
    {
        if (firing)
        {
            while (fireTimer >= 1 / fireRate)
            {
                SpawnShot();
                fireTimer -= 1 / fireRate;
            }

            fireTimer += Time.deltaTime;
            firing = false;
        }
        else
        {
            if (fireTimer < 1 / fireRate)
            {
                fireTimer += Time.deltaTime;
            }
            else
            {
                fireTimer = 1 / fireRate;
            }
        }
    }

    void SpawnShot()
    {
        var gunPoint = gunPoints[gunPointIndex++];
        var bullet = Instantiate(shotPrefab, gunPoint.position, gunPoint.rotation);

        bullet.GetComponent<BulletScript>().target = targetTransform;
        Destroy(bullet, 4);
        gunPointIndex %= gunPoints.Length;
    }

    public void Fire(Transform direction)
    {
        firing = true;
        targetTransform = direction;
    }
}
