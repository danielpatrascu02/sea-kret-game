using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public int rotationSpeed = 0;
	public int linearSpeed = 0;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if ( Input.IsActionJustPressed("ui_up") == true)
		{
			linearSpeed += 100;

			if (linearSpeed > 300)
			{
				linearSpeed = 300;
			}
		}

		if ( Input.IsActionJustPressed("ui_down") == true)
		{
			linearSpeed -= 100;

			if (linearSpeed < 0)
			{
				linearSpeed = 0;
			}
		}

		if ( Input.IsActionJustPressed("ui_left") == true)
		{
			rotationSpeed += 100;

			if (rotationSpeed > 300)
			{
				rotationSpeed = 300;
			}
		}

		if ( Input.IsActionJustPressed("ui_right") == true)
		{
			rotationSpeed -= 100;

			if (rotationSpeed < -300)
			{
				rotationSpeed = -300;
			}
		}
	}
}
