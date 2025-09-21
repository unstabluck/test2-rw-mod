using BepInEx;
using BepInEx.Logging;
using MonoMod;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using UnityEngine;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TestMod;

[BepInPlugin("manngo.spider_revive_explosion", "Spiders Explode on Revive", "0.1.4")]
sealed class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;
    bool IsInit;
    public List<FreeHomingMath> boomFlies = new List<FreeHomingMath>();
    public Stopwatch stopwatch;

    public void OnEnable()
    {
        Logger = base.Logger;
        stopwatch = new Stopwatch();
        On.RainWorld.OnModsInit += OnModsInit;

        //New Hooks

        //Spider revive triggers new feature
        On.BigSpider.Revive += MyReviveFunc;

        //Frame by Frame Update
        On.Room.Update += MyUpdateFunc;
    }

    private void OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        if (IsInit) return;
        IsInit = true;

        // Initialize assets, your mod config, and anything that uses RainWorld here
        Logger.LogDebug("Hello world!");
    }
    public void MyReviveFunc(On.BigSpider.orig_Revive orig, BigSpider self)
    {

        orig(self);

        //Short-hand variables
        Player player = self.room.PlayersInRoom[0];
        var pos = self.mainBodyChunk.pos;
        
        //Explode
        Logger.LogInfo("Boom");
        self.room.AddObject(new Explosion(self.room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
        self.room.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, new UnityEngine.Color(1f, 1f, 1f)));
        self.room.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new UnityEngine.Color(1f, 1f, 1f)));
        self.room.AddObject(new ExplosionSpikes(self.room, pos, 14, 30f, 9f, 7f, 170f, new UnityEngine.Color(1f, 1f, 1f)));
        self.room.AddObject(new ShockWave(pos, 330f, 0.045f, 5, false));
        self.room.PlaySound(SoundID.Bomb_Explode, self.mainBodyChunk.pos);
        //Add a new homing explosion
        this.boomFlies.Add(new FreeHomingMath(self.room.PlayersInRoom[0].mainBodyChunk, pos, self.room) );
        
    }

    public void MyUpdateFunc(On.Room.orig_Update orig, Room self)
    {
        orig(self);
        if (boomFlies.Count > 0)
        {
            stopwatch.Start();
        }  
        if(stopwatch.ElapsedMilliseconds >= 25)
        {
            //update each existing homing explosion every half second.
            for (int i = 0; i < this.boomFlies.Count; i++)
            {
                Logger.LogDebug("Milliseconds passed: ")
                Logger.LogDebug(stopwatch.ElapsedMilliseconds);
                
                Vector2 boomPos = boomFlies[i].pos;
                Room room = boomFlies[i].room;
                Player owner = boomFlies[i].room.PlayersInRoom[0];

                //Log Info for Debugging

                Logger.LogDebug("Position: ");
                Logger.LogDebug(boomPos);
                Logger.LogDebug("Target position: ");
                Logger.LogDebug(boomFlies[i].targetPos);
                Logger.LogDebug("Position Error: ");
                Logger.LogDebug(boomFlies[i].errorp);
                /*
                Logger.LogDebug("Velocity: ");
                Logger.LogDebug(boomFlies[i].vel);
                Logger.LogDebug("Target Velocity: ");
                Logger.LogDebug(boomFlies[i].targetVel);
                Logger.LogDebug("Derivative Error: ");
                Logger.LogDebug(boomFlies[i].errord);
                */
                Logger.LogDebug("Accel: ");
                Logger.LogDebug(boomFlies[i].accel);
                /*
                Logger.LogDebug("Integrated Error: ");
                Logger.LogDebug(boomFlies[i].errori);
                Logger.LogDebug("Ki contribution: ");
                Logger.LogDebug(boomFlies[i].errori * boomFlies[i].ki);
                */
                



                //Explosions
                //room.AddObject(new Explosion(room, owner, boomPos, 7, 250f, 6.2f, 2f, 280f, 0.25f, owner, 0.7f, 160f, 1f));
                room.AddObject(new Explosion.ExplosionLight(boomPos, 280f, 1f, 7, new UnityEngine.Color(1f, 1f, 1f)));
                room.AddObject(new Explosion.ExplosionLight(boomPos, 230f, 1f, 3, new UnityEngine.Color(1f, 1f, 1f)));
                room.AddObject(new ExplosionSpikes(room, boomPos, 14, 30f, 9f, 7f, 170f, new UnityEngine.Color(1f, 1f, 1f)));
                room.AddObject(new ShockWave(boomPos, 330f, 0.045f, 5, false));
                //room.PlaySound(SoundID.Bomb_Explode, boomPos);

                //Update
                boomFlies[i].Update(stopwatch.ElapsedMilliseconds);
                //Update position of target
                boomFlies[i].targetPos = owner.mainBodyChunk.pos;



            }
            stopwatch.Stop();
            stopwatch.Restart();
        }
        
        
    }
    
}