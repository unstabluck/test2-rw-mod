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

[BepInPlugin("manngo.spider_revive_explosion", "Spiders Explode on Revive", "0.1.4")]
internal class FreeHomingMath
{
    public BodyChunk target;
    public Room room;
    public float timer = 0f;

    //Plant Vectors for each Homing Instance 
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
    public Vector2 lastErrorp;
    
    //PID Controller Constants
    public float kp = 1f; //0.5f 
    public float ki = 0.8f; //0.5f
    public float kd = 0.05f; //0.05f
    //public float flyAway = 10f;
    bool addDiff;

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
        lastErrorp = Vector2.zeroVector;

        targetVel = Vector2.zeroVector;
        targetAccel = Vector2.zeroVector;

        errori = Vector2.zeroVector;

        addDiff = true;


    }

    public void Update(float delta)
    {
        //dt in seconds 
        float dt = delta / 1000f;
        //Find Error Vectors
        
        errorp = new Vector2( targetPos.x - pos.x, targetPos.y - pos.y);
        errori += errorp*dt;
        errord = new Vector2((errorp.x - lastErrorp.x)/dt, (errorp.y - lastErrorp.y)/dt);


        //Find Derivatives of Position
        vel = new Vector2((pos.x -lastPos.x)/dt, (pos.y - lastPos.y)/dt);
        targetVel = new Vector2((targetPos.x - targetLastPos.x)/dt, (targetPos.y - targetLastPos.y)/dt);
        accel = new Vector2((vel.x - lastVel.x)/dt, (vel.y - lastVel.y)/dt);
        targetAccel = new Vector2((targetVel.x - targetLastVel.x)/dt, (targetVel.y - targetLastVel.y)/dt);
        

        //Last = Now 
        lastPos = pos;
        lastVel = vel;
        lastAccel = accel;
        targetLastPos = targetPos;
        targetLastVel = targetVel;
        targetLastAccel = targetAccel;
        lastErrorp = errorp;

        //Kinematics Update
        //magnitude reduction
        if (errord.magnitude > 0)
        {
            vel = Vector2.zero;
            addDiff = false;
        }

        accel = (errorp * kp) + (errori * ki) + (errord * kd);
        if (addDiff)
        {
            vel = vel + accel * dt + errorp * kp + errori * ki +  errord * kd;
        }
        else
        {
            vel = vel + accel * dt + errorp * kp + errori * ki;
        }
        pos = pos + vel*dt;

        //magnitude reduction
        if (errord.magnitude > 0)
        {
            vel = Vector2.zero;
        }
    }

    
}

