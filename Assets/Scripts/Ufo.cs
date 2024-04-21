using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    void Update()
    {
        //move the ufo
        transform.Translate(Vector3.right * Time.deltaTime * 2);
        //shoot bullets
        if (Random.Range(0, 100) < 1)
        {
            GameObject bullet = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
        }
    }
}
