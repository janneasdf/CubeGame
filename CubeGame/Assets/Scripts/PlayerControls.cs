using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public float forceFactor;
	public GameController gameController;
	public int powerLeft = 1;		// ability
	public float power;
	public float powerRadius;		// not used atm
	public float enemyDisableTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -6.0f) {
			gameController.RegisterPlayerDeath ();
			return;
		}

		Vector3 input = new Vector3 ();
		if (Input.GetKey (KeyCode.A))
			input.x -= 1;
		if (Input.GetKey (KeyCode.D))
			input.x += 1;
		if (Input.GetKey (KeyCode.W))
			input.z += 1;
		if (Input.GetKey (KeyCode.S))
			input.z -= 1;
		input.Normalize();

		if (Input.GetKeyDown (KeyCode.Space) && powerLeft > 0) {
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(GameController.enemyTag)) {
				Vector3 force = (enemy.transform.position - transform.position).normalized * power;
				enemy.rigidbody.AddForce(force);
				enemy.GetComponent<EnemyAI>().DisableMovement(enemyDisableTime);
			}
			powerLeft--;
		}

		rigidbody.AddForce (input * forceFactor);
	}
}
