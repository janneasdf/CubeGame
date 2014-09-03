using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static string enemyTag;

	public GameObject playerPrefab;
	public GameObject enemyPrefab;
	public GameObject ground;
	public GameObject player;
	public float beginEnemySpawnCooldown;

	private float enemySpawnCooldown;
	private float timeSinceGameStart = 0.0f;
	private GUIText scoreText;
	private int enemiesBeaten = 0;
	private int highscore = 0;
	private float sinceSpawn = 0.0f; 

	// Use this for initialization
	void Start () {
		enemyTag = "Enemy";
		scoreText = GetComponent<GUIText> ();
		StartGame ();
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceGameStart += Time.deltaTime;
		enemySpawnCooldown = beginEnemySpawnCooldown - Mathf.Min(1.35f, timeSinceGameStart / 120.0f);

		sinceSpawn += Time.deltaTime;
		while (sinceSpawn > enemySpawnCooldown) {
			sinceSpawn -= enemySpawnCooldown;
			float width = ground.transform.lossyScale.x;
			float height = ground.transform.lossyScale.z;
			Vector3 position = new Vector3(0.5f * (-0.5f + Random.value) * width, 0.52f, 0.5f * (-0.5f + Random.value) * height);
			GameObject enemy = (GameObject)GameObject.Instantiate(enemyPrefab, position, Quaternion.identity);
			enemy.GetComponent<EnemyAI>().player = player;
			enemy.GetComponent<EnemyAI>().gameController = this;
			enemy.tag = enemyTag;
		}

		scoreText.text = "Enemies beaten: " + enemiesBeaten + "\n" + "Highscore: " + highscore + "\n\n" + "Power left: " + player.GetComponent<PlayerControls>().powerLeft;
	}

	void EndGame() {
		if (enemiesBeaten > highscore) {
			highscore = enemiesBeaten;
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag (enemyTag)) {
			GameObject.Destroy (enemy);
		}
		GameObject.Destroy (player);
	}

	void StartGame() {
		enemiesBeaten = 0;
		timeSinceGameStart = 0.0f;
		Vector3 position = new Vector3 (0.0f, 0.52f, 0.0f);
		player = (GameObject)GameObject.Instantiate(playerPrefab, position, Quaternion.identity);
		player.GetComponent<PlayerControls> ().gameController = this;
	}

	// Called by enemies on death
	public void RegisterEnemyDeath() {
		enemiesBeaten++;
		if (enemiesBeaten != 0 && (enemiesBeaten % 50 == 0)) {
			player.GetComponent<PlayerControls>().powerLeft++;
		}
	}

	public void RegisterPlayerDeath() {
		EndGame ();
		StartGame ();
	}
}
