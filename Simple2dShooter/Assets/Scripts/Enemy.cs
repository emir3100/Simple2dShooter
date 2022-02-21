using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float DetectionRadius = 0.45f;
    public float AttackRate = 2f;
    public int GiveDamage;
    public LayerMask PlayerLayer;
    public GameObject HitEffect;
    public bool DirRight;

    private Transform target;
    private Rigidbody2D rb;
    private float nextAttackTime = 0;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / AttackRate;
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.x < target.transform.position.x)
        {
            rb.velocity = new Vector2(MoveSpeed, 0);    
            GetComponent<SpriteRenderer>().flipX = false;
            DirRight = true;
        }
        else if (transform.position.x > target.transform.position.x)
        {
            rb.velocity = new Vector2(-MoveSpeed, 0);
            GetComponent<SpriteRenderer>().flipX = true;
            DirRight = false;
        }
    }

    private void Attack()
    {
        var hitPlayer = Physics2D.OverlapCircleAll(this.transform.position, DetectionRadius, PlayerLayer);
        var player = hitPlayer.FirstOrDefault();

        if (player != null)
        {
            player.gameObject.GetComponent<PlayerHealth>().TakeDamage(GiveDamage);
            HitPlayer(player.GetComponent<Rigidbody2D>());
        }
    }

    private void HitPlayer(Rigidbody2D rb)
    {
        rb.AddForce(new Vector2(DirRight ? 1000 : -1000, 500));
    }
}
