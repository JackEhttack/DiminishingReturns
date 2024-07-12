using System.Collections;
using On.DunGen;
using On.GameNetcodeStuff;

namespace JackEhttack.patch;

public static class PlayerControllerBPatch
{
    public static void ApplyPatches()
    {
        On.GameNetcodeStuff.PlayerControllerB.PlayerJump += PlayerJumpPatch;
    }

    private static IEnumerator PlayerJumpPatch(PlayerControllerB.orig_PlayerJump orig, GameNetcodeStuff.PlayerControllerB self)
    {
        self.isJumping = false;
        IEnumerator enumerator = orig(self);
        yield return enumerator;
    }
}