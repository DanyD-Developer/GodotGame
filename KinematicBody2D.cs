using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	private AnimatedSprite _animatedSprite;
	public int JumpImpulse = -400;
	 const float gravity = 200.0f;
	  const int walkSpeed = 200;
	   bool jumping = false;
	Vector2 velocity;
	//private Vector3 _velocity = Vector3.Zero;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public void GetInput(){
		bool jump = Input.IsActionPressed("ui_select");
		if(jump && IsOnFloor()){
			jumping = true;
			velocity.y = JumpImpulse;
		 	_animatedSprite.Play("Jump");
		 }
	}
	public override void _Process(float _delta)
	{
		 if(Input.IsKeyPressed((int)KeyList.D) && Input.IsKeyPressed((int)KeyList.Shift)){
			velocity.x = walkSpeed + 100;
			_animatedSprite.Play("RunRight");
		}
		else if(Input.IsKeyPressed((int)KeyList.A) && Input.IsKeyPressed((int)KeyList.Shift)){
			velocity.x = -walkSpeed + 100;
			_animatedSprite.Play("RunLeft");
		}
		else if (Input.IsKeyPressed((int)KeyList.A)){
			velocity.x = -walkSpeed;
			_animatedSprite.Play("WalkLeft");
		}
			GetInput();
			velocity.y += gravity * _delta;
		if(jumping && IsOnFloor()){
			
			jumping = false;
			velocity = MoveAndSlide(velocity, new Vector2(0, -1));
		}
		else if (Input.IsKeyPressed((int)KeyList.D)){
			velocity.x = walkSpeed;
			_animatedSprite.Play("WalkRight");	
		}
		 else if(Input.IsMouseButtonPressed((int)ButtonList.Left)){
			 _animatedSprite.Play("Attack1");
		 }
		else{
			_animatedSprite.Play("Wait");
			velocity.x = 0;
		}

		MoveAndSlide(velocity, new Vector2(0, -1));

		// float Speed = 1;
		// velocity.y += _delta * gravity;
		// if(Input.IsKeyPressed((int)KeyList.D) && Input.IsKeyPressed((int)KeyList.Shift)){
		// 	this.Position += new Vector2(Speed * 2,0);
		// 	_animatedSprite.Play("RunRight");	
		// }
		// else if(Input.IsKeyPressed((int)KeyList.A) && Input.IsKeyPressed((int)KeyList.Shift)){
		//    	this.Position += new Vector2(-Speed * 2,0);
		//    _animatedSprite.Play("RunLeft");
		// }
		// else if (Input.IsKeyPressed((int)KeyList.D))
		// {
		// 	this.Position += new Vector2(Speed,0);
		// 	_animatedSprite.Play("WalkRight");
			
		// }
		// else if (Input.IsKeyPressed((int)KeyList.A))
		// {
		// 	this.Position += new Vector2(-Speed,0);
		// 	 _animatedSprite.Play("WalkLeft");
		// }
		// else if(Input.IsActionJustPressed("jump")){
		// 	_animatedSprite.Play("Jump");
		// }
		// else if(Input.IsMouseButtonPressed((int)ButtonList.Left)){
		// 	 _animatedSprite.Play("Attack1");
		// }
		// else
		// {
		// 	 _animatedSprite.Play("Wait");
		// }

	}
}
