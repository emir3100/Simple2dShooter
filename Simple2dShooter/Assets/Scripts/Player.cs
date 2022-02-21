using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float Speed;
	public float JumpForce;
	public bool Jump = false;
	[HideInInspector] public bool FacingRight = true; 
	
	private UnityEvent OnLandEvent;
	[SerializeField] private float horizontal;
    [SerializeField] private LayerMask whatIsGround;                         
    [SerializeField] private Transform groundCheck;                        

    const float k_GroundedRadius = .2f;
    private bool isGrounded;           
    private Rigidbody2D rb;
	
    private Vector3 m_Velocity = Vector3.zero;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal") * Speed;

		if (Input.GetButtonDown("Jump"))
			Jump = true;
	}

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, k_GroundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

		Move(horizontal * Time.fixedDeltaTime, Jump);
		Jump = false;
    }

	public void Move(float move, bool jump)
	{
		Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, 0.05f);

		if (move > 0 && !FacingRight)
		{
			Flip();
		}
		else if (move < 0 && FacingRight)
		{
			Flip();
		}

		if (isGrounded && jump)
		{
			isGrounded = false;
			rb.AddForce(new Vector2(0f, JumpForce));
		}
	}
	private void Flip()
	{
		FacingRight = !FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
