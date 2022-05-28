using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
	// Called when the node enters the scene tree for the first time.
	private AnimatedSprite _animatedSprite;
	public float JumpImpulse = 700;
	 const float gravity = 8.81f;
	  const float Speed = 200.0f;
	  public float Mass = 1f;
	  private float friction = 0.1f;
	  private float accelaration = 0.25f;
	 // public float Move_Speed = 30f;
	 private RayCast2D groundRay;


	Vector2 velocity = new Vector2();
	//private Vector3 _velocity = Vector3.Zero;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public override void _Process(float _delta)
	{
		//All Movement Character and Jump
		CharacterMovement(_delta);
		//Gravity Defenitinion
		velocity.y += gravity * Mass;
		//Allow the Character to Move 
		MoveAndSlide(velocity, Vector2.Up);

		//velocity.x = 0;
	}
	private void CharacterMovement(float delta){
		int direcetion = 0;

		if(Input.IsKeyPressed((int)KeyList.Shift) && Input.IsKeyPressed((int)KeyList.D)){
			velocity.x = Speed + 100;
			_animatedSprite.Play("RunRight");
		}
		else if(Input.IsKeyPressed((int)KeyList.Shift) && Input.IsKeyPressed((int)KeyList.A)){
			velocity.x = -Speed - 100;
			_animatedSprite.Play("RunLeft");
		}
		else if (Input.IsKeyPressed((int)KeyList.A)){
			direcetion -= 1;
			_animatedSprite.Play("WalkLeft");
		}
		else if (Input.IsKeyPressed((int)KeyList.D)){
			direcetion += 1;
			_animatedSprite.Play("WalkRight");	
		}
		else if(Input.IsActionJustPressed("jump")){
			velocity.y -= JumpImpulse; 
		}
		else{
			_animatedSprite.Play("Wait");
			velocity.x = 0;
		}

		if(direcetion != 0){
			velocity.x = Mathf.Lerp(velocity.x,direcetion*Speed,accelaration);
		}else{
			velocity.x = Mathf.Lerp(velocity.x,0,friction);
		}
		
	}
}
