using Godot;
using System;

public class Player1 : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private AnimatedSprite _animatedSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	  float Speed = 1;
		
		if (Input.IsKeyPressed((int)KeyList.D))
		{
			this.Position += new Vector2(Speed,0);
			 _animatedSprite.Play("WalkRight");
		}
		else if (Input.IsKeyPressed((int)KeyList.A))
		{
			this.Position += new Vector2(-Speed,0);
			_animatedSprite.Play("WalkLeft");
		}
		else
		{
			 _animatedSprite.Play("Wait");
		}
  }
}
