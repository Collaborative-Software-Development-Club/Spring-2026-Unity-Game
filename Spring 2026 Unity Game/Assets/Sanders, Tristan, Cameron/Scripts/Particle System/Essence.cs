using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Essence
{
    const float friction = 0.05f;
    private Vector2 velocity;
    public EssenceType Type { get; set; }

    public static readonly Essence Empty = new Essence(Vector2Int.zero, EssenceType.none);

    public Essence(Vector2Int velocity, EssenceType type)
    {
        this.velocity = velocity;
        Type = type;
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity;
    }

    public Vector2Int GetVelocity()
    {
        return new Vector2Int(Mathf.CeilToInt(velocity.x), Mathf.CeilToInt(velocity.y));
    }

    /// <summary>
    /// Returns the position of where the essence should be after a step
    /// </summary>
    /// <returns></returns>
    public Vector2Int SimulateEssence(Vector2Int oldLocation, int gridWidth, int gridHeight)
    {
        Vector2Int newLocation = oldLocation + GetVelocity();
        ApplyFriction();
        if (GridUtility.IsInBounds(newLocation,gridWidth,gridHeight))
        {
            return newLocation;
        }
        else
        {
            return oldLocation;
        }

    }

    //Slows down essence linearly by the friction amount
    private void ApplyFriction()
    {
        float velX = Mathf.Sign(velocity.x) * (Mathf.Abs(velocity.x) - friction);
        float velY = Mathf.Sign(velocity.y) * (Mathf.Abs(velocity.y) - friction);
        velocity = new Vector2(velX,velY);
    }



    public override bool Equals(object obj)
    {
        if (obj is Essence other)
        {
            return GetVelocity().Equals(other.GetVelocity()) && Type == other.Type;
        }
        return false;
    }


    public override int GetHashCode()
    {
        return System.HashCode.Combine(velocity, Type);
    }


}
