using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Gameplay : Node2D
{
	private Player _playerBody;
	private Object _objectBody;

	public PackedScene packedScene;

	public float gameplayTimer = 0;
	public float spawnTime = 10000;


	public int playerLinearSpeed;

	public int playerRotationSpeed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		packedScene = ResourceLoader.Load("res://scenes/Object.tscn") as PackedScene;

		_playerBody = GetNode<Player>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	// 	Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();
		// playerLinearSpeed = _playerBody.linearSpeed;
		// playerRotationSpeed = _playerBody.rotationSpeed;

		gameplayTimer += playerLinearSpeed * (float)delta;

		// Instantiate an object after a random period of time
		// Timer is based on Speed variable
		if (gameplayTimer > spawnTime)
		{
			Object newObject = packedScene.Instantiate() as Object;
			newObject.GlobalPosition = new Vector2(1000, -500);

			AddChild(newObject);

			GD.Print(gameplayTimer);

			gameplayTimer = 0;

			spawnTime = GD.RandRange(7500, 9500);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if ( @event.IsActionPressed("ui_up") )
		{
			playerLinearSpeed += 500;

			if (playerLinearSpeed > 2500)
			{
				playerLinearSpeed = 2500;
			}
		}

		if ( @event.IsActionPressed("ui_down"))
		{
			playerLinearSpeed -= 500;

			if (playerLinearSpeed < 0)
			{
				playerLinearSpeed = 0;
			}
		}

		if ( @event.IsActionPressed("ui_left"))
		{
			playerRotationSpeed += 1000;

			if (playerRotationSpeed > 5000)
			{
				playerRotationSpeed = 5000;
			}
		}

		if ( @event.IsActionPressed("ui_right"))
		{
			playerRotationSpeed -= 1000;

			if (playerRotationSpeed < -5000)
			{
				playerRotationSpeed = -5000;
			}
		}
	}
}
