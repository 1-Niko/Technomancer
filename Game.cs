using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Slugpack
{
    static class GameHooks
    {
        internal static void Apply()
        {
            On.RainWorldGame.ctor += RainWorldGame_ctor;
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
            On.RoomRealizer.CanAbstractizeRoom += RoomRealizer_CanAbstractizeRoom;
            On.RainWorldGame.Update += RainWorldGame_Update;
            On.Region.GetProperRegionAcronym += Region_GetProperRegionAcronym;
        }

        private static void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame self, ProcessManager manager)
        {
            orig(self, manager);

            if (!Constants.DamagedShortcuts.TryGetValue(self, out var _))
            { Constants.DamagedShortcuts.Add(self, _ = new WeakTables.ShortcutList()); }
        }

        private static void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            Futile.atlasManager.LoadAtlas("atlases/slugpackatlas");
        }

        private static bool RoomRealizer_CanAbstractizeRoom(On.RoomRealizer.orig_CanAbstractizeRoom orig, RoomRealizer self, RoomRealizer.RealizedRoomTracker tracker)
        {
            if (Constants.DamagedShortcuts.TryGetValue(self.world.game, out var ShortcutTable))
            {
                using List<AbstractCreature>.Enumerator enumerator = tracker.room.world.game.NonPermaDeadPlayers.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    for (int i = 0; i < ShortcutTable.locks.Count; i++)
                    {
                        for (int r = 0; r < 2; r++)
                        {
                            if (ShortcutTable.locks[i].rooms[r].abstractRoom == enumerator.Current.Room)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return orig(self, tracker);
        }

        private static void RainWorldGame_Update(On.RainWorldGame.orig_Update orig, RainWorldGame self)
        {
            orig(self);
            if (Constants.DamagedShortcuts.TryGetValue(self, out var ShortcutTable))
            {
                for (int i = 0; i < ShortcutTable.locks.Count; i++)
                {
                    if (ShortcutTable.locks[i].time > 0)
                    {
                        ShortcutTable.locks[i].time--;
                        for (int r = 0; r < 2; r++)
                        {
                            if (Random.Range(0, 80) == 0)
                            {
                                if (ShortcutTable.locks[i].rooms[r].abstractRoom.realizedRoom != null)
                                {
                                    for (int j = 0; j < Random.Range(10, 30); j++)
                                    {
                                        Vector2 a = RWCustom.Custom.RNV();
                                        ShortcutTable.locks[i].rooms[r].AddObject(new Spark(ShortcutTable.locks[i].rooms[r].MiddleOfTile(ShortcutTable.locks[i].shortcuts[r].StartTile) + a * Random.value * 40f, a * Mathf.Lerp(4f, 30f, Random.value), new Color(0.9f, 0.9f, 1f), null, 16, 30));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ShortcutTable.locks.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private static string Region_GetProperRegionAcronym(On.Region.orig_GetProperRegionAcronym orig, SlugcatStats.Name character, string baseAcronym)
        {
            string text = baseAcronym;
            if (character.ToString() == Constants._catID && text == "SL")
            {
                text = "LM";
                foreach (var path in AssetManager.ListDirectory("World", true, false)
                    .Select(p => AssetManager.ResolveFilePath($"World{Path.DirectorySeparatorChar}{Path.GetFileName(p)}{Path.DirectorySeparatorChar}equivalences.txt"))
                    .Where(File.Exists)
                    .SelectMany(p => File.ReadAllText(p).Trim().Split(',')))
                {
                    var parts = path.Contains("-") ? path.Split('-') : new[] { path };
                    if (parts[0] == baseAcronym && (parts.Length == 1 || character.value.Equals(parts[1], System.StringComparison.OrdinalIgnoreCase)))
                    {
                        text = Path.GetFileName(path).ToUpper();
                        break;
                    }
                }
                return text;
            }
            return orig(character, baseAcronym);
        }
    }
}
