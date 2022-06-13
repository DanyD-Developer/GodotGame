using Godot;
using System;

public class GameManager : Node2D
{
	[Export]
	public Position2D RespawnPoint;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.RespawnPoint = this.GetNode<Position2D>("RespawnPoint");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	public void RespawnPlayer(){
		//When he dies the player will spawn on the position 
		PlayerScript ps = GetNode<PlayerScript>("Player");
		ps.GlobalPosition = RespawnPoint.GlobalPosition;
		ps.RespawnPlayer();
	} 

	//if the Player death signal is emited the character will Respawn
	private void _on_Player_Death()
	{
		RespawnPlayer();
	}   
}



