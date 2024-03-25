using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputsScript : MonoBehaviour
{
    [SerializeField]
    private InputActionReference movement;
    [SerializeField]
    private RectTransform dotPosition;
    float maxRadius = 0f;
    Vector2 inputVector = Vector2.zero;
    private Color[] trailBuffer = new Color[65536];
    private Color defaultColor = new Color(1f, 0.25f, 0.25f, 0f);

    Vector2 dir;
    float magnitude;

    [SerializeField]
    RootmotionMovement rootmotionScript;
    [SerializeField]
    RootmotionMovementTopdown rootmotionScriptTopdown;
    bool hasRootmotionScript = false;

[SerializeField]
    private Texture2D testTexture;
    [SerializeField]
    private Image ren;


    void Awake()
    {
        hasRootmotionScript = (rootmotionScript != null) || (rootmotionScriptTopdown != null);
        maxRadius = dotPosition.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x / dotPosition.sizeDelta.x * (dotPosition.sizeDelta.x * 0.5f);
        for(int i = 0; i < trailBuffer.Length; i++)
        {
            trailBuffer[i] = defaultColor;
        }
        testTexture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        ren.material.mainTexture = testTexture;
    }

    void Update()
    {
        if (hasRootmotionScript)
        {
            if(rootmotionScript != null)
            {
                inputVector = rootmotionScript.Direction;
                dir = inputVector.normalized;
                magnitude = Mathf.Min(rootmotionScript.Speed, 1f) * maxRadius;
            }
            else
            {
                inputVector = rootmotionScriptTopdown.Direction;
                dir = inputVector.normalized;
                magnitude = Mathf.Min(rootmotionScriptTopdown.Speed, 1f) * maxRadius;
            }
        }
        else
        {
            inputVector = movement.action.ReadValue<Vector2>();
            dir = inputVector.normalized;
            magnitude = Mathf.Min(inputVector.magnitude, 1f) * maxRadius;
        }
        dotPosition.anchoredPosition = dir * magnitude;
        UpdateTexture();

        testTexture.SetPixels(trailBuffer, 0);
        testTexture.Apply(true, false);
    }

    private void UpdateTexture()
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
    }
}
