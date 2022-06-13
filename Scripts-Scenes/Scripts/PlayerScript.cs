using Godot;
using System;

public class PlayerScript : Godot.KinematicBody2D
{
	// Variables.
	[Export]
	public float JumpHeight = 200f;

	[Export]
	public int gravity = 430;

	[Export]
	public float MoveSpeed = 200.0f;

	[Export]
	private float friction = 0.1f;

	[Export]
	private float accelaration = 0.50f;

	[Export]
	private int dashSpeed = 500;

	[Export]
	private float ClimbSpeed = 100f;

	[Export]
	public PackedScene GhostPlayerInstance;

	private float dashTimer = 0.2f;
	private float dashTimerReset = 0.2f;
	private AnimatedSprite _animatedSprite;
	private bool isDashing = false;
	private bool isDashAvaible = true;
	private bool isWallJumping = false;
	private float WallJumpTimer = 0.45f;
	private float WallJumpTimerReset = 0.45f;
	private bool CanClimb = false;
	private bool isClimbing = false;
	private float climbTimer = 5f;
	private float climbTimerReset = 5f;
	private bool isRunning = false;
	private bool isWalking = false;
	private bool IsinAir = false;
	public int Health = 3;
	private int facingDirecetion = 0;
	private int facingRundirecetion = 0;
	private Boolean isTakingDamage = false;

	//Creating a signal
	[Signal]
	public delegate void Death();

	Vector2 velocity = new Vector2();
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		//GetNode("LadderTop").Connect("body_entered",this,nameof(_on_LadderTop_body_entered));
	}

	public override void _Process(float _delta)
	{
		if(Health > 0){
			//All Movement Character and Run 
			CharacterMovement(_delta);
			//Character Dash Method 
			CharacterDash(_delta);
			//Character Wall Jump
			WallJump(_delta);
			//CharacterClibing Method
			CharacterClimbing(_delta);
			//Jump Method
			CharacterJump(_delta);
			//Gravity Defenitinion multiplying gravity by delta
			if (!isClimbing)
			{
				velocity.y += gravity * _delta;
			}
			//Allow the Character to Move 
			MoveAndSlide(velocity, Vector2.Up);
		}
	}
	private void CharacterMovement(float delta)
	{
		facingDirecetion = 0;
		//Verify if is Dashing
		if (!isDashing && !isWallJumping)
		{
			//if is taking damage he cant move 
			if(!isTakingDamage){
				if (Input.IsActionPressed("A"))
				{
					//give direction to left 
					facingDirecetion -= 1;
					_animatedSprite.FlipH = true;
				}
				if (Input.IsActionPressed("D"))
				{
					//give direction to right
					facingDirecetion += 1;
					_animatedSprite.FlipH = false;
				}
			}

			if (facingDirecetion != 0)
			{
				//Here MathF.Lerp is use to make the player accelarate slowly 
				velocity.x = Mathf.Lerp(velocity.x, facingDirecetion * MoveSpeed, accelaration);
				//Run Method Call
				if(Input.IsActionPressed("Run")){
					CharacterRun(delta);
				}
				
				isWalking = true;
				if(!IsinAir){
					_animatedSprite.Play("Walk");
				}
				
				
			}
			else
			{
				
				//Her MathF.Lerp is used to make the player stop slowly 
				velocity.x = Mathf.Lerp(velocity.x, 0, friction);

				//Reset the Taking damage and allow as to move after receive damage and we assume 
				//when the velocity is smaller than 5 and greater than -5 we assume he stopped and plays the wait animation
				if(velocity.x < 5 && velocity.x > -5){
					if(!IsinAir)
					_animatedSprite.Play("Wait");
					
					isTakingDamage = false;
				}
			
			}

		}

	}
	//TODO Fix the Character Run the problem is not making the animation
	private void CharacterRun(float delta)
	{
		facingRundirecetion = 0;
		if(IsOnFloor()){	
			if(!isTakingDamage){	
				if (Input.IsActionPressed("A"))
				{
					facingRundirecetion -= 1;
					_animatedSprite.FlipH = true;
				}
				if (Input.IsActionPressed("D"))
				{
					facingRundirecetion += 1;
					_animatedSprite.FlipH = false;
				}
			}
		
			if (facingRundirecetion != 0)
			{
				//Here MathF.Lerp is use to make the player accelarate slowly 
				velocity.x = Mathf.Lerp(velocity.x, facingRundirecetion * MoveSpeed*2, accelaration);

				if(!IsinAir){
				_animatedSprite.Play("RunRight");
				}
				isRunning = true;
				
			}
			else
			{
				//Her MathF.Lerp is used to make the player stop slowly
				velocity.x = Mathf.Lerp(velocity.x, 0, friction);

				//Reset the Taking damage and allow as to move after receive damage and we assume 
				//when the velocity is smaller than 5 and greater than -5 we assume he stopped and plays the wait animation
				if(velocity.x < 5 && velocity.x > -5){
					if(!IsinAir)
						_animatedSprite.Play("Wait");
					
					isTakingDamage = false;
				}

			}
		}
		
	}
	private void CharacterJump(float delta){
		//Verify if is on floor
		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("jump"))
			{
				velocity.y = -JumpHeight;
				_animatedSprite.Play("Jump_Animation");
				IsinAir = true;
			}
			else{
				IsinAir = false;
			}
			isDashAvaible = true;
			//Reseting the Climb
			CanClimb = true;
		}

	}
	private void WallJump(float delta)
	{
		if(!IsOnFloor()){
			//Verify is "Space" Button are pressed and verify if the RaysCast2D is Colliding with Something
			if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastLeft").IsColliding())
			{
				//1 Option Jump Diagnolly for Left
				velocity.y = -JumpHeight;
				velocity.x = JumpHeight;

				isWallJumping = true;
				_animatedSprite.FlipH = false;
			}
			if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastRight").IsColliding())
			{
				//1 Option Jump Diagnolly for Right
				velocity.y = -JumpHeight;
				velocity.x = -JumpHeight;

				isWallJumping = true;
				_animatedSprite.FlipH = true;
			}

			if (isWallJumping)
			{
				//Reduce the WallJumptimer frame by frame until WallJumpTimer is 0
				WallJumpTimer -= delta;
				//When WallJumpTimer is 0 the Character reset Dash and Reset Velocity
				if (WallJumpTimer <= 0)
				{
					isWallJumping = false;
					WallJumpTimer = WallJumpTimerReset;
				}
			}
		}
		
	}

	//!	1º Climb Attempt 
	private void CharacterClimbing(float delta)
	{
		if (Input.IsActionPressed("Climb") && (GetNode<RayCast2D>("RayCastLeft").IsColliding() || GetNode<RayCast2D>("RayCastRight").IsColliding() ||
		GetNode<RayCast2D>("RayCastRightClimb").IsColliding() || GetNode<RayCast2D>("RayCastLeftClimb").IsColliding()))
		{
			if (CanClimb && !isWallJumping)
			{
				isClimbing = true;
				if (Input.IsActionPressed("W"))
				{
					velocity.y = -ClimbSpeed;
				}
				else if (Input.IsActionPressed("S"))
				{
					velocity.y = ClimbSpeed;
				}
				else
				{
					velocity = new Vector2(0, 0);
				}
			}
			else
			{
				isClimbing = false;
			}

		}
		else
		{
			isClimbing = false;
		}

		//Will force the character to fall and there not be able to climb
		if(CanClimb){
			//Reduce the climbtimer frame by frame until climbtimer is 0
			climbTimer -= delta;
			//When  climbTimer is 0 the Character reset Climb and Reset Velocity
			if(climbTimer <= 0){
				isClimbing = false;
				CanClimb = false;
				climbTimer = climbTimerReset;
			}
		}

	}
	//!Temporary Dash Method
	private void CharacterDash(float delta)
	{

		if (isDashAvaible)
		{
			if (Input.IsActionJustPressed("Dash"))
			{
				//Make a dash to Left
				if (Input.IsActionPressed("A"))
				{
					//Increasing the x axis with dash speed for left
					velocity.x = -dashSpeed;
					isDashing = true;
				}
				//Make a dash to Right
				if (Input.IsActionPressed("D"))
				{
					//Increasing the x axis with dash speed for right
					velocity.x = dashSpeed;
					isDashing = true;
				}
				//Make a dash to Up
				if (Input.IsActionPressed("W"))
				{
					//Increasing the x axis with dash speed for left
					velocity.y = -dashSpeed;
					isDashing = true;
				}
				//Make a dash to Right diagonal
				if (Input.IsActionPressed("D") && Input.IsActionPressed("W"))
				{
					//Increasing the x axis with dash speed for right
					velocity.x = dashSpeed;
					velocity.y = -dashSpeed;
					isDashing = true;
				}
				//Make a dash to Left diagonal
				if (Input.IsActionPressed("A") && Input.IsActionPressed("W"))
				{
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
		if (isDashing)
		{
			//Reduce the dashtimer frame by frame until dashtimer is 0
			dashTimer -= delta;

			//Will spawn the Sprite inside a character
			GhostPlayer ghost = GhostPlayerInstance.Instance() as GhostPlayer;
			Owner.AddChild(ghost);
			ghost.GlobalPosition = this.GlobalPosition;
			ghost.SetHValue(_animatedSprite.FlipH = true);

			//When dash Timer is 0 the Character reset Dash and Reset Velocity
			if (dashTimer <= 0)
			{
				isDashing = false;
				velocity = new Vector2(0, 0);
			}
		}
	}

	public void TakeDamage(){
		GD.Print("Player is taking Damage");
		Health -= 1;
		
		GD.Print("Current Health "+ Health);

		if(!isRunning){
			//Will get knocked for behind or front when Walking
			velocity = MoveAndSlide(new Vector2(500f * -facingDirecetion ,-80), Vector2.Up);
			isTakingDamage = true;
			_animatedSprite.Play("Hurt");
		}
		else{
			//Will get knocked for behind or front when Running
			velocity = MoveAndSlide(new Vector2(500f * -facingRundirecetion ,-80), Vector2.Up);
			isTakingDamage = true;
			_animatedSprite.Play("Hurt");
		}
		if(Health <= 0){
			Health = 0;
			_animatedSprite.Play("Death");
			GD.Print("Player has died");
		}
		
	}

	//Verify give a signal if a animation as finished
	private void _on_AnimatedSprite_animation_finished()
	{
		//Verify if the animation is death animation
		if(_animatedSprite.Animation == "Death"){
			_animatedSprite.Stop();
			Hide();
			GD.Print("Animation Finished");
			EmitSignal(nameof(Death));
		}
	}

	public void RespawnPlayer(){
		Show();
		Health = 3;
	}
}




