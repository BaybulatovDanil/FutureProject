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
    private Texture2D testTexture;
    [SerializeField]
    //private Material mat;
    private Image ren;


    /*[SerializeField]
    private RenderTexture rt;*/

    //private RectTransform dotPosition;

    void Awake()
    {
        maxRadius = dotPosition.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x / dotPosition.sizeDelta.x * (dotPosition.sizeDelta.x * 0.5f);
        for(int i = 0; i < trailBuffer.Length; i++)
        {
            trailBuffer[i] = defaultColor;
        }
        testTexture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        ren.material.mainTexture = testTexture;
        //mat.SetTexture("_MainTex", testTexture);
    }

    void Update()
    {
        inputVector = movement.action.ReadValue<Vector2>();
        dir = inputVector.normalized;
        magnitude = Mathf.Min(inputVector.magnitude, 1f) * maxRadius;
        //inputVector = Vector2.Min(inputVector, inputVector.normalized);
        dotPosition.anchoredPosition = dir * magnitude;
        //var f = rt.colorBuffer;
        UpdateTexture();
        /*for (int i = 0; i < trailBuffer.Length; i++)
        {
            testTexture.SetPixel(i % 256, i / 256, Color.green);
        }*/
        //testTexture.SetPixelData(trailBuffer, 0);
        testTexture.SetPixels(trailBuffer, 0);
        testTexture.Apply(true, false);
        //mat.SetTexture("_MainTex", testTexture);
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
            //Debug.Log($"pos:[{pos}], dotPos:[{dotPos}]");

            float alpha = condition2 ? 1f : (trailBuffer[i].a) * 0.995f;
            //trailBuffer[i].a = ((t==(i%256)) || (t == ((i+1) % 256)) || (t == ((i-1) % 256))) ? 1f: (trailBuffer[i].a + 1f) / 2f;
            trailBuffer[i].a = alpha;

            //trailBuffer[i] = new Color(trailBuffer[i].r, trailBuffer[i].g, trailBuffer[i].b, alpha);
            /*if(((t == (i % 256)) || (t == ((i + 1) % 256)) || (t == ((i - 1) % 256))))
            {
                Debug.Log($"i%256:[{i % 256}], Mathf.CeilToInt(Time.realtimeSinceStartup%256f): [{Mathf.CeilToInt(Time.realtimeSinceStartup % 256f)}]");
            }*/
            //Debug.Log($"i%256:[{i % 256}], Mathf.CeilToInt(Time.realtimeSinceStartup%256f): [{Mathf.CeilToInt(Time.realtimeSinceStartup % 256f)}]");
        }
    }
}
