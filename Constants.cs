using System.Runtime.CompilerServices;
using UnityEngine;

namespace Slugpack
{
    static class Constants
    {
        public const string _slugID = "niko_dragonslayer";

        public const string _catID = "niko_technomancer";

        public const int timeReached = 10;

        public static ConditionalWeakTable<Player, WeakTables.ScanLine> ScanLineMemory = new();

        public static ConditionalWeakTable<PlayerGraphics, WeakTables.GraphicsData> Graphics = new();

        public static ConditionalWeakTable<Vulture, WeakTables.VultureStuff> VultureStuff = new();

        public static ConditionalWeakTable<RainWorldGame, WeakTables.ShortcutList> DamagedShortcuts = new();

        public static ConditionalWeakTable<Oracle, WeakTables.OracleData> OracleInfo = new();

        // Hologram Data

        public static readonly DataStructures.Sprite[] dsData = {
            new DataStructures.Sprite(1, 0, new Vector2(0f, 0f), 0f,  new DataStructures.Anchorpoint(0.8125f, 0.625f), new DataStructures.Scale(1f, -1f))
        };

        public static readonly DataStructures.Sprite[] spriteData = {
                new DataStructures.Sprite(1, 0, new Vector2(-4.375f, -4.5f), 32.5f, new DataStructures.Anchorpoint(0.8125f, 0.625f), new DataStructures.Scale(1f, -1f)),
                new DataStructures.Sprite(1, 1, new Vector2(-2.875f, -7.75f), -92.5f, new DataStructures.Anchorpoint(0.25f, 0.16f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(1, 2, new Vector2(-6.25f, 0.625f), -57.5f, new DataStructures.Anchorpoint(0.25f, 0.2f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(1, 3, new Vector2(-4.75f, 4f), -47.5f, new DataStructures.Anchorpoint(0.4f, 0.25f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(1, 4, new Vector2(-5.25f, -0.25f), -62.5f, new DataStructures.Anchorpoint(0.33f, 0.28f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(1, 5, new Vector2(-3.625f, 5.25f), -25f, new DataStructures.Anchorpoint(0.75f, 0.28f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(1, 6, new Vector2(1.125f, -8.125f), -62.5f, new DataStructures.Anchorpoint(0.2f, 0.875f), new DataStructures.Scale(-1f, -1f)),
                new DataStructures.Sprite(1, 7, new Vector2(0.75f, -9f), -147.5f, new DataStructures.Anchorpoint(0f, 0.8f), new DataStructures.Scale(1f, -1f)),
                new DataStructures.Sprite(1, 8, new Vector2(1.875f, -9.25f), -75f, new DataStructures.Anchorpoint(0.75f, 0.833f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(1, 9, new Vector2(4.125f, -7.875f), -100f, new DataStructures.Anchorpoint(0f, 1f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(1, 10, new Vector2(4.875f, -6.125f), -125f, new DataStructures.Anchorpoint(0.833f, 0.916f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(1, 11, new Vector2(4.375f, -2f), -160f, new DataStructures.Anchorpoint(0.833f, 0.916f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(1, 12, new Vector2(5.75f, 0.875f), 47.5f, new DataStructures.Anchorpoint(0.75f, 0.16f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(1, 13, new Vector2(5.25f, 4.375f), -332.5f, new DataStructures.Anchorpoint(0.33f, 0.357f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(1, 14, new Vector2(3f, 5.375f), 57.5f, new DataStructures.Anchorpoint(0.8f, 0.875f), new DataStructures.Scale(1f, -1f)),
                new DataStructures.Sprite(1, 15, new Vector2(2.875f, 5.125f), -50f, new DataStructures.Anchorpoint(0.8f, 0.16f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(0, 16, new Vector2(0f, 1.5f), 107.5f, new DataStructures.Anchorpoint(0.2f, 0.1f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(0, 17, new Vector2(1.75f, 0.375f), -112.5f, new DataStructures.Anchorpoint(0.66f, 0.85f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(0, 18, new Vector2(3f, 0.25f), 72.5f, new DataStructures.Anchorpoint(0.83f, 0.083f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(0, 19, new Vector2(3.5f, -0.625f), 135f, new DataStructures.Anchorpoint(0.75f, 0.14f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 20, new Vector2(4.75f, -2.125f), -5f, new DataStructures.Anchorpoint(0.25f, 0.2f), new DataStructures.Scale(1f, -1f)),
                new DataStructures.Sprite(0, 21, new Vector2(3f, -3.25f), 90f, new DataStructures.Anchorpoint(0.2f, 0.16f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 22, new Vector2(4.5f, -5.75f), 50f, new DataStructures.Anchorpoint(0.625f, 0.1875f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 23, new Vector2(4f, -5.75f), 60f, new DataStructures.Anchorpoint(0.2f, 0.83f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 24, new Vector2(-0.75f, 1.375f), 72.5f, new DataStructures.Anchorpoint(0.2f, 0.9f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(0, 25, new Vector2(-2.25f, 0.875f), -52.5f, new DataStructures.Anchorpoint(0.8f, 0.16f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 26, new Vector2(-2.5f, 0.125f), -65f, new DataStructures.Anchorpoint(1f, 0f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 27, new Vector2(-6.25f, -1.125f), 80f, new DataStructures.Anchorpoint(0.75f, 0.8f), new DataStructures.Scale(-1f, 1f)),
                new DataStructures.Sprite(0, 28, new Vector2(-3f, -2.875f), 207.5f, new DataStructures.Anchorpoint(0.2f, 0.14f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 29, new Vector2(-4.5f, -4.25f), 0f, new DataStructures.Anchorpoint(0.75f, 0.83f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 30, new Vector2(-3.25f, -4.625f), -37.5f, new DataStructures.Anchorpoint(0.83f, 0.85f), new DataStructures.Scale(1f, 1f)),
                new DataStructures.Sprite(0, 31, new Vector2(-1.25f, -6f), -137.5f, new DataStructures.Anchorpoint(0.2f, 0.33f), new DataStructures.Scale(-1f, 1f)),
                null

                // These ones need work done to fix detatchment issues

                //new DataStructures.Sprite(2, 32, new Vector2(0f, 0f), 0f, new DataStructures.Anchorpoint(0.8f, 0.28f), new DataStructures.Scale(-1f, 1f)),
                //new DataStructures.Sprite(2, 33, new Vector2(8f, 2.25f), 55f, new DataStructures.Anchorpoint(0.25f, 0.25f), new DataStructures.Scale(1f, 1f)),
                //new DataStructures.Sprite(2, 34, new Vector2(5.75f, -0.125f), 315f, new DataStructures.Anchorpoint(0.25f, 0.8f), new DataStructures.Scale(1f, 1f)),
                //new DataStructures.Sprite(2, 35, new Vector2(-0.75f, 3.25f), 47.5f, new DataStructures.Anchorpoint(0.8f, 0.75f), new DataStructures.Scale(1f, 1f)),

                // The sprites for these ones don't exist yet

                //new DataStructures.Sprite(2, 36, new Vector2(0f, 0f), 0f, new DataStructures.Anchorpoint(0.25f, 0.8f), new DataStructures.Scale(1f, 1f)),
                //new DataStructures.Sprite(2, 37, new Vector2(0f, 0f), 0f, new DataStructures.Anchorpoint(0.25f, 0.8f), new DataStructures.Scale(1f, 1f)),
            };
    }
}