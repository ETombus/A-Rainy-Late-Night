using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public EnemyHandler handler;
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            try
            {
                handler.PlaySound(handler.thisType);
                collision.gameObject.GetComponent<Healthbar>().ReduceHealth(damage, false);
            }
            catch
            {
                Debug.Log("Player got no healthScript");
            }
        }
        Destroy(this.gameObject);
    }
}
