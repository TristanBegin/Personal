using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Catcher : MonoBehaviour {

    public LayerMask TelekableMask;
    public LayerMask PullableMask;

    List<GameObject> Arsenal = new List<GameObject>();

    bool Catching = false;
    float CatchTime = 0;
    float MaxCatchTime = 0.05f;

    Vector3 arsCenter;
    Vector3 arsOffset;
    Vector3 arsRight;
    Vector3 arsUp;
    Vector3 arsForward { get { return Vector3.Cross(arsRight, arsUp); } }

    float arsenalRadius = 2;
    float arsenalRotationSpeed = 2;
    float arsenalRotation = 0;

    float MeleeLength = 1;

    public enum States
    {
        Idle,
        Melee,
        Roll,
        Pull
    }

    float ActiveTimer = 0;
    float MaxActiveTimer = 5;

    public static States State     = States.Idle;
    int InnerState   = 0;
    float stateTimer = 0;
    float MeleeRange = 3;
    Vector3 OriginalOffset;
    Vector3 GoalOffset;

    Camera cam;

    Transform arsenalParent;
    Transform arsenalDirReference;

    GameObject myTelek;
    Vector3 myTelekOGPosition;
    bool pulling = false;

    bool bulletCircle = true;

    Vector3 bulletForwardOverride = Vector3.zero;

    public static bool Rolling { get { return State == States.Roll; } }

    public static Vector3 telekPosition { get { return instance.myTelek.transform.position; } }

    public static Catcher instance;

    public static bool HitEnemyThisFrame = false;

    // Use this for initialization
    void Start ()
    {
        instance = this;
        cam = Camera.main;
        arsenalParent = transform.parent;
        arsenalDirReference = cam.transform;
    }

    void LateUpdate()
    {
        ActiveTimer -= Time.deltaTime;

        if (Input.GetButton("LB"))
        {
            Catching = true;
            CatchTime = MaxCatchTime;
            SetActive();
        }

        if (State == States.Melee)
        {
            HitEnemyThisFrame = false;
            Physics.OverlapSphere(arsCenter, arsenalRadius).ToList().ForEach(col =>
            {
                if (col.tag == "Enemy")
                {
                    HitEnemyThisFrame = true;
                    Debug.Log("HIT");
                }
            });
            
        }

        if (Catching)
        {
            CatchTime -= Time.deltaTime;
            transform.localScale = new Vector3 (3, 4, 3);
            //transform.forward = cam.transform.forward;

            Vector3 inputVec = (cam.transform.right * Input.GetAxis("Horizontal_RS") + cam.transform.up * Input.GetAxis("Vertical_RS")).normalized;
            if (inputVec.magnitude < 0.1f) inputVec = Vector3.up;

            transform.position = transform.parent.position + inputVec * 5;
            transform.up = inputVec;

            if (CatchTime < 0)
            {
                Catching = false;
            }
        }
        else
        {
            CatchTime = 0;
            transform.localScale = Vector3.zero;
        }


        switch (State)
        {
            case States.Idle:  Idle();  break;
            case States.Melee: Melee(); break;
            case States.Roll:  Roll();  break;
            case States.Pull:  Pull(); break;
        }



        arsenalRotation += Time.deltaTime * arsenalRotationSpeed;
        arsCenter = arsenalParent.position + arsOffset;

        if (IsActive())
        {
            if (bulletCircle)
            { 
                for (int i = 0; i < Arsenal.Count; i++)
                {
                    float indexOffset = ((float)i / Arsenal.Count) * Mathf.PI * 2;
                    Vector3 offsetVector = arsRight * Mathf.Sin(arsenalRotation + indexOffset) * arsenalRadius + arsUp * Mathf.Cos(arsenalRotation + indexOffset) * arsenalRadius;
                    Vector3 goalPosition = arsCenter + offsetVector;
                    Arsenal[i].transform.position += (goalPosition - Arsenal[i].transform.position) * Time.deltaTime * 20;
                    Arsenal[i].GetComponent<Rigidbody>().velocity = Vector3.zero;

                    Vector3 goalForward = offsetVector.normalized;
                    if (bulletForwardOverride.magnitude > 0.1f)
                    {
                        goalForward = bulletForwardOverride;
                    }

                    Arsenal[i].transform.rotation = Quaternion.RotateTowards(Arsenal[i].transform.rotation, Quaternion.LookRotation(goalForward), Time.deltaTime * 800);
                }
            }
        }
        else
        {
            for (int i = 0; i < Arsenal.Count; i++)
            {
                Vector3 goalPosition = arsenalParent.transform.position - arsenalParent.forward * 0.5f;
                Arsenal[i].transform.position += (goalPosition - Arsenal[i].transform.position) * Time.deltaTime * 20;
                Arsenal[i].GetComponent<Rigidbody>().velocity = Vector3.zero;

                Vector3 goalForward = arsenalParent.up;
                Arsenal[i].transform.rotation = Quaternion.RotateTowards(Arsenal[i].transform.rotation, Quaternion.LookRotation(goalForward), Time.deltaTime * 800);
            }
        }

        Vector3 ShootDirection = (arsForward + arsenalParent.up * 0.175f).normalized;

        if (Input.GetAxis("RightTrigger") < -0.1f || Input.GetAxis("LeftTrigger") < -0.1f || Input.GetButton("RB"))
        {
            SetActive();
            if (myTelek == null)
            {
                if (Input.GetAxis("LeftTrigger") < -0.1f)
                    pulling = true;
                else
                    pulling = false;

                if (Input.GetAxis("RightTrigger") < -0.1f || Input.GetAxis("LeftTrigger") < -0.1f)
                {
                    RaycastHit hit;
                    Physics.SphereCast(arsCenter, arsenalRadius * 3, arsForward, out hit, 100, Input.GetAxis("RightTrigger") < -0.1f ? TelekableMask : PullableMask);
                    if (hit.transform != null)
                        myTelek = hit.transform.gameObject;
                }
                else if (Arsenal.Count > 0)
                {
                    myTelek = Arsenal[0];
                    Arsenal.RemoveAt(0);
                }

                if (myTelek != null)
                    myTelekOGPosition = myTelek.transform.position;
            }
            else
            {
                if (pulling)
                {
                    State = States.Pull;
                }
                else
                {
                    myTelek.transform.position += (arsCenter - myTelek.transform.position) * 14 * Time.deltaTime;
                    myTelek.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    myTelek.GetComponent<Collider>().isTrigger = true;
                    myTelek.transform.rotation = Quaternion.RotateTowards(myTelek.transform.rotation, Quaternion.LookRotation(ShootDirection), Time.deltaTime * 800);
                }
            }
        }
        else if (myTelek != null)
        {
            if (myTelek.GetComponent<Rigidbody>() != null)
                myTelek.GetComponent<Rigidbody>().velocity = ShootDirection * 80;

            myTelek.GetComponent<Collider>().isTrigger = false;
            myTelek = null;
        }
    }

    void SetActive()
    {
        ActiveTimer = MaxActiveTimer;
    }
    

    bool IsActive()
    {
        return ActiveTimer > 0;
    }

    void Idle()
    {
        arsRight = arsenalDirReference.right;
        arsUp = arsenalDirReference.up;

        arsOffset = arsenalDirReference.up * 2.5f + arsenalDirReference.forward * 2;

        arsenalRotationSpeed = 2;
        arsenalRadius = 1.5f;
        bulletForwardOverride = arsenalDirReference.forward;

        bulletCircle = true;

        if (Input.GetButtonDown("X_Button"))
        {
            SetActive();
            State = States.Melee;
        }

        if (Input.GetButton("B_Button"))
        {
            SetActive();
            State = States.Roll;
        }
    }

    void Melee()
    {
        switch (InnerState)
        {
            case 0:
                {
                    stateTimer = 0;
                    InnerState = 1;
                    arsUp = arsenalParent.forward;
                    arsRight = -arsenalParent.right;
                    bulletForwardOverride = Vector3.zero;
                    OriginalOffset = Vector3.zero;
                    GoalOffset = Vector3.up * 2 + arsenalParent.forward * 8;
                    bulletCircle = true;
                } break;

            case 1:
                {
                    stateTimer += Time.deltaTime;
                    float progress = stateTimer / MeleeLength;
                    arsOffset = OriginalOffset + Mathf.Sin(progress * Mathf.PI) * (GoalOffset - OriginalOffset);
                    arsenalRadius = Mathf.Sin(progress * Mathf.PI) * MeleeRange;
                    arsenalRotationSpeed = Mathf.Sin(progress * Mathf.PI) * 20;

                    if (progress > 1)
                    {
                        InnerState = 0;
                        State = States.Idle;
                    }
                } break;
        }

    }

    void Roll()
    {
        switch (InnerState)
        {
            case 0:
                {
                    SetActive();
                    stateTimer = 0;
                    arsUp = (arsenalParent.up + arsenalParent.right * Input.GetAxis("Horizontal") * 0.5f).normalized;
                    arsRight = arsenalParent.forward;
                    arsOffset = Vector3.zero;
                    bulletForwardOverride = Vector3.zero;
                    arsenalRotationSpeed = 5;
                    arsenalRadius = 3;
                    bulletCircle = true;

                    if (Input.GetButton("B_Button") == false)
                        State = States.Idle;
                }
                break;
        }
    }

    void Pull()
    {
        switch (InnerState)
        {
            case 0:
                {
                    SetActive();
                    stateTimer = 0;
                    for (int i = 0; i < Arsenal.Count; i++)
                    {
                        Vector3 vecToTelek = myTelek.transform.position - arsenalParent.transform.position;
                        Arsenal[i].transform.position = arsenalParent.transform.position + vecToTelek * ((float) i / Arsenal.Count);
                    }

                    myTelek.transform.position = myTelekOGPosition;
                    
                    bulletCircle = false;

                    if (Input.GetAxis("LeftTrigger") > -0.1f)
                        State = States.Idle;
                }
                break;
        }
    }

    void OnTriggerEnter (Collider hit)
    {
        if (Catching && hit.tag == "Catchable" && Arsenal.Contains(hit.gameObject) == false)
        {
            Arsenal.Add(hit.gameObject);
        }
		
	}
}
