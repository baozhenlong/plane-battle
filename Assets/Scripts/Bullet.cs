using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case GameEnum.EnemyTag:
                if (name == GameEnum.PlayerBullet)
                {
                    Destroy(gameObject);
                    Destroy(other.gameObject);
                }
                break;
            case GameEnum.PlayerTag:
                if (name == GameEnum.EnemyBullet)
                {
                    other.GetComponent<Player>().hurt(damage);
                    Destroy(gameObject);
                }
                break;
            case GameEnum.WallTag:
                Destroy(gameObject);
                break;
            default:
                // 子弹和子弹相撞
                if (name != other.name)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }
}
