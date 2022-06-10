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
    private float maxX;
    private float maxY;
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
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        game.Init(hp, score);
        time = 0;
        transform.position = new Vector3(0, -maxY, transform.position.z);
        InvokeRepeating("Attack", 0, attackInterval);
        Vector2 plyerSize = gameObject.GetComponent<BoxCollider2D>().size;
        maxX = bgRender.gameObject.transform.localScale.x / 2 - plyerSize.x / 2;
        maxY = bgRender.gameObject.transform.localScale.y / 2 - plyerSize.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        bgRender.material.SetTextureOffset("_MainTex", new Vector2(0, time / bgScrollSpeed));
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
            animatorComp.SetTrigger("Hurt");
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
