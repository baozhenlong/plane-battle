using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BigAttack
{
    public float angle;
    public int num;
}
[System.Serializable]
public struct Audio
{
    public AudioClip clip;
    public float volume;
}

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float maxX = 1.0f;
    public float maxY = 1.0f;
    public Renderer bgRender;
    public GameObject bulletPrefab;
    public Transform fireTransform;
    public float attackInterval = 0.5f;
    public float bgScrollSpeed = 5.0f;
    public BigAttack bigAttack = new BigAttack();
    public int hp = 10;
    public Game game;

    public float scoreRatio = 100.0f;
    private float score = 0.0f;
    public AudioSource audioSourceComp;
    public Audio attackAudio;
    public Audio gameOverAudio;
    public Animator animatorComp;

    // Start is called before the first frame update
    void Start()
    {
        game.Init(hp, score);
        transform.position = new Vector3(0, -maxY, transform.position.z);
        InvokeRepeating("Attack", 0, attackInterval);
        // animatorComp
    }

    // Update is called once per frame
    void Update()
    {
        bgRender.material.SetTextureOffset("_MainTex", new Vector2(0, Time.time / bgScrollSpeed));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BigAttack();
        }
        score += scoreRatio * Time.time;
        game.ChangeScore(score);
    }

    private void OnMouseDrag()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = transform.position.z;
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pos.y = Mathf.Clamp(pos.y, -maxY, maxY);
        transform.position = pos;
    }

    private void Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab, fireTransform.position, fireTransform.rotation);
        bullet.name = GameEnum.PlayerBullet;
        audioSourceComp.PlayOneShot(attackAudio.clip, attackAudio.volume);
    }

    private void BigAttack()
    {
        int min = -(bigAttack.num / 2);
        int max = bigAttack.num / 2 + 1;
        for (int i = min; i < max; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, fireTransform.position, fireTransform.rotation);
            bullet.name = GameEnum.PlayerBullet;
            bullet.transform.Rotate(0, 0, bigAttack.angle * i);
        }
    }

    public void hurt(int damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            if (hp <= 0)
            {
                game.ChangeHp(0);
                game.GameOver();
                audioSourceComp.PlayOneShot(gameOverAudio.clip, gameOverAudio.volume);
            }
            else
            {
                game.ChangeHp(hp);
            }
        }
    }
}
