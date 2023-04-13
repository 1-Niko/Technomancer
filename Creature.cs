using System.Linq;
using UnityEngine;

namespace Slugpack
{
    static class CreatureHooks
    {
        internal static void Apply()
        {
            On.Creature.SuckedIntoShortCut += Creature_SuckedIntoShortCut;
            On.Vulture.Update += Vulture_Update;
        }

        private static void Creature_SuckedIntoShortCut(On.Creature.orig_SuckedIntoShortCut orig, Creature self, RWCustom.IntVector2 entrancePos, bool carriedByOther)
        {
            var shortcutLocation = Utilities.DetermineObjectFromPosition(self.room.MiddleOfTile(entrancePos), self.room).nearestShortcut;
            if (Constants.DamagedShortcuts.TryGetValue(self.room.game, out var ShortcutTable))
            {
                for (int i = 0; i < ShortcutTable.locks.Count; i++)
                {
                    for (int r = 0; r < 2; r++)
                    {
                        if (ShortcutTable.locks[i].shortcuts[r].Equals(shortcutLocation))
                        {
                            self.enteringShortCut = null;
                            self.inShortcut = false;
                            return;
                        }
                    }
                }
            }
            orig(self, entrancePos, carriedByOther);
        }

        private static void Vulture_Update(On.Vulture.orig_Update orig, Vulture self, bool eu)
        {
            orig(self, eu);

            if (Constants.VultureStuff.TryGetValue(self, out var vulturestuff))
            {
                if (vulturestuff.timer > 0)
                {
                    if (vulturestuff.thruster == -1)
                    {
                        vulturestuff.thruster = Random.Range(0, 5);
                    }

                    self.stun = 45;
                    self.landingBrake = vulturestuff.timer;


                    if (self.IsKing)
                    {
                        if (Random.Range(0, 20) == 0)
                        {
                            //self.kingTusks.TryToShoot();

                            int tusk = Random.Range(0, 2);

                            if (self.kingTusks.tusks.ElementAtOrDefault(tusk) != null && self.kingTusks.tusks[tusk] != null)
                            {
                                if (self.kingTusks.tusks[tusk].mode == KingTusks.Tusk.Mode.Attached)
                                {
                                    Vector2 vector = RWCustom.Custom.DirVec(self.kingTusks.tusks[tusk].vulture.neck.tChunks[self.kingTusks.tusks[tusk].vulture.neck.tChunks.Length - 1].pos, self.kingTusks.tusks[tusk].vulture.bodyChunks[4].pos);
                                    Vector2 a = RWCustom.Custom.PerpendicularVector(vector);
                                    Vector2 vector2 = self.kingTusks.tusks[tusk].vulture.bodyChunks[4].pos + vector * -5f;
                                    vector2 += a * self.kingTusks.tusks[tusk].zRot.x * 15f;
                                    vector2 += a * self.kingTusks.tusks[tusk].zRot.y * ((self.kingTusks.tusks[tusk].side == 0) ? -1f : 1f) * 7f;

                                    self.kingTusks.tusks[tusk].Shoot(vector2);
                                }
                            }
                        }
                    }

                    /*foreach (var chunk in self.bodyChunks)
                     {
                         chunk.vel += RWCustom.Custom.RNV() * 1.5f;
                     }*/

                    if (self.thrusters.ElementAtOrDefault(vulturestuff.thruster) != null)
                    {
                        self.thrusters[vulturestuff.thruster].Activate(20);
                        for (int i = 0; i < 4; i++)
                        {
                            if (i != vulturestuff.thruster)
                            {
                                self.thrusters[i].thrust = 0;
                                self.thrusters[i].smoke = null;
                            }
                        }

                        self.mainBodyChunk.vel *= 3f;
                    }

                    vulturestuff.timer--;
                }
                else
                {
                    vulturestuff.thruster = -1;
                }
            }
        }
    }
}