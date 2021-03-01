using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickUp : MonoBehaviour
{
    public GameObject Grenade;

    private void Update()
    {
        transform.Rotate(new Vector3(30f, 45f, 60f) * Time.deltaTime);
    }
}
