using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RootmotionMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference movement;
    [SerializeField]
    private InputActionReference lookDirection;
    [SerializeField]
    float angleSpeed = 0.01f;
    [SerializeField]
    float movementDelta = 3f;

    float maxRadius = 0f;
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

    void Awake()
    {
        unit = animator.gameObject;
        maxRadius = 1f;
    }

    void Update()
    {
        inputVector = Vector2.MoveTowards(lastInputVector, movement.action.ReadValue<Vector2>(), movementDelta * Time.deltaTime);
        lookVector = lookDirection.action.ReadValue<Vector2>();
        lastLookAngle = Mathf.SmoothStep(lastLookAngle, Vector2.Dot(lookVector,Vector2.right), 0.5f);
        //lastLookAngle = Mathf.MoveTowardsAngle(lastLookAngle, Vector2.Dot(lookVector, Vector2.right), 0.5f);
        animator.SetFloat("LookAngle", lastLookAngle);
        dir = inputVector.normalized;
        magnitude = Mathf.Min(inputVector.magnitude, 1f) * maxRadius;

        //lastSpeed = Mathf.SmoothStep(lastSpeed, magnitude, 0.05f);
        lastSpeed = /*Mathf.SmoothStep(lastSpeed, */magnitude/*, 0.05f)*/;

        animator.SetFloat("Speed", magnitude);
        /*lastSpeed = Mathf.MoveTowards(lastSpeed, magnitude, 0.05f * Time.deltaTime);
        animator.SetFloat("Speed", lastSpeed);*/

        //lastAngle = Mathf.SmoothStep(lastAngle, Vector2.SignedAngle(dir, Vector2.up) / 180f, 0.1f);
        lastAngle = Vector2.SignedAngle(Vector2.Lerp(lastDirection, dir, 0.1f), Vector2.up) / 180f;

        animator.SetFloat("Angle", lastAngle);

        float lerpedAngleSpeedSpeed = magnitude * angleSpeed * lastLookAngle;
        unit.transform.localRotation *= Quaternion.Euler(0f, lerpedAngleSpeedSpeed, 0f);
        lastDirection = dir;
        lastInputVector = inputVector;
    }
}

