using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementRootMotion : MonoBehaviour
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

        Vector3 velocity = playerAnimator.deltaPosition;

        if (vInput != 0)
        {
            playerAnimator.SetInteger("Speed", 0);
        }
        else
        {
            playerAnimator.SetInteger("Speed", 1);
        }

    }
}
