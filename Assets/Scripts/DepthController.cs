using UnityEngine;
using System.Collections;

public class DepthController : MonoBehaviour {

	public float Offset = 0.0f;

	private float FixedOffset = 0.0f;

	void Start()
	{
		FixedOffset = Offset / 1000.0f;
		transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.y/1000.0f) + FixedOffset);
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.y/1000.0f) + FixedOffset);
	}
}
