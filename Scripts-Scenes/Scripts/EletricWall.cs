using Godot;
using System;

public class EletricWall : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	//Will send a signal if a Body as entered inside a Area2d 
	private void _on_Area2D_body_entered(object body){
		GD.Print("Body: " + body + "has entered");
		//Verify if the body as entered is a KinematicBody2D
		if(body is KinematicBody2D){
			//Will cast body as a playerscript
			if(body is PlayerScript){
				PlayerScript ps = body as PlayerScript;
				//Call the method to take damage if the player entered inside a area2d
				ps.TakeDamage();
			}
		}
	}
}


