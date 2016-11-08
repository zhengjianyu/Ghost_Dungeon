using UnityEngine;
using System.Collections;

public class FireFlicker : MonoBehaviour {

	public float MaxIntensity = 6.0f;
	public float MinIntensity = 4.0f;
	private Light lt;
	private int flip = 1;

	void Start()
	{
		lt = GetComponent<Light>();
	}

	void Update()
	{
		//super awesome one-liner
		lt.intensity += (lt.intensity > MaxIntensity || lt.intensity < MinIntensity ? flip *= -1 : flip) * 0.1f;
		//lt.intensity += flip * 0.02f;
		//if (lt.intensity > MaxIntensity || lt.intensity < MinIntensity)
		//	flip *= -1;
	}
}
