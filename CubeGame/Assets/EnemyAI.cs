using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public GameObject player;
	public float forceFactor;
	public GameController gameController;
	float disabledTime = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -2.0f) {
			gameController.RegisterEnemyDeath ();
			Destroy (gameObject);
			return;
		}

		disabledTime -= Time.deltaTime;
		if (disabledTime <= 0.0f) {
			Vector3 playerPos = player.transform.position;
			Vector3 force = (playerPos - transform.position).normalized * forceFactor;
			force.y = 0.0f;
			rigidbody.AddForce (force);
		}
	}

	public void DisableMovement(float time) {
		disabledTime = time;
	}
}
