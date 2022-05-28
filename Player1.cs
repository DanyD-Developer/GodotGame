using Godot;
using System;

public class Player1 : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	const float gravity = 250.0f;
	const int walkSpeed = 200;
	Vector2 velocity;
	private AnimatedSprite _animatedSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
		//Gravity Defenitinion
		velocity.y += gravity * delta;

		//Moving Method
		Moving();

		MoveAndSlide(velocity, Vector2.Up);
		velocity.x = 0;
  }
  public void Moving(){
		if(Input.IsKeyPressed((int)KeyList.Shift)){
			_animatedSprite.Play("Ball");
		}
		else if (Input.IsKeyPressed((int)KeyList.D))
		{
			velocity.x = walkSpeed;
			_animatedSprite.Play("WalkRight");
		}
		else if (Input.IsKeyPressed((int)KeyList.A))
		{
			velocity.x = -walkSpeed;
			_animatedSprite.Play("WalkLeft");
		}
		else
		{
			 _animatedSprite.Play("Wait");
		}
  }
}
