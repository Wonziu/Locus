using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private List<Transform> Muzzles;

    private void Start()
    {
        Muzzles = new List<Transform>();

        foreach (Transform child in transform)
            Muzzles.Add(child);
    }

    public void Shoot(WeaponStats ws)
    {
        int count = Mathf.Clamp(ws.BulletAmount, 0, Muzzles.Count);

        for (int i = 0; i < count; i++)
        {
            MovingObject bullet = PoolManager.Instance.GetPooledObject("bullet");
            bullet.transform.position = Muzzles[i].position;
            bullet.transform.rotation = Muzzles[i].rotation;
            bullet.GetComponent<Bullet>().SetBulletValues(ws);
        }
    }

    public void Aim(Transform target)
    {
        foreach (var m in Muzzles)
        {        
            Vector3 diff = target.position - m.transform.position;
            diff.Normalize();

            float rotZ = Mathf.Atan2(diff.y, diff.x)*Mathf.Rad2Deg;

            m.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        }
    }
}
