namespace Common.Animation;

using UnityEngine;
using System;

public sealed class ShaderColorPropertyAnimation : ObjectAnimation<Material>
{
    private readonly int _propertyID;
    private readonly Func<Color, Color> _func;

    public ShaderColorPropertyAnimation(int propertyID, Func<Color, Color> func)
    {
        _propertyID = propertyID;
        _func = func ?? throw new ArgumentNullException("func is null");
    }

    protected override bool OnUpdate(Material actor)
    {
        if (!actor) return false;

        var color = actor.GetColor(_propertyID);
        actor.SetColor(_propertyID, _func(color));
        return true;
    }

    protected override void OnStart(Material actor) { }
    protected override void OnStop(Material actor) { }
}