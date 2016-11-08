using UnityEngine;
using System.Collections;

public class Oscillator : MonoBehaviour {
    public float speed = 1f;
    public float radius = 0.16f;
    public Vector2 offSet = new Vector3(-0.1f, -0.1f, 0);//For most of trinkets
    float time = 0;
    Transform target;
	// Use this for initialization
	void Start () {
        if (transform.parent == null)
            return;

        target = transform.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate() {
        if (target == null)
            return;
        time += speed * Time.deltaTime;
        float x = Mathf.Cos(time) * radius;
        float y = Mathf.Sin(time) * radius;
        transform.position = (Vector2)target.transform.position + new Vector2(x,y) + offSet;
    }
}
