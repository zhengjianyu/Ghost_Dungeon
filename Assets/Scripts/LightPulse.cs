using UnityEngine;
using System.Collections;

public class LightPulse : MonoBehaviour
{

	public float MaxIntensity = 6.0f;
	public float MinIntensity = 4.0f;
	private Light lt;
	private int flip = 1;

	// Use this for initialization
	void Start ()
	{
		lt = GetComponent<Light>();
	}

	void Update ()
	{
		//super awesome one-liner
		lt.intensity += (lt.intensity > MaxIntensity || lt.intensity < MinIntensity ? flip *= -1 : flip) * 0.1f;
	}
}
