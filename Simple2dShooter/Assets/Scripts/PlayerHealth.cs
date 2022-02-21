using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 100f;
    public Slider Slider;

    public float currentHealth;
    [HideInInspector]
    public bool isDead = false;
    public AudioClip DeadSound;
    public GameObject HitEffect;
    public AudioClip HitSound;

    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    private void Update()
    {
        Slider.value = Mathf.Lerp(Slider.value, currentHealth, 5 * Time.deltaTime);

        if (currentHealth < 100f && currentHealth > 0f)
            StartCoroutine("Regenerate");

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(7f);
        currentHealth += 5 * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        GameManager.Instance.AudioSource.PlayOneShot(HitSound);
    }

    private void Die()
    {
        GameManager.Instance.AudioSource.PlayOneShot(DeadSound);
        currentHealth = 0;
        Slider.value = 0;
        isDead = true;
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePosition;
        this.gameObject.GetComponent<Player>().enabled = false;
        this.gameObject.transform.GetChild(0).GetComponent<Shooting>().enabled = false;
        this.gameObject.GetComponent<PlayerHealth>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(HitEffect, transform.position, Quaternion.identity);
    }

    public void SetDeadPosition()
    {
        float x = transform.position.x;
        transform.position = new Vector2(x, -1.77f);
    }
}
