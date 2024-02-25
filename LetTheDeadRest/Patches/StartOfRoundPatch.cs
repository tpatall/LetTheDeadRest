using GameNetcodeStuff;
using HarmonyLib;

namespace LetTheDeadRest.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("UpdatePlayerVoiceEffects")]
        static void ChangeVolumeWhileDead(PlayerControllerB[] ___allPlayerScripts)
        {
            if (GameNetworkManager.Instance == null || GameNetworkManager.Instance.localPlayerController == null)
            {
                return;
            }

            for (int i = 0; i < ___allPlayerScripts.Length; i++)
            {
                PlayerControllerB playerControllerB2 = ___allPlayerScripts[i];
                if ((!playerControllerB2.isPlayerControlled && !playerControllerB2.isPlayerDead) || playerControllerB2 == GameNetworkManager.Instance.localPlayerController)
                {
                    continue;
                }

                if (GameNetworkManager.Instance.localPlayerController.isPlayerDead)
                {
                    if (playerControllerB2.isPlayerDead)
                    {
                        playerControllerB2.voicePlayerState.Volume = 1.0f;
                    } 
                    else
                    {
                        playerControllerB2.voicePlayerState.Volume = Plugin.instance.volumeMultiplier.Value;
                    }
                }
            }
        }
    }
}
