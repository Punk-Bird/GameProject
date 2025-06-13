using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    Transform playerPosit;
    public float stopDistance;
    private bool isPlayerInSight = false;

    public int health;

    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public int damage;
    public float damageDistance;

    private Animator anim;
    private PlayerController player;

    private bool facingLeft = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerPosit = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeBtwAttack = startTimeBtwAttack;
    }

    void Update()
    {
        var distance = Vector2.Distance(playerPosit.position, transform.position);

        if(isPlayerInSight)
            MoveTowardsPlayer(distance);

        Attack(distance);
    }

    private void MoveTowardsPlayer(float distance)
    {
        if (distance >= stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPosit.position, speed * Time.deltaTime);
            anim.SetBool("Enemy_Running", true);
        }
        else
        {
            anim.SetBool("Enemy_Running", false);
        }

        if (facingLeft == false && transform.position.x > playerPosit.position.x)
            Flip();
        else if (facingLeft == true && transform.position.x < playerPosit.position.x)
            Flip();
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Destroy(gameObject);
    }

    private void Attack(float distance)
    {
        if (timeBtwAttack <= 0 && distance <= damageDistance)
        {
            timeBtwAttack = startTimeBtwAttack;
            anim.SetTrigger("Enemy_Attack");
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void OnEnemyAttack()
    {
        player.health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInSight = true;
    }
}