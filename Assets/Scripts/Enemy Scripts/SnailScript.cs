using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailScript : MonoBehaviour {

	public float moveSpeed = 1f;
	private Rigidbody2D myBody;
	private Animator anim;

	public LayerMask playerLayer;

	private bool moveLeft;

	private bool canMove;
	private bool stunned;

	public Transform left_Collision, right_Collision, top_Collision, down_Collision;
	private Vector3 left_Collision_Pos, right_Collision_Pos;

	void Awake() {
		myBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		left_Collision_Pos = left_Collision.position;
		right_Collision_Pos = right_Collision.position;
	}

	void Start () {
		moveLeft = true;
		canMove = true;
	}

	void Update () {
		if (canMove) {
			if (moveLeft) {
				myBody.velocity = new Vector2 (-moveSpeed, myBody.velocity.y);
			} else {
				myBody.velocity = new Vector2 (moveSpeed, myBody.velocity.y);
			}
		}

		CheckCollision ();

	}

	void CheckCollision() {

		RaycastHit2D leftHit = Physics2D.Raycast (left_Collision.position, Vector2.left, 0.1f, playerLayer);
		RaycastHit2D rightHit = Physics2D.Raycast (right_Collision.position, Vector2.right, 0.1f, playerLayer);

		Collider2D topHit = Physics2D.OverlapCircle (top_Collision.position, 0.2f, playerLayer);

		if (topHit != null) {
			if (topHit.gameObject.tag == MyTags.PLAYER_TAG) {
				if (!stunned) {
					topHit.gameObject.GetComponent<Rigidbody2D> ().velocity =
						new Vector2 (topHit.gameObject.GetComponent<Rigidbody2D>().velocity.x, 7f);

					canMove = false;
					myBody.velocity = new Vector2 (0, 0);

					anim.Play ("Stunned");
					stunned = true;

					// BEETLE CODE HERE
					if(tag == MyTags.BEETLE_TAG) {
						anim.Play ("Stunned");
						StartCoroutine (Dead (0.5f));
					}
				}
			}
		}

		if (leftHit) {
			if (leftHit.collider.gameObject.tag == MyTags.PLAYER_TAG) {
				if (!stunned) {
					// APPLY DAMAGE TO PLAYER
					leftHit.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();
				} else {
					if (tag != MyTags.BEETLE_TAG) {
						myBody.velocity = new Vector2 (15f, myBody.velocity.y);
						StartCoroutine (Dead (3f));
					}
				}
			}
		}

		if (rightHit) {
			if (rightHit.collider.gameObject.tag == MyTags.PLAYER_TAG) {
				if (!stunned) {
					// APPLY DAMAGE TO PLAYER
					rightHit.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();
				} else {
					if (tag != MyTags.BEETLE_TAG) {
						myBody.velocity = new Vector2 (-15f, myBody.velocity.y);
						StartCoroutine (Dead (3f));
					}
				}
			}
		}

		// IF we don't detect collision any more do whats in {}
		if (!Physics2D.Raycast (down_Collision.position, Vector2.down, 0.1f)) {

			ChangeDirection ();
		}

	}

	void ChangeDirection() {

		moveLeft = !moveLeft;

		Vector3 tempScale = transform.localScale;

		if (moveLeft) {
			tempScale.x = Mathf.Abs (tempScale.x);

			left_Collision.position = left_Collision_Pos;
			right_Collision.position = right_Collision_Pos;

		} else {
			tempScale.x = -Mathf.Abs (tempScale.x);

			left_Collision.position = right_Collision_Pos;
			right_Collision.position = left_Collision_Pos;
		}

		transform.localScale = tempScale;

	}

	IEnumerator Dead(float timer) {
		yield return new WaitForSeconds (timer);
		gameObject.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D target) {
		if (target.tag == MyTags.BULLET_TAG) {

			if (tag == MyTags.BEETLE_TAG) {
				anim.Play ("Stunned");

				canMove = false;
				myBody.velocity = new Vector2 (0, 0);

				StartCoroutine (Dead (0.4f));
			}

			if (tag == MyTags.SNAIL_TAG) {
				if (!stunned) {
					
					anim.Play ("Stunned");
					stunned = true;
					canMove = false;
					myBody.velocity = new Vector2 (0, 0);

				} else {
					gameObject.SetActive (false);
				}
			}

		}
	}

} // class














































