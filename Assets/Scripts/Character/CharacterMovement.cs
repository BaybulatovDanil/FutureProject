using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject Character;

    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;

    private float vInput;
    private float hInput;

    private Animator playerAnimator;

    private void Start()
    {

        playerAnimator = Character.GetComponent<Animator>();

    }

    private void Update()
    {
        vInput = Input.GetAxis("Vertical") * moveSpeed;
        hInput = Input.GetAxis("Horizontal") * rotateSpeed;

        this.transform.Translate(Vector3.forward * vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * hInput * Time.deltaTime);

        if (vInput != 0)
        {
            playerAnimator.SetInteger("Move", 0);
        }
        else
        {
            playerAnimator.SetInteger("Move", 1);
        }

    }

}

