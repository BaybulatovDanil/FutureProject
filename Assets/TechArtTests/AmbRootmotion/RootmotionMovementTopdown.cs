using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RootmotionMovementTopdown : MonoBehaviour
{
    [SerializeField]
    private InputActionReference movement;
    [SerializeField]
    private InputActionReference lookDirection;
    [SerializeField]
    private InputActionReference forceWalkButton;
    [SerializeField]
    private float angleSpeed = 0.01f;
    [SerializeField]
    private float movementDelta = 3f;
    [SerializeField]
    private Transform cameraTransform;

    bool hasCamera = false;

    float speedCap => forceWalkButton.action.IsPressed() ? 0.49f : 1f;

    Vector2 inputVector = Vector2.zero;
    Vector2 lastInputVector = Vector2.zero;
    Vector2 lookVector = Vector2.zero;
    Vector2 dir = Vector2.zero;
    float magnitude = 0f;

    float lastSpeed = 0f;
    float lastAngle = 0f;
    float lastLookAngle = 0f;
    Vector2 lastDirection = Vector2.zero;

    public Vector2 Direction => lastDirection;
    public float Speed => lastSpeed;

    GameObject unit;

    [SerializeField]
    private Animator animator;

    float lookAngleDotDiff = 0f;

#if UNITY_EDITOR
    Vector3 angleA = Vector3.zero;
    Vector3 angleB = Vector3.zero;
#endif

    #region AnimThresholds
    const float rot90Degrees = 0f;
    const float rot180Degrees = -0.7f;
    #endregion

    #region AnimBools
    bool shouldResetAnimBools = false;

    //bool rotate90L = false;
    //bool rotate90R = false;
    //bool rotate180L = false;
    //bool rotate180R = false;

    public bool isTurning = false;
    #endregion

    void ResetAnimBools()
    {
        //rotate90L = false;
        //rotate90R = false;
        //rotate180L = false;
        //rotate180R = false;
        animator.ResetTrigger("TurnLeft180");
        animator.ResetTrigger("TurnRight180");
        animator.ResetTrigger("TurnRunLeft180");
        animator.ResetTrigger("TurnRunRight180");
    }

    public void EndTurn()
    {
        isTurning = false;
    }

    void Awake()
    {
        unit = animator.gameObject;
        hasCamera = cameraTransform != null;
    }

    Vector2 rotateVector2(Vector2 vec, float angle)
    {
        float newAngle = Mathf.Atan2(vec.y, vec.x) + angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));
    }

    void Update()
    {
        if (shouldResetAnimBools) ResetAnimBools();

        var rawMovementInput = movement.action.ReadValue<Vector2>();
        inputVector = Vector2.MoveTowards(lastInputVector, rawMovementInput, movementDelta * Time.deltaTime);

        magnitude = Mathf.Min(rawMovementInput.magnitude, speedCap);

        /*lookVector = lookDirection.action.ReadValue<Vector2>();
        lastLookAngle = Mathf.SmoothStep(lastLookAngle, Vector2.Dot(lookVector,Vector2.right), 0.5f);
        //lastLookAngle = Mathf.MoveTowardsAngle(lastLookAngle, Vector2.Dot(lookVector, Vector2.right), 0.5f);
        animator.SetFloat("LookAngle", lastLookAngle);*/
        dir = inputVector.normalized;
        if (hasCamera)
        {
            var v3Inputs = new Vector3(inputVector.x, 0f, inputVector.y);
            v3Inputs =  this.transform.InverseTransformDirection(cameraTransform.TransformDirection(v3Inputs));
            dir = new Vector2(v3Inputs.x, v3Inputs.z).normalized;
        }
        lookAngleDotDiff = Vector3.Dot(this.transform.TransformDirection(new Vector3(dir.x, 0f, dir.y)), this.transform.forward);
        //Debug.Log($"AngleDiff:[{(lookAngleDotDiff> rot180Degrees ? (lookAngleDotDiff > rot90Degrees ? $"<color=green>{lookAngleDotDiff}</color>": $"<color=blue>{lookAngleDotDiff}</color>"): ($" <color=red>{lookAngleDotDiff}</color>"))}]");
#if UNITY_EDITOR
        angleA = this.transform.forward;
        angleB = new Vector3(dir.x, 0f, dir.y);
#endif


        /*lastSpeed = magnitude;
        animator.SetFloat("Speed", magnitude);*/
        lastSpeed = Mathf.SmoothStep(lastSpeed, magnitude, 0.2f);
        animator.SetFloat("Speed", lastSpeed);

        lastAngle = Vector2.SignedAngle(Vector2.Lerp(lastDirection, dir, /*0.1f*/0.3f), Vector2.up) / 180f;
        animator.SetFloat("Angle", lastAngle);

        var lookAngleDotRight = Vector3.Dot(transform.right, this.transform.TransformDirection(angleB));
        var lookInput = (lookAngleDotDiff < rot180Degrees) ? 0f : lookAngleDotRight;
        if ((lookAngleDotDiff < rot180Degrees) && !isTurning)
        {
            isTurning = true;
            shouldResetAnimBools = true;
            if (lookAngleDotRight < 0f)
            {
                if (lastSpeed > 0.5f)
                {
                    animator.SetTrigger("TurnRunLeft180");
                }
                else
                {
                    animator.SetTrigger("TurnLeft180");
                }
            }
            else
            {
                if (lastSpeed > 0.5f)
                {
                    animator.SetTrigger("TurnRunRight180");
                }
                else
                {
                    animator.SetTrigger("TurnRight180");
                }
            }
        }
        else
        {

        }
        //lookVector = /*lookDirection.action.ReadValue<Vector2>();*/lookAngleDotRight;
        //lastLookAngle = Mathf.SmoothStep(lastLookAngle, /*Vector2.Dot(lookVector, Vector2.right)*/lookInput, 0.5f);
        lastLookAngle = lookInput;
        //lastLookAngle = Mathf.MoveTowardsAngle(lastLookAngle, Vector2.Dot(lookVector, Vector2.right), 0.5f);
        animator.SetFloat("LookAngle", lastLookAngle);
            //animator.SetFloat("Angle", lastLookAngle);


        float lerpedAngleSpeedSpeed = magnitude * angleSpeed * lastLookAngle * 300f;
        unit.transform.localRotation *= Quaternion.Euler(0f, lerpedAngleSpeedSpeed, 0f);
        lastDirection = dir;
        lastInputVector = inputVector;
            //Debug.Log($"LookInput:[{lookInput}], lerpedAngleSpeedSpeed:[{lerpedAngleSpeedSpeed}]");
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var origCol = Gizmos.color;
        Gizmos.color = Color.white;
        Gizmos.DrawLine(this.transform.position + Vector3.up, this.transform.position + Vector3.up + angleA * 5f);
        Gizmos.color = (lookAngleDotDiff > rot180Degrees ? (lookAngleDotDiff > rot90Degrees ? Color.green : Color.blue) : Color.red);
        Gizmos.DrawLine(this.transform.position + Vector3.up, this.transform.position + Vector3.up + this.transform.TransformDirection(angleB) * 5f);
    }
#endif
}

