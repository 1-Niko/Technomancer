using BepInEx;
using System.Security;
using System.Security.Permissions;

#pragma warning disable CS0618 // ignore false message
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace SlugpackPlugin
{
    [BepInEx.BepInPlugin(_ID, nameof(SlugpackPlugin), "1.0.0")]

    public class Plugin : BaseUnityPlugin
    {
        const string _ID = "niko.slugpack";

        public void OnEnable()
        {
            Slugpack.PlayerHooks.Apply();
            Slugpack.PlayerGraphicsHooks.Apply();
            Slugpack.HypothermiaHooks.Apply();
            Slugpack.GameHooks.Apply();
            Slugpack.CreatureHooks.Apply();
            Slugpack.MoonDialogue.Apply();
        }
    }
}