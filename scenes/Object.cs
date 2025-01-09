using Godot;
using System;
using System.Dynamic;
using System.Reflection.Metadata;

public partial class Object : CharacterBody2D
{
	private int _rotationSpeed = 0;
	private int _linearSpeed = 0;
	private Vector2 _rotationVelocity;
	private Vector2 _linearVelocity;
	private Vector2 _linearVector;

	private Gameplay _gameplayNode;


	public override void _Ready()
	{
		_gameplayNode = GetParent<Gameplay>();

		SetLinearVector(new Vector2(-2,1));
	}

	public override void _PhysicsProcess(double delta)
	{
		_rotationSpeed = _gameplayNode.playerRotationSpeed;
		_linearSpeed = _gameplayNode.playerLinearSpeed;

		_rotationVelocity = GetRotationVelocity(_rotationSpeed, delta);
		_linearVelocity = GetLinearVelocity(_linearSpeed, _rotationSpeed, delta);

		Velocity =  _rotationVelocity + _linearVelocity;

		MoveAndSlide();
	}

	public Vector2 GetLinearVector()
	{
		return _linearVector;
	}

	public void SetLinearVector(Vector2 vector)
	{
		_linearVector = vector;
	}

	public Vector2 GetLinearVelocity(int linearSpeed, int rotationSpeed, double delta)
	{
		Vector2 linearVector, linearVelocity;

		float X, Y; 

		// This is the angle (in Degrees) that will be taken into consideration for rotation
		const float ROTATION_ANGLE = 5;

		float currentTheta;

		linearVector = GetLinearVector();

		// Update linear vector if rotation is active
		// This way, the object will maintain its current trajectory
		if ((int)rotationSpeed > 0)
		{
			currentTheta = Mathf.DegToRad(ROTATION_ANGLE) * (float)delta;

			X = (linearVector.X * Mathf.Cos(currentTheta) - linearVector.Y * Mathf.Sin(currentTheta)) ;
			Y = (linearVector.X * Mathf.Sin(currentTheta) + linearVector.Y * Mathf.Cos(currentTheta)) ;		

			linearVector.X = X;
			linearVector.Y = Y;	
		}

		// Update linear vector if rotation is active
		// This way, the object will maintain its current trajectory
		if ((int)rotationSpeed < 0)
		{
			currentTheta = Mathf.DegToRad((-1) * ROTATION_ANGLE) * (float)delta;

			X = (linearVector.X * Mathf.Cos(currentTheta) - linearVector.Y * Mathf.Sin(currentTheta)) ;
			Y = (linearVector.X * Mathf.Sin(currentTheta) + linearVector.Y * Mathf.Cos(currentTheta)) ;

			linearVector.X = X;
			linearVector.Y = Y;
		}

		// Save the current vector
		SetLinearVector(linearVector);
	
		// Calculate velocity
		linearVelocity = linearVector * linearSpeed * (float)delta;

		return linearVelocity;
	}

	public Vector2 GetRotationVelocity(int rotationSpeed, double delta)
	{
		Vector2 rotationVector, rotationVelocity;

		// This is the angle (in Degrees) that will be taken into consideration for rotation
		const float ROTATION_ANGLE = 5;

		// Angular speed should scale with the distance from the origin of the ellipse
		float angularSpeed;

		// Calculate the current ellipse trajectory for rotation
		//This calculation is valid only when ellipse X is double of ellipse Y
		float ellipseY = Mathf.Sqrt((Mathf.Pow(GlobalPosition.X, 2) + 4 * Mathf.Pow(GlobalPosition.Y, 2)) / 4);
		float ellipseX = ellipseY * 2;

		// Current rotation angle (in radians) relative to scene origin (0, 0)
		// The Y component is multiplied by 2 to take into consideration the elliptic trajectory
		float currentTheta = Mathf.Atan2(-GlobalPosition.Y * 2, GlobalPosition.X);

		// Offset current angle by ROTATION_ANGLE
		if ((int)rotationSpeed > 0)
		{
			currentTheta += Mathf.DegToRad(ROTATION_ANGLE);
		}
		if ((int)rotationSpeed < 0)
		{
			currentTheta -= Mathf.DegToRad(ROTATION_ANGLE);
		}

		// Rotate object by new angle Theta on the current ellipse
		rotationVector.X = ellipseX * Mathf.Cos(currentTheta);
		rotationVector.Y = -ellipseY * Mathf.Sin(currentTheta);

		angularSpeed = Mathf.Abs(rotationSpeed) * (Mathf.Abs(rotationVector.X) + Mathf.Abs(rotationVector.Y)) * 10 / 750;
		// angularSpeed = Mathf.Abs(rotationSpeed) * 10;


		rotationVelocity = (rotationVector - GlobalPosition).Normalized() * (float)delta * angularSpeed;

		return rotationVelocity;
	}
}

