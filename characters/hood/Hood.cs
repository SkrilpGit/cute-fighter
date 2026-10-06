using Godot;
using System;

public partial class Hood : CharacterBody2D
{
	[Export] public float Speed = 10f;

	private AnimationPlayer _anim;
	private Sprite2D _sprite;

	private bool walking = false;
	private bool dash = true;
	private int dash_dir = 1;
	private float HVelocity;

	private Timer _dashTimer = new Timer();
	
	public override void _Ready()
	{
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		_anim.AnimationFinished += OnAnimationFinished;
		_anim.Play("idle");

		_sprite = GetNode<Sprite2D>("Sprite2D");

		AddChild(_dashTimer);
		_dashTimer.WaitTime = 0.083;
		_dashTimer.OneShot = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		float in_dir = Input.GetAxis("move_left", "move_right");
		
		if (in_dir != 0){
			GD.Print(in_dir);
			if (!walking){
				if (!_dashTimer.IsStopped()){
					dash = true;
				}

				_anim.Play("walk_start");
				walking = true;
			}
		}
		if (in_dir < 0) {
			_sprite.FlipH = true;
			
			if (dash && dash_dir != in_dir){
				dash = false;
			}
			dash_dir = -1;
		} else if (in_dir > 0) {
			_sprite.FlipH = false;
			
			if (dash && dash_dir != in_dir){
				dash = false;
			}
			dash_dir = 1;
		} else {
			_anim.Play("idle");
			
			if (walking && !dash){
				_dashTimer.Start();
			}

			walking = false;
			dash = false;
		}
		
		HVelocity = in_dir * Speed;
		if (dash){ HVelocity *= 2;}
		Velocity = new Vector2(HVelocity, Velocity.Y);
		MoveAndSlide();

	}
	
	private void OnAnimationFinished(StringName animName)
	{
		//GD.Print("hi");
		if (animName == "idle"){
			_anim.Play("idle");
		}
		if (animName == "walk_start"){
			_anim.Play("walk_loop");
		}
	}
}
