using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;
    private Rigidbody2D rb;
    private Vector2 move;

    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Gun gun;
    public Text countEnemyText;
    private int countEnemy;

    private bool facingLeft = true;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        CheckHP();
        CheckCountOfHeart();
        Move();

        CheckCountEnemy();
        countEnemyText.text = "Enemy: " + countEnemy;
    }

    private void CheckCountEnemy()
    {
        countEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (countEnemy == 0)
            SceneManager.LoadScene("Win");
    }

    private void CheckHP()
    {
        if (health < 1)
            SceneManager.LoadScene("GameOver");
    }

    private void CheckCountOfHeart()
    {
        if (health > numOfHearts)
            health = numOfHearts;

        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < Mathf.RoundToInt(health))
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
            if (i < numOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("First Aid Kit") && health < numOfHearts)
        {
            ChangeHealth(1);
            Destroy(other.gameObject);
        }

        else if(other.CompareTag("Ammo"))
        {
            gun.TakeAmmo(30);
            Destroy(other.gameObject);
        }
    }

    private void ChangeHealth(int healthValue)
    {
        health += healthValue;
    }

    private void Move()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + move.normalized * speed * Time.fixedDeltaTime);

        if (move.y == 0 && move.x == 0)
            anim.SetBool("Running", false);
        else
            anim.SetBool("Running", true);

        if (facingLeft == false && Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x < 0)
            Flip();
        else if (facingLeft == true && Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x > 0)
            Flip();
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
