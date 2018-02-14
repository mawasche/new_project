using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class walk : MonoBehaviour {

	private Animator anim;
	private Rigidbody2D rigidbody;
	private bool grounded;
	const float groundedRadius = 0.2f;
	private Transform groundCheck;
	private float jumpPower = 500f;
	int flip;
	float old_h = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
		groundCheck = transform.Find("GroundCheck");
	}
	
	// Update is called once per frame
	void Update () {
		grounded = false;
		
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		rigidbody.velocity = new Vector2(h * 2, rigidbody.velocity.y);
		if (h != 0)
		{
			anim.SetFloat("Speed", Mathf.Abs(h));
			flip = ((old_h > 0 && h > 0) || (old_h < 0 && h < 0)) ? 1 : -1;
			Vector3 theScale = transform.localScale;
            theScale.x *= flip;
			if (old_h != 0 || h < 0)
            	transform.localScale = theScale;
			old_h = h;
			print(flip);
		}
    	Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                grounded = true;
        }
		if (grounded && CrossPlatformInputManager.GetButtonDown("Jump") && anim.GetBool("Ground"))
        {
    		grounded = false;
            anim.SetBool("Ground", false);
            rigidbody.AddForce(new Vector2(0f, jumpPower));
        }
        anim.SetBool("Ground", grounded);
	}

}
