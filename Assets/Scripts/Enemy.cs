using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Enemy : MonoBehaviour
{
    public float[] speedRange = {
        3,6
    };
    private float speed;
    public GameObject bulletPrefab;
    public Transform fireTransform;
    public float attackInterval = 3.0f;
    public int damage = 2;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedRange[0], speedRange[1]);
        InvokeRepeating("Attack", 0, attackInterval);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }

    private void Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab, fireTransform.position, fireTransform.rotation);
        bullet.name = GameEnum.EnemyBullet;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == GameEnum.PlayerTag)
        {
            other.GetComponent<Player>().hurt(damage);
            Destroy(gameObject);
        }
        else if (other.tag == GameEnum.WallTag)
        {
            Destroy(gameObject);
        }
    }
}
