using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController2D controller;
	public CharacterInteraction interactions;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;

	void Update()
	{
		if(!ShowTextUI.instance.CanMove())
        {
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F))
				ShowTextUI.instance.SkipText();


			animator.SetBool("Walk", false);
			horizontalMove = 0f;
			return;
        }
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetBool("Walk", horizontalMove != 0f);

		if (Input.GetKeyDown(KeyCode.F))
			Interaction();
		else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
			TextEditorUI.instance.TextEditorClicked();
	}


	void FixedUpdate()
	{
		controller.Move(TextEditorUI.instance.showedUI ? 0f : horizontalMove * Time.fixedDeltaTime);
	}

	void Interaction()
    {
		if (!TextEditorUI.instance.showedUI)
			interactions.MakeInteractions();
	}

}