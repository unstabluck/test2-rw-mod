using BepInEx;
using BepInEx.Logging;
using MonoMod;
using System.Security.Permissions;
using UnityEngine;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TestMod;

[BepInPlugin("manngo.spider_revive_explosion", "Spiders Explode on Revive", "0.1.0")]
internal class FreeHomingMath
{
    public AbstractPhysicalObject target;
    public Room room = target.Room;
    public Vector2 lastPos;
    public Vector2 pos;
    public Vector2 lastVel;
    public Vector2 vel;
    public Vector2 lastAccel;
    public Vector2 Accel;
    public Vector2 targetlastPos;
    public Vector2 targetPos;
    public Vector2 targetlastVel;
    public Vector2 targetVel;
    public Vector2 targetlastAccel;
    public Vector2 targetAccel;

    public FreeHomingMath(AbstractPhysicalObject target, Vector2 pos) { 
        this.target = target;
        this.pos = pos;
        this.room = target.Room;
        this.targetPos = target.pos;

    }

    public void Update()
    {

        lastPos = pos;
        lastVel = vel;
        lastAccel = Accel;
        targetLastPos = targetPos;
        targetlastVel = targetVel
        targetLastAccel = targetAccel;
        //TODO add math and explosions 
    }

    
}

