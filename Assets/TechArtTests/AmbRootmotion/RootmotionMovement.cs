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
    float angleSpeed = 1f;
    /*[SerializeField]
    private RectTransform dotPosition;*/
    float maxRadius = 0f;
    Vector2 inputVector = Vector2.zero;
    Vector2 lookVector = Vector2.zero;
    /*private Color[] trailBuffer = new Color[65536];
    private Color defaultColor = new Color(1f, 0.25f, 0.25f, 0f);*/

    Vector2 dir = Vector2.zero;
    float magnitude = 0f;

    float lastSpeed = 0f;
    float lastAngle = 0f;
    float lastLookAngle = 0f;
    Vector2 lastDirection = Vector2.zero;

    GameObject unit;

    /*[SerializeField]
    private Texture2D testTexture;
    [SerializeField]
    private Image ren;*/

    [SerializeField]
    private Animator animator;

    void Awake()
    {
        unit = animator.gameObject;
        maxRadius = 1f;/*dotPosition.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x / dotPosition.sizeDelta.x * (dotPosition.sizeDelta.x * 0.5f);*/
        /*for (int i = 0; i < trailBuffer.Length; i++)
        {
            trailBuffer[i] = defaultColor;
        }
        testTexture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        ren.material.mainTexture = testTexture;*/
    }

    void Update()
    {
        inputVector = movement.action.ReadValue<Vector2>();
        lookVector = lookDirection.action.ReadValue<Vector2>();
        lastLookAngle = Mathf.SmoothStep(lastLookAngle, Vector2.Dot(lookVector,Vector2.right), 0.5f);
        animator.SetFloat("LookAngle", lastLookAngle);
        dir = inputVector.normalized;
        magnitude = Mathf.Min(inputVector.magnitude, 1f) * maxRadius;

        lastSpeed = Mathf.SmoothStep(lastSpeed, magnitude, 0.05f);
        animator.SetFloat("Speed", magnitude);

        //lastAngle = Mathf.SmoothStep(lastAngle, Vector2.SignedAngle(dir, Vector2.up) / 180f, 0.1f);
        lastAngle = Vector2.SignedAngle(Vector2.Lerp(lastDirection, dir, 0.1f), Vector2.up) / 180f;

        animator.SetFloat("Angle", lastAngle);

        float lerpedAngleSpeedSpeed = magnitude * angleSpeed * lastLookAngle;
        unit.transform.localRotation *= Quaternion.Euler(0f, lerpedAngleSpeedSpeed, 0f);
        lastDirection = dir;
        //Debug.Log(Vector2.SignedAngle(dir, Vector2.up));

        //dotPosition.anchoredPosition = dir * magnitude;
        /*UpdateTexture();
        testTexture.SetPixels(trailBuffer, 0);
        testTexture.Apply(true, false);*/
    }

    /*private void UpdateTexture()
    {
        var t = Mathf.CeilToInt((Time.realtimeSinceStartup * 50) % 256f);
        for (int i = 0; i < trailBuffer.Length; i++)
        {
            bool condition = ((t == (i % 256)) || (t == ((i + 1) % 256)) || (t == ((i - 1) % 256)) || (t == ((i + 2) % 256)) || (t == ((i - 2) % 256)));
            Vector2 pos = new Vector2(i % 256, i / 256);
            Vector2 dotPos = (dir * magnitude) * 2.56f + new Vector2(128f, 128f);
            bool condition2 = Vector2.Distance(pos, dotPos) < 10f;

            float alpha = condition2 ? 1f : (trailBuffer[i].a) * 0.995f;
            trailBuffer[i].a = alpha;

        }
    }*/
}

