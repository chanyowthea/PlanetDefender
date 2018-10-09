using UnityEngine;  
using System.Collections;  

public class UVmove : MonoBehaviour {  

	public int ScrollSpeed = 5;  
	public int countX = 1;  
	public int countY = 2;  
	public int tilingX = 10;  
	public int tilingY = 10;  

	private float offsetX = 0.0f;  
	private float offsetY = 0.0f;  
	// private GameObject singleTexSize;  
	// Use this for initialization  
	void Start () {  
		float x_1 = 1.0f / countX;  
		float y_1 = 1.0f / countY;  
		GetComponent<Renderer>().material.mainTextureScale = new Vector2(x_1,y_1);  
		GetComponent<Renderer> ().material.SetTextureScale ("_MainTex", new Vector2(tilingX, tilingY));

	}  

	// Update is called once per frame  
	void Update () {  

		float frame = Mathf.Floor(Time.time * ScrollSpeed);  
		offsetX = frame / countX;  
		offsetY = -(frame - frame % countX) / countY / countX;  
		GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));  

	}  
}  