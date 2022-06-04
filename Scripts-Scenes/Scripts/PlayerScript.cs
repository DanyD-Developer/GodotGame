using Godot;
using System;

public class PlayerScript : Godot.KinematicBody2D
{
	// Variables.
	[Export]
	public float JumpHeight = 350f;

	[Export]
	public int gravity = 500;

	[Export]
	public float MoveSpeed = 200.0f;

	[Export]
	private float friction = 0.1f;

	[Export]
	private float accelaration = 0.50f;

	[Export]
	private int dashSpeed = 500;

	private float dashTimer = 0.2f;
	private float dashTimerReset = 0.2f;
	private AnimatedSprite _animatedSprite;
	private bool isDashing = false;
	private bool isDashAvaible = true;
	private bool isWallJumping = false;
	private float WallJumpTimer = 0.45f;
	private float WallJumpTimerReset = 0.45f;


	Vector2 velocity = new Vector2();
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public override void _Process(float _delta)
	{
		//All Movement Character and Jump
		CharacterMovement(_delta);
		//Character Run
		CharacterRun(_delta);
		//Character Dash Method 
		CharacterDash(_delta); 
		//Character Wall Jump
		WallJump(_delta);
		//Gravity Defenitinion multiplying gravaty by delta
		velocity.y += gravity * _delta;
		//Allow the Character to Move 
		MoveAndSlide(velocity, Vector2.Up);

	}
	private void CharacterMovement(float delta){
		int direcetion = 0;
		//Verify if is Dashing
			if(!isDashing && !isWallJumping){

				if (Input.IsActionPressed("A")){
					//give direction to left 
					direcetion -= 1;	
				}
				if (Input.IsActionPressed("D")){
					//give direction to right
					direcetion += 1;
				}

				if(direcetion != 0){
					//Here MathF.Lerp is use to make the player accelarate slowly 
					velocity.x = Mathf.Lerp(velocity.x,direcetion*MoveSpeed,accelaration);
				}else{
					//Her MathF.Lerp is used to make the player stop slowly 
					velocity.x = Mathf.Lerp(velocity.x,0,friction);
				}
			
			}
		
		
		//Verify if is on floor
		if(IsOnFloor()){
			if(Input.IsActionJustPressed("jump")){

				velocity.y = -JumpHeight; 
			}
			isDashAvaible = true;
		}
		
		
	}
	private void CharacterRun(float delta){
		if(Input.IsActionPressed("D") &&  Input.IsActionPressed("Run")){
			velocity.x = MoveSpeed * 2;
		}
		if(Input.IsActionPressed("A") &&  Input.IsActionPressed("Run")){
			velocity.x = -MoveSpeed * 2;
		}
	}
	private void WallJump(float delta){
		//Verify is "Space" Button are pressed and verify if the RaysCast2D is Colliding with Something
		if(Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastLeft").IsColliding()){
			velocity.y = -JumpHeight;
			velocity.x = -JumpHeight;
			isWallJumping = true;
		}
		if(Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastRight").IsColliding()){
			velocity.y = -JumpHeight;
			velocity.x = JumpHeight;
			isWallJumping = true;
		}

		if(isWallJumping){
			//Reduce the WallJumptimer frame by frame until WallJumpTimer is 0
			WallJumpTimer -= delta;
			//When WallJumpTimer is 0 the Character reset Dash and Reset Velocity
			if(WallJumpTimer <= 0){
				isWallJumping = false;
				WallJumpTimer = WallJumpTimerReset;
			}
		}
	}
	//!Temporary Dash Method
	private void CharacterDash(float delta){
		if(isDashAvaible){
			if(Input.IsActionJustPressed("Dash"))
			{
				//Make a dash to Left
				if (Input.IsActionPressed("A")){
					//Increasing the x axis with dash speed for left
					velocity.x = -dashSpeed;
					isDashing = true;
				}
				//Make a dash to Right
				if (Input.IsActionPressed("D")){
					//Increasing the x axis with dash speed for right
					velocity.x = dashSpeed;
					isDashing = true;
				}
				//Make a dash to Up
				if (Input.IsActionPressed("W")){
					//Increasing the x axis with dash speed for left
					velocity.y = -dashSpeed;
					isDashing = true;
				}
				//Make a dash to Right diagonal
				if (Input.IsActionPressed("D") && Input.IsActionPressed("W")){
					//Increasing the x axis with dash speed for right
					velocity.x = dashSpeed;
					velocity.y = -dashSpeed;
					isDashing = true;
				}
				//Make a dash to Left diagonal
				if (Input.IsActionPressed("A") && Input.IsActionPressed("W")){
					//Increasing the x axis with dash speed for right
					velocity.x = -dashSpeed;
					velocity.y = -dashSpeed;
					isDashing = true;
				}

				//Equals dashtime to dashtimereset to reset the dash time
				dashTimer = dashTimerReset;
				isDashAvaible = false;
			}
		}
		
		//Reset Logic to reset the dash
		if(isDashing){
			//Reduce the dashtimer frame by frame until dashtimer is 0
			dashTimer -= delta;
			//When dash Timer is 0 the Character reset Dash and Reset Velocity
			if(dashTimer <= 0){
				isDashing= false;
				velocity = new Vector2(0,0);
			}
		}
	}
}
