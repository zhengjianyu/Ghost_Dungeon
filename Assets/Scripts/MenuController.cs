using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
	public KeyCode ToggleOnOff = KeyCode.Escape;
	public KeyCode ToggleOff = KeyCode.C;

	public Light MouseLight = null;

    GameObject ES;

	void Awake ()
	{
        ES = GameObject.Find("EventSystem");
        //gameObject.SetActive(false);
    }


	void Update ()
	{
		if(GetComponent<SpriteRenderer> ().enabled)
		{
			Vector3 Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Mouse = new Vector3 (Mouse.x, Mouse.y, MouseLight.transform.position.z);
			MouseLight.transform.position = Mouse;
		}
	}

    public void Toggle() {
        if (IsOn()) {
            TurnOff();
        } else {
            TurnOn();
        }

    }

    public void TurnOn() {
        gameObject.SetActive(true);
        ES.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        GameObject FBO = GameObject.Find("MainPlayer/PlayerUI/Menu/SaveButton").gameObject;
        ES.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(FBO);
    }

    public void TurnOff() {
        gameObject.SetActive(false);
    }

    public bool IsOn() {
        return gameObject.active;
    }
}