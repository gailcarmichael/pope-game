using Godot;
using System;
using StoryEngine;

public class TestSprite : Sprite
{
    private int Speed = 400;
    private float AngularSpeed = Mathf.Pi;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Hello, world!");
        GD.Print((new Story()).Test);
    }

    public override void _Process(float delta)
    {
        Rotation += AngularSpeed * delta;
        var velocity = Vector2.Up.Rotated(Rotation) * Speed;

        Position += velocity * delta;

    }
}
