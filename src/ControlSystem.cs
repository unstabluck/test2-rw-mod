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
    public BodyChunk target;
    public Room room;
    public Vector2 lastPos;
    public Vector2 pos;
    public Vector2 lastVel;
    public Vector2 vel;
    public Vector2 lastAccel;
    public Vector2 accel;
    public Vector2 targetLastPos;
    public Vector2 targetPos;
    public Vector2 targetLastVel;
    public Vector2 targetVel;
    public Vector2 targetLastAccel;
    public Vector2 targetAccel;
    public Vector2 errorp;
    public Vector2 errori;
    public Vector2 errord;
    public float kp = 0.05f;
    public float ki = 0.05f;
    public float kd = 0.05f;

    public FreeHomingMath(BodyChunk target, Vector2 pos, Room room) { 
        this.target = target;
        this.pos = pos;
        this.room = room;
        this.targetPos = target.pos;

        this.lastPos = pos;
        this.lastVel = Vector2.zeroVector;
        this.lastAccel = Vector2.zeroVector;
        targetLastVel = Vector2.zeroVector;
        targetLastAccel = Vector2.zeroVector;

        targetVel = Vector2.zeroVector;
        targetAccel = Vector2.zeroVector;



    }

    public void Update()
    {
        errorp = new Vector2(vel.x - targetVel.x, vel.y - targetVel.y);
        errori = new Vector2(pos.x - targetPos.x, pos.y - targetPos.y);
        errord = new Vector2(accel.x - targetAccel.x, accel.y - targetAccel.y);

        vel = new Vector2(pos.x -lastPos.x, pos.y - lastPos.y);
        targetVel = new Vector2(targetPos.x - targetLastPos.x, targetPos.y - targetLastPos.y);
        accel = new Vector2(vel.x - lastVel.x, vel.y - lastVel.y);
        targetAccel = new Vector2(targetVel.x - targetLastVel.x, targetVel.y - targetLastVel.y);
        


        lastPos = pos;
        lastVel = vel;
        lastAccel = accel;
        targetLastPos = targetPos;
        targetLastVel = targetVel;
        targetLastAccel = targetAccel;
        //TODO add math and explosions 

        pos = pos - (errori * ki);
        vel = vel - (errorp * kp);
        accel = accel - (errord * kd);
    }

    
}

