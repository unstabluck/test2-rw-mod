using BepInEx;
using BepInEx.Logging;
using MonoMod;
using System.Security.Permissions;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TestMod;

[BepInPlugin("com.manngo.testmod2", "Test Mod 2", "0.1.0")]
sealed class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;
    bool IsInit;

    public void OnEnable()
    {
        Logger = base.Logger;
        On.RainWorld.OnModsInit += OnModsInit;
        On.Player.Jump += myFunc;
    }

    private void OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        if (IsInit) return;
        IsInit = true;

        // Initialize assets, your mod config, and anything that uses RainWorld here
        Logger.LogDebug("Hello world!");
    }
    public static void MyFunc(On.Player.orig_Jump, Player self)
    {
        orig(self);
        Logger.LogInfo("Hello World");
        new Explosion(self.room, self, self.bodyChunks[0].pos, 40, 10.0, 10.0, 10.0, 10.0, 0.0, self, 0.1, 0.0, 0.0);
    }
}