using BepInEx;
using BepInEx.Logging;
using MonoMod;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TestMod;

[BepInPlugin("manngo.spider_revive_explosion", "Spiders Explode on Revive", "0.1.2")]
sealed class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;
    bool IsInit;
    public List<FreeHomingMath> boomFlies = new List<FreeHomingMath>();

    public void OnEnable()
    {
        Logger = base.Logger;
        On.RainWorld.OnModsInit += OnModsInit;
        On.BigSpider.Revive += MyReviveFunc;
        On.RainWorldGame.Update += MyUpdateFunc;
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
        Player player = self.room.PlayersInRoom[0];
        var pos = self.mainBodyChunk.pos;
        
        Logger.LogInfo("Boom");
        self.room.AddObject(new Explosion(self.room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
        self.room.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, new UnityEngine.Color(1f, 1f, 1f)));
        self.room.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new UnityEngine.Color(1f, 1f, 1f)));
        self.room.AddObject(new ExplosionSpikes(self.room, pos, 14, 30f, 9f, 7f, 170f, new UnityEngine.Color(1f, 1f, 1f)));
        self.room.AddObject(new ShockWave(pos, 330f, 0.045f, 5, false));
        self.room.PlaySound(SoundID.Bomb_Explode, self.mainBodyChunk.pos);
        this.boomFlies.Add(new FreeHomingMath(self.room.PlayersInRoom[0].mainBodyChunk, pos, self.room) );
        
    }

    public void MyUpdateFunc(On.RainWorldGame.orig_Update orig, RainWorldGame self)
    {
        orig(self);
        for (int i = 0; i < this.boomFlies.Count; i++)
        {
            Logger.LogInfo("inside update-for boomflies loop");
            boomFlies[i].Update();
            Vector2 boomPos = boomFlies[i].pos;
            Room room = boomFlies[i].room;
            Player owner = boomFlies[i].room.PlayersInRoom[0];
            
            Logger.LogInfo(boomPos);
            Logger.LogInfo("Integral Error: ");
            Logger.LogInfo(boomFlies[i].errori);
            room.AddObject(new Explosion(room, owner, boomPos, 7, 250f, 6.2f, 2f, 280f, 0.25f, owner, 0.7f, 160f, 1f));
            room.AddObject(new Explosion.ExplosionLight(boomPos, 280f, 1f, 7, new UnityEngine.Color(1f, 1f, 1f)));
            room.AddObject(new Explosion.ExplosionLight(boomPos, 230f, 1f, 3, new UnityEngine.Color(1f, 1f, 1f)));
            room.AddObject(new ExplosionSpikes(room, boomPos, 14, 30f, 9f, 7f, 170f, new UnityEngine.Color(1f, 1f, 1f)));
            room.AddObject(new ShockWave(boomPos, 330f, 0.045f, 5, false));
            room.PlaySound(SoundID.Bomb_Explode, boomPos);


        }
    }
    
}