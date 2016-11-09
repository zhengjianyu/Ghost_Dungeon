using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour {
    //Keyboard
    public KeyCode Menu = KeyCode.Escape;
    public KeyCode CharacterSheet = KeyCode.C;

    public KeyCode MoveUp = KeyCode.W;
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveDown = KeyCode.S;
    public KeyCode MoveRight = KeyCode.D;

    public KeyCode AttackUp = KeyCode.UpArrow;
    public KeyCode AttackLeft = KeyCode.LeftArrow;
    public KeyCode AttackDown = KeyCode.DownArrow;
    public KeyCode AttackRight = KeyCode.RightArrow;

    public KeyCode Interact = KeyCode.F;

    //Active Skills
    public KeyCode Skill0 = KeyCode.Alpha1;
    public KeyCode Skill1 = KeyCode.Alpha2;
    public KeyCode Skill2 = KeyCode.Alpha3;
    public KeyCode Skill3 = KeyCode.Alpha4;

    //Xbobx one Controller 

    public string J_Start = "joystick button 7";
    public string J_Back = "joystick button 6";

    public string J_LeftAnalogHor = "J_HorizontalMove";//Axis
    public string J_LeftAnalogVer = "J_VerticalMove";//Axis
    public string J_RightAnalogHor = "J_HorizontalAttack";//Axis
    public string J_RightAnalogVer = "J_VerticalAttack";//Axis
    public string J_LeftTrigger = "joystick button 8";
    public string J_RightTrigger = "joystick button 9";

    public string J_LB = "joystick button 4";
    public string J_RB = "joystick button 5";
    public string J_LTRT = "J_LTRT"; //This one is actually an axis for fk sake IDK Why

    public string J_X = "joystick button 2";
    public string J_Y = "joystick button 3";
    public string J_B = "joystick button 1";
    public string J_A = "joystick button 0";

    public string J_DH = "J_DpadHor";//Axis
    public string J_DV = "J_DpadVer";//Axis


    //public string J_LB 
    [HideInInspector]
    public Vector2 MoveVector;
    [HideInInspector]
    public Vector2 AttackVector;
    [HideInInspector]
    public int Direction; //0,1,2,3 -> Down,Left,Right,Up

    [HideInInspector]
    public bool AllowControlUpdate = true;


    public static ControllerManager instance;
    public static ControllerManager Instance { get { return instance; } }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        MoveVector = new Vector2(0, 0);
        AttackVector = new Vector2(0, 0);
    }

    // Use this for initialization
    void Start () {

	}

    // Update is called once per frame
    void Update() {
        if (AllowControlUpdate) {
            UpdateMoveVector();
            UpdateAttackVector();
            UpdateDirection();
        }
    }
   
    //Moving Update
    void UpdateMoveVector() {
        Vector2 J_MoveInput = new Vector2(Input.GetAxisRaw(J_LeftAnalogHor), Input.GetAxisRaw(J_LeftAnalogVer));
        if (J_MoveInput != Vector2.zero) {
            GetControllerMoveInput(J_MoveInput);
        }
        else
            GetKeyboardMoveInput(); 
    }

    //Attacking Update
    void UpdateAttackVector() {
        Vector2 J_AttackInput = new Vector2(Input.GetAxisRaw(J_RightAnalogHor), Input.GetAxisRaw(J_RightAnalogVer));
        if (J_AttackInput != Vector2.zero) {
            GetControllerAttackInput(J_AttackInput);
        } else
            GetKeyboardAttackInput();
    }
    
    void UpdateDirection() {
        Vector2 J_MoveInput = new Vector2(Input.GetAxisRaw(J_LeftAnalogHor), Input.GetAxisRaw(J_LeftAnalogVer));
        Vector2 J_AttackInput = new Vector2(Input.GetAxisRaw(J_RightAnalogHor), Input.GetAxisRaw(J_RightAnalogVer));
        if (J_MoveInput != Vector2.zero || J_AttackInput != Vector2.zero) {
            GetControllerDirection(J_MoveInput, J_AttackInput);
        } else {
            GetKeyboardDirection();
        }
    }

    void GetControllerMoveInput(Vector2 J_MoveInput) {
        MoveVector = Vector3.Normalize(J_MoveInput - Vector2.zero);
    }

    void GetKeyboardMoveInput() {
        if (Input.GetKey(MoveLeft)) {
            MoveVector = new Vector2(-1, 0);
        }
        if (Input.GetKey(MoveRight)) {
            MoveVector = new Vector2(1, 0);
        }
        if (Input.GetKey(MoveUp)) {
            MoveVector = new Vector2(0, 1);
        }
        if (Input.GetKey(MoveDown)) {
            MoveVector = new Vector2(0, -1);
        }
        if (Input.GetKey(MoveLeft) && Input.GetKey(MoveUp)) {
            MoveVector = new Vector2(-Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2);
        }
        if (Input.GetKey(MoveLeft) && Input.GetKey(MoveDown)) {
            MoveVector = new Vector2(-Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2);
        }
        if (Input.GetKey(MoveRight) && Input.GetKey(MoveUp)) {
            MoveVector = new Vector2(Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2);
        }
        if (Input.GetKey(MoveRight) && Input.GetKey(MoveDown)) {
            MoveVector = new Vector2(Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2);
        }
        if (Input.GetKey(MoveUp) && Input.GetKey(MoveLeft)) {
            MoveVector = new Vector2(-Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2);
        }
        if (Input.GetKey(MoveUp) && Input.GetKey(MoveRight)) {
            MoveVector = new Vector2(Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2);
        }
        if (Input.GetKey(MoveDown) && Input.GetKey(MoveLeft)) {
            MoveVector = new Vector2(-Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2);
        }
        if (Input.GetKey(MoveDown) && Input.GetKey(MoveRight)) {
            MoveVector = new Vector2(Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2);
        }

        //Stop player movement on corresponding axis on key release
        if (!Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight)) {
            MoveVector = new Vector2(0, MoveVector.y);
        }
        if (!Input.GetKey(MoveUp) && !Input.GetKey(MoveDown)) {
            MoveVector = new Vector2(MoveVector.x, 0);
        }

        //Stop player movement on move direction conflict
        if (Input.GetKey(MoveLeft) && Input.GetKey(MoveRight))
            MoveVector = new Vector2(0, MoveVector.y);
        if (Input.GetKey(MoveUp) && Input.GetKey(MoveDown))
            MoveVector = new Vector2(MoveVector.x, 0);

        if (!Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight) && !Input.GetKey(MoveUp) && !Input.GetKey(MoveDown)) {
            MoveVector = Vector2.zero;
        }
    }

    void GetControllerAttackInput(Vector2 J_AttackInput) {
        AttackVector = Vector3.Normalize(J_AttackInput - Vector2.zero);
    }

    void GetKeyboardAttackInput() {
        if (Input.GetKey(AttackLeft)) {
            AttackVector = new Vector2(-1, 0);
        }
        if (Input.GetKey(AttackRight)) {
            AttackVector = new Vector2(1, 0);
        }
        if (Input.GetKey(AttackUp)) {
            AttackVector = new Vector2(0, 1);
        }
        if (Input.GetKey(AttackDown)) {
            AttackVector = new Vector2(0, -1);
        }
        if (Input.GetKey(AttackLeft) && Input.GetKey(AttackUp)) {
            AttackVector = new Vector2(-1, 1);
        }
        if (Input.GetKey(AttackLeft) && Input.GetKey(AttackDown)) {
            AttackVector = new Vector2(-1, -1);
        }
        if (Input.GetKey(AttackRight) && Input.GetKey(AttackUp)) {
            AttackVector = new Vector2(1, 1);
        }
        if (Input.GetKey(AttackRight) && Input.GetKey(AttackDown)) {
            AttackVector = new Vector2(1, -1);
        }
        if (Input.GetKey(AttackUp) && Input.GetKey(AttackLeft)) {
            AttackVector = new Vector2(-1, 1);
        }
        if (Input.GetKey(AttackUp) && Input.GetKey(AttackRight)) {
            AttackVector = new Vector2(1, 1);
        }
        if (Input.GetKey(AttackDown) && Input.GetKey(AttackLeft)) {
            AttackVector = new Vector2(-1, -1);
        }
        if (Input.GetKey(AttackDown) && Input.GetKey(AttackRight)) {
            AttackVector = new Vector2(1, -1);
        }

        //Stop player attack on corresponding axis on key release
        if (!Input.GetKey(AttackLeft) && !Input.GetKey(AttackRight)) {
            AttackVector = new Vector2(0, AttackVector.y);
        }
        if (!Input.GetKey(AttackUp) && !Input.GetKey(AttackDown)) {
            AttackVector = new Vector2(AttackVector.x, 0);
        }

        //Stop player attack on attack direction conflict
        if (Input.GetKey(AttackLeft) && Input.GetKey(AttackRight))
            AttackVector = new Vector2(0, AttackVector.y);
        if (Input.GetKey(AttackUp) && Input.GetKey(AttackDown))
            AttackVector = new Vector2(AttackVector.x, 0);

        if (!Input.GetKey(AttackLeft) && !Input.GetKey(AttackRight) && !Input.GetKey(AttackUp) && !Input.GetKey(AttackDown)) {
            AttackVector = Vector2.zero;
        }
    }
    void GetControllerDirection(Vector2 J_MoveInput, Vector2 J_AttackInput) {
        if (AttackVector == Vector2.zero) {
            if (Mathf.Abs(J_MoveInput.x) > Mathf.Abs(J_MoveInput.y)) {
                if (J_MoveInput.x < 0)
                    Direction = 1;
                else if (J_MoveInput.x > 0)
                    Direction = 2;
            }
            if (Mathf.Abs(J_MoveInput.x) < Mathf.Abs(J_MoveInput.y)) {
                if (J_MoveInput.y < 0)
                    Direction = 0;
                else if (J_MoveInput.y > 0)
                    Direction = 3;
            }
        }

        if (Mathf.Abs(J_AttackInput.x) > Mathf.Abs(J_AttackInput.y)) {
            if (J_AttackInput.x < 0)
                Direction = 1;
            else if (J_AttackInput.x > 0)
                Direction = 2;
        }
        if (Mathf.Abs(J_AttackInput.x) < Mathf.Abs(J_AttackInput.y)) {
            if (J_AttackInput.y < 0)
                Direction = 0;
            else if (J_AttackInput.y > 0)
                Direction = 3;
        }
    }
    void GetKeyboardDirection() {
        //Keydown Update
        if (Input.GetKeyDown(MoveLeft) && AttackVector == Vector2.zero) {
            Direction = 1;
        }
        if (Input.GetKeyDown(MoveRight) && AttackVector == Vector2.zero) {
            Direction = 2;
        }
        if (Input.GetKeyDown(MoveUp) && AttackVector == Vector2.zero) {
            Direction = 3;
        }
        if (Input.GetKeyDown(MoveDown) && AttackVector == Vector2.zero) {
            Direction = 0;
        }
        //Keyup Update
        if ((Input.GetKeyUp(MoveLeft) || Input.GetKeyUp(MoveRight)) && AttackVector == Vector2.zero) {
            if (MoveVector.y > 0) {
                Direction = 3;
            } else if (MoveVector.y < 0) {
                Direction = 0;
            }
        }
        if ((Input.GetKeyUp(MoveUp) || Input.GetKeyUp(MoveDown)) && AttackVector == Vector2.zero) {
            if (MoveVector.x > 0) {
                Direction = 2;
            } else if (MoveVector.x < 0) {
                Direction = 1;
            }
        }

        //Keydown Update
        if (Input.GetKeyDown(AttackLeft)) {
            Direction = 1;
        }
        if (Input.GetKeyDown(AttackRight)) {
            Direction = 2;
        }
        if (Input.GetKeyDown(AttackUp)) {
            Direction = 3;
        }
        if (Input.GetKeyDown(AttackDown)) {
            Direction = 0;
        }
        //Keyup Update
        if (Input.GetKeyUp(AttackLeft) || Input.GetKeyUp(AttackRight)) {
            if (AttackVector.y > 0) {
                Direction = 3;
            } else if (AttackVector.y < 0) {
                Direction = 0;
            }
        }
        if (Input.GetKeyUp(AttackUp) || Input.GetKeyUp(AttackDown)) {
            if (AttackVector.x > 0) {
                Direction = 2;
            } else if (AttackVector.x < 0) {
                Direction = 1;
            }
        }

        //Get back to move direction
        if (Input.GetKeyUp(AttackLeft) && (AttackVector == Vector2.zero)) {
            if (Input.GetKey(MoveLeft)) {
                Direction = 1;
            }
            if (Input.GetKey(MoveRight)) {
                Direction = 2;
            }
            if (Input.GetKey(MoveUp)) {
                Direction = 3;
            }
            if (Input.GetKey(MoveDown)) {
                Direction = 0;
            }
        }
        if (Input.GetKeyUp(AttackRight) && (AttackVector == Vector2.zero)) {
            if (Input.GetKey(MoveLeft)) {
                Direction = 1;
            }
            if (Input.GetKey(MoveRight)) {
                Direction = 2;
            }
            if (Input.GetKey(MoveUp)) {
                Direction = 3;
            }
            if (Input.GetKey(MoveDown)) {
                Direction = 0;
            }
        }
        if (Input.GetKeyUp(AttackUp) && (AttackVector == Vector2.zero)) {
            if (Input.GetKey(MoveLeft)) {
                Direction = 1;
            }
            if (Input.GetKey(MoveRight)) {
                Direction = 2;
            }
            if (Input.GetKey(MoveUp)) {
                Direction = 3;
            }
            if (Input.GetKey(MoveDown)) {
                Direction = 0;
            }
        }
        if (Input.GetKeyUp(AttackDown) && (AttackVector == Vector2.zero)) {
            if (Input.GetKey(MoveLeft)) {
                Direction = 1;
            }
            if (Input.GetKey(MoveRight)) {
                Direction = 2;
            }
            if (Input.GetKey(MoveUp)) {
                Direction = 3;
            }
            if (Input.GetKey(MoveDown)) {
                Direction = 0;
            }
        }
    }
}
