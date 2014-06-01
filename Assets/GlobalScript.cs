using UnityEngine;
using System.Collections;
using SharpConnect;
using System.Net;

public class GlobalScript : MonoBehaviour, OnActionListener, RecvLogger {

	public GameObject player;
	public GameObject imageTarget;
	public GameObject reference;

	private Connector connector;
	private Queue queue;
	private object _queueLock = new Object();

	private int[,,] game_matrix_1 = new int[4,4,4] {
		{
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}
		},
		{
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}
		},
		{
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}
		},
		{
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}, 
			{0, 0, 0, 0}
		}
	};

	private int[,,] game_matrix_2 = new int[4,4,4] {
		{
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}
		},
		{
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}
		},
		{
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}
		},
		{
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}, 
			{8, 8, 8, 8}
		}
	};

	class Anim {
		public int from_x = 0;
		public int from_y = 0, from_z = 0;
		public int to_x = 0, to_y = 0, to_z = 0;
	}

	float animTime = 3.0f;
	float currTime = 0.0f;

	private Anim[] animList;


	// Use this for initialization
	void Start () {
		Debug.Log ("testing");
		connector = new Connector();

		queue = new Queue ();

		IPAddress ipAddress = IPAddress.Parse ("172.16.236.105");
		IPEndPoint endpoint = new IPEndPoint (ipAddress, 6070);

		string result = connector.fnConnectResult (endpoint);
		Debug.Log (result);
		connector.setOnActionListener (this);
		connector.setRecvLogger (this);

//		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
//		cube.transform.parent = imageTarget.transform;
//		cube.transform.position = player.transform.position + new Vector3(1, 0, 0) * 100f;
//		cube.transform.localScale = new Vector3 (0.108f, 0.108f, 0.108f);
//
		Debug.Log ("Local scale:" + reference.transform.localScale[0]);


		Anim anim1 = new Anim ();

		anim1.from_x = 0;
		anim1.from_y = 0;
		anim1.from_z = 0;
		anim1.to_x = 0;
		anim1.to_y = 0;
		anim1.to_z = 3;

		Anim anim2 = new Anim ();

		anim2.from_x = 0;
		anim2.from_y = 0;
		anim2.from_z = 2;
		anim2.to_x = 0;
		anim2.to_y = 0;
		anim2.to_z = 3;

		animList = new Anim[2] {anim1, anim2};
	}

	Queue gameObjects = new Queue();

	// Update is called once per frame
	void Update () {

		while (!(gameObjects.Count == 0)) {
			GameObject gameObject = (GameObject) gameObjects.Dequeue();
			Destroy(gameObject);
		}

		for (int i=0; i<4; i++) {
			for (int j=0;j<4;j++) {
				for (int k=0;k<4;k++) {
					//game_matrix_1[i,j,k] = 8;
//					Anim anim = new Anim();
//					bool found = false;
//					for (int anim_index = 0; anim_index < animList.Length; anim_index++) {
//						if (animList[anim_index].from_x == i && animList[anim_index].from_y == j && animList[anim_index].from_z == k) {
//							found = true;
//							anim = animList[anim_index];
//						}
//					}
//
//					if (found) {
//
//						GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
//						cube.transform.parent = reference.transform;
//						cube.transform.localScale = new Vector3 (1f, 1f, 1f) * 50f;
//						float delta_x = (float) anim.from_x + ( (float) anim.to_x -  (float) anim.from_x) * (currTime / animTime);
//						float delta_y = (float) anim.from_y + ( (float) anim.to_y -  (float) anim.from_y) * (currTime / animTime);
//						float delta_z = (float) anim.from_z + ( (float) anim.to_z -  (float) anim.from_z) * (currTime / animTime);
//						cube.transform.position = new Vector3(30f, 30f, 30f) + new Vector3(delta_x , delta_y, delta_z) * 60f;
//
//						string texture = "Assets/Resources/Textures/" + game_matrix_1[i,j,k] + ".png";
//						Texture2D inputTexture = (Texture2D)Resources.LoadAssetAtPath(texture, typeof(Texture2D));
//						
//						cube.renderer.material.mainTexture = inputTexture;
//						cube.renderer.material.shader = Shader.Find ("Unlit/Texture");
//
//
//						gameObjects.Enqueue(cube);
//
//						continue;
//					}

					if (game_matrix_1[i,j,k] != 0) {
						GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
						cube.transform.parent = reference.transform;
						cube.transform.localScale = new Vector3 (1f, 1f, 1f) * 50f;
						cube.transform.position = new Vector3(90f, -30f, 90f) * -1f + new Vector3(i, j, k) * 60f;
						
						
						string texture = "Textures/" + game_matrix_1[i,j,k];
						Texture2D inputTexture = (Texture2D)Resources.Load(texture, typeof(Texture2D));

						cube.renderer.material.mainTexture = inputTexture;
						cube.renderer.material.shader = Shader.Find ("Unlit/Texture");

						gameObjects.Enqueue(cube);
					}

				}
			}
		}


		//player.transform.position = player.transform.position - Vector3.forward;
		Debug.Log (connector.res);
		Runnable run = null;
		lock (_queueLock) {
			if (queue.Count > 0) {
				run = (Runnable) queue.Dequeue();
			}
		}
		if (run != null) {
						run.Run ();
				}

		//GameObject.Instantiate (cube, player.transform.position + Vector3.forward * 5f, Quaternion.identity);

		Debug.Log ("Just a test");

//		currTime += Time.deltaTime;
//		if (currTime >= animTime) {
//			currTime = animTime;
//			game_matrix_1 = game_matrix_2;
//			animList = new Anim[0];
//				}
	}

	public void onDown() {
		lock (_queueLock) {
			queue.Enqueue(new MoveRunnable(player,0,game_matrix_1, connector.gameManager));
		}
	}

	public void onUp() {
		lock (_queueLock) {
			queue.Enqueue(new MoveRunnable(player,1,game_matrix_1, connector.gameManager));
		}
	}

	public void onLeft() {
		lock (_queueLock) {
			queue.Enqueue(new MoveRunnable(player,2,game_matrix_1, connector.gameManager));
		}
	}

	public void onRight() {
		//player.transform.position = player.transform.position + Vector3.forward;
		lock (_queueLock) {
			queue.Enqueue(new MoveRunnable(player,3,game_matrix_1, connector.gameManager));
		}
	}

	public void onReceive(string message) {
		lock (_queueLock) {
			queue.Enqueue(new DebugRunnable(message));
		}
	}

	private interface Runnable {
		void Run ();
	}

	class DebugRunnable : Runnable {
		private string message;
		public DebugRunnable(string message) {
			this.message = message;
		}
		public void Run() {
			Debug.Log ("Received " + message);
		}
	}

	class MoveRunnable : Runnable {
		private GameObject player;
		private int direction;
		private GameManager gameManager;
		private int[,,] matrix;

		public MoveRunnable(GameObject player, int direction, int[,,] matrix, GameManager gameManager) {
			this.player = player;
			this.direction = direction;
			this.gameManager = gameManager;
			this.matrix = matrix;
		}

		public void Run() {
			for (int i=0;i<4;i++) {
				for (int j=0;j<4;j++) {
					for (int k=0;k<4;k++) {
						matrix[i,j,k] = gameManager.matrix[i,3 - j,3 - k];
					}
				}
			}
//			switch (direction) {
//			case 0:
//				player.transform.position = player.transform.position + new Vector3(-1, 0, 0) * 5f;
//				break;
//			case 1:
//				player.transform.position = player.transform.position + new Vector3(1, 0, 0) * 5f;
//				break;
//			case 2:
//				player.transform.position = player.transform.position + new Vector3(0, -1, 0) * 5f;
//				break;
//			case 3:
//				player.transform.position = player.transform.position + new Vector3(0, 1, 0) * 5f;
//				break;
//						}
		}
	}

}
