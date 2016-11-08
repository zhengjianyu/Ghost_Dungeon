using UnityEngine;
using System.Collections;

public class CameraAspector : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera.main.aspect = 1920f / 1080f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
