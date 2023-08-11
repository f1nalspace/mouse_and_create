using System;

namespace MouseAndCreate.Behaviors;

public class DirectionalMovement : IMovement
{
    public const byte TopIndex = 0;
    public const byte RightIndex = 8;
    public const byte BottomIndex = 16;
    public const byte LeftIndex = 24;

    public float Speed { get; set; } = 0.5f;
    public float Acceleration { get; set; } = 0.5f;
    public float Deceleration { get; set; } = 0.5f;

    public bool[] AllowedDirections => _allowedDirections;
    private readonly bool[] _allowedDirections = new bool[32];

    public bool[] InitialDirections => _initialDirections;
    private readonly bool[] _initialDirections = new bool[32];

    public bool AllowedTop { get => _allowedDirections[TopIndex]; set => _allowedDirections[TopIndex] = value; }
    public bool AllowedRight { get => _allowedDirections[RightIndex]; set => _allowedDirections[RightIndex] = value; }
    public bool AllowedBottom { get => _allowedDirections[BottomIndex]; set => _allowedDirections[BottomIndex] = value; }
    public bool AllowedLeft { get => _allowedDirections[LeftIndex]; set => _allowedDirections[LeftIndex] = value; }

    public DirectionalMovement()
    {
        Array.Clear(_allowedDirections, 0, _allowedDirections.Length);
    }

    public void ClearAllowed() => Array.Clear(_allowedDirections, 0, _allowedDirections.Length);
    public void AllAllowed() => Array.Fill(_allowedDirections, true, 0, _allowedDirections.Length);
    public void SetAllowed(byte index, bool value) => _allowedDirections[index] = value;
    public bool IsAllowed(byte index) => _allowedDirections[index];

    public void ClearInitial() => Array.Clear(_initialDirections, 0, _initialDirections.Length);
    public void AllInitial() => Array.Fill(_initialDirections, true, 0, _initialDirections.Length);
    public void SetInitial(byte index, bool value) => _initialDirections[index] = value;
    public bool IsInitial(byte index) => _initialDirections[index];

    public IMovement Clone()
    {
        DirectionalMovement result = new DirectionalMovement()
        {
            Speed = Speed,
            Acceleration = Acceleration,
            Deceleration = Deceleration,
        };
        result._allowedDirections.CopyTo(result._allowedDirections, 0);
        result._initialDirections.CopyTo(result._initialDirections, 0);
        return result;
    }
}
