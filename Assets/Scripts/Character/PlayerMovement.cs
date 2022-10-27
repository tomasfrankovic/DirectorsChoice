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


	public void PlayStepSound()
    {
		SoundManager.instance.PlayStep();
	}

	void Update()
	{
		if(!ShowTextUI.instance.CanMove() || CutsceneUI.instance.isCutscene)
        {
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F))
				ShowTextUI.instance.SkipText();


			animator.SetBool("Walk", false);
			horizontalMove = 0f;
			return;
        }
		if (StartWindows.instance.IsShowedUI())
		{
			animator.SetBool("Walk", false);
			horizontalMove = 0f;
		}
		else
		{
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetBool("Walk", horizontalMove != 0f);
		}

		if (Input.GetKeyDown(KeyCode.F))
			Interaction();
		else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)) && !StartWindows.instance.animLock)
			StartWindows.instance.TextEditorClicked();
	}


	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime);
	}

	void Interaction()
    {
		if (!StartWindows.instance.IsShowedUI())
			interactions.MakeInteractions();
	}

}