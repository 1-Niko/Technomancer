using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Slugpack
{
    static class PlayerGraphicsHooks
    {
        internal static void Apply()
        {
            On.PlayerGraphics.InitiateSprites += PlayerGraphics_InitiateSprites;
            On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
            On.PlayerGraphics.AddToContainer += (orig, self, sLeaser, rCam, newContainer) =>
            {
                orig(self, sLeaser, rCam, newContainer);
                if (self.player.slugcatStats.name.ToString() == Constants._catID)
                {
                    if (sLeaser.sprites.Length > 13)
                    {
                        var midground = rCam.ReturnFContainer("Midground");
                        var foreground = rCam.ReturnFContainer("Foreground");

                        /*if (Constants.spriteData[Constants.spriteData.Length - 1] != null)
                        {
                            midground.RemoveChild(sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock]);
                            foreground.AddChild(sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock]);
                        }*/

                        for (int i = 0; i < Constants.spriteData.Length - ((Constants.spriteData[Constants.spriteData.Length - 1] != null) ? 0 : 1); i++)
                        {
                            foreground.RemoveChild(sLeaser.sprites[i + 13]);
                            midground.AddChild(sLeaser.sprites[i + 13]);
                            sLeaser.sprites[i + 13].MoveBehindOtherNode(sLeaser.sprites[0]);
                        }
                    }
                }
            };
        }

        private static void PlayerGraphics_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            orig(self, sLeaser, rCam);
            if (self.player.slugcatStats.name.ToString() == Constants._slugID)
            {
                self.tail = new TailSegment[5];
                self.tail[0] = new TailSegment(self, 8f, 4f, null, 0.85f, 1f, 1f, true);
                self.tail[1] = new TailSegment(self, 6f, 7f, self.tail[0], 0.85f, 1f, 0.5f, true);
                self.tail[2] = new TailSegment(self, 4.5f, 7f, self.tail[1], 0.85f, 1f, 0.5f, true);
                self.tail[3] = new TailSegment(self, 2f, 7f, self.tail[2], 0.85f, 1f, 0.5f, true);
                self.tail[4] = new TailSegment(self, 1f, 7f, self.tail[3], 0.85f, 1f, 0.5f, true);

                List<BodyPart> list = Enumerable.ToList<BodyPart>(self.bodyParts);
                list.RemoveAll((BodyPart x) => x is TailSegment);
                list.AddRange(self.tail);
                self.bodyParts = list.ToArray();
            }
            else if (self.player.slugcatStats.name.ToString() == Constants._catID)
            {
                self.tail = new TailSegment[4];
                self.tail[0] = new TailSegment(self, 5.5f, 4f, null, 0.85f, 1f, 1f, true);
                self.tail[1] = new TailSegment(self, 3.7f, 7f, self.tail[0], 0.85f, 1f, 0.5f, true);
                self.tail[2] = new TailSegment(self, 2.3f, 7f, self.tail[1], 0.85f, 1f, 0.5f, true);
                self.tail[3] = new TailSegment(self, 1f, 7f, self.tail[2], 0.85f, 1f, 0.5f, true);

                List<BodyPart> list = Enumerable.ToList<BodyPart>(self.bodyParts);
                list.RemoveAll((BodyPart x) => x is TailSegment);
                list.AddRange(self.tail);
                self.bodyParts = list.ToArray();

                Array.Resize(ref sLeaser.sprites, sLeaser.sprites.Length + Constants.spriteData.Length - ((Constants.spriteData[Constants.spriteData.Length - 1] != null) ? 0 : 1));

                for (int i = 0; i < Constants.spriteData.Length - ((Constants.spriteData[Constants.spriteData.Length - 1] != null) ? 0 : 1); i++)
                {
                    if (Constants.spriteData[i] != null)
                    {
                        sLeaser.sprites[i + 13] = new FSprite(Constants.spriteData[i].SpriteIndex, true)
                        {
                            isVisible = true
                        };

                        if (Constants.spriteData[Constants.spriteData.Length - 1] != null && (Constants.spriteData[i].Lock == 0 || Constants.spriteData[i].Lock == 1))
                        {
                            sLeaser.sprites[i + 13].scaleX = Constants.spriteData[i].ScaleX * ((Constants.spriteData[Constants.spriteData.Length - 1] != null && Constants.spriteData[i].Lock == Constants.spriteData[Constants.spriteData.Length - 1].Lock) ? 2 : 1);
                            sLeaser.sprites[i + 13].scaleY = Constants.spriteData[i].ScaleY * ((Constants.spriteData[Constants.spriteData.Length - 1] != null && Constants.spriteData[i].Lock == Constants.spriteData[Constants.spriteData.Length - 1].Lock) ? 2 : 1);
                        }
                        sLeaser.sprites[i + 13].anchorX = Constants.spriteData[i].AnchorX;
                        sLeaser.sprites[i + 13].anchorY = Constants.spriteData[i].AnchorY;
                    }
                }

                self.AddToContainer(sLeaser, rCam, null);
            }
        }

        private static void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            orig(self, sLeaser, rCam, timeStacker, camPos);
            if (self.player.slugcatStats.name.ToString() == Constants._slugID)
            {
                float num = 0.5f + 0.5f * Mathf.Sin(Mathf.Lerp(self.lastBreath, self.breath, timeStacker) * 3.1415927f * 2f);
                float num2 = Mathf.InverseLerp(0.3f, 0.5f, Mathf.Abs(RWCustom.Custom.DirVec(Vector2.Lerp(self.drawPositions[1, 1], self.drawPositions[1, 0], timeStacker), Vector2.Lerp(self.drawPositions[0, 1], self.drawPositions[0, 0], timeStacker)).y));
                sLeaser.sprites[0].scaleX = 1.17f + Mathf.Lerp(Mathf.Lerp(Mathf.Lerp(-0.05f, -0.15f, self.malnourished), 0.05f, num) * num2, 0.15f, self.player.sleepCurlUp);
                sLeaser.sprites[1].scaleX = 1.2f + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;

                var fsprite = sLeaser.sprites[3];
                if (fsprite?.element?.name is string text && text.StartsWith("Head"))
                {
                    foreach (var atlas in Futile.atlasManager._atlases)
                    {
                        if (atlas._elementsByName.TryGetValue("Fluff" + text, out var element))
                        {
                            fsprite.element = element;
                            break;
                        }
                    }
                }
            }
            else if (self.player.slugcatStats.name.ToString() == Constants._catID)
            {
                float num = 0.5f + 0.5f * Mathf.Sin(Mathf.Lerp(self.lastBreath, self.breath, timeStacker) * 3.1415927f * 2f);
                float num2 = Mathf.InverseLerp(0.3f, 0.5f, Mathf.Abs(RWCustom.Custom.DirVec(Vector2.Lerp(self.drawPositions[1, 1], self.drawPositions[1, 0], timeStacker), Vector2.Lerp(self.drawPositions[0, 1], self.drawPositions[0, 0], timeStacker)).y));
                sLeaser.sprites[0].scaleX = 0.96f + Mathf.Lerp(Mathf.Lerp(Mathf.Lerp(-0.05f, -0.15f, self.malnourished), 0.05f, num) * num2, 0.15f, self.player.sleepCurlUp);
                sLeaser.sprites[1].scaleX = 0.93f + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;

                var fsprite = sLeaser.sprites[3];
                if (fsprite?.element?.name is string text && text.StartsWith("Head"))
                {
                    foreach (var atlas in Futile.atlasManager._atlases)
                    {
                        if (atlas._elementsByName.TryGetValue("Fluff" + text, out var element))
                        {
                            fsprite.element = element;
                            break;
                        }
                    }
                }

                for (int i = 0; i < Constants.spriteData.Length - ((Constants.spriteData[Constants.spriteData.Length - 1] != null) ? 0 : 1); i++)
                {
                    if (sLeaser.sprites[i + 13] != null)
                    {
                        if (Constants.spriteData[Constants.spriteData.Length - 1] != null && Constants.spriteData[i].Lock == Constants.spriteData[Constants.spriteData.Length - 1].Lock)
                        {
                            sLeaser.sprites[i + 13].color = new Color(1f, 1f, 1f);
                        }

                        if (Constants.spriteData[i].Lock == 0 || Constants.spriteData[i].Lock == 1)
                        {
                            sLeaser.sprites[i + 13].SetPosition(Utilities.RotateAroundPoint(sLeaser.sprites[Constants.spriteData[i].Lock].GetPosition(), Utilities.MultiplyVector2ByFloats(Constants.spriteData[i].Offset, sLeaser.sprites[Constants.spriteData[i].Lock].scaleX, sLeaser.sprites[Constants.spriteData[i].Lock].scaleY) * ((Constants.spriteData[Constants.spriteData.Length - 1] != null && Constants.spriteData[i].Lock == Constants.spriteData[Constants.spriteData.Length - 1].Lock) ? 2 : 1), -sLeaser.sprites[Constants.spriteData[i].Lock].rotation));
                            sLeaser.sprites[i + 13].rotation = sLeaser.sprites[Constants.spriteData[i].Lock].rotation + Constants.spriteData[i].Rotation;
                        }
                        else
                        {
                            //Vector2 point = (Constants.spriteData[i].Lock == 2) ? Utilities.GetPointC(scanline.tail_0_p, scanline.tail_0_c) : (Constants.spriteData[i].Lock == 3) ? Utilities.GetPointC(scanline.tail_1_p, scanline.tail_1_c) : (Constants.spriteData[i].Lock == 4) ? Utilities.GetPointC(scanline.tail_2_p, scanline.tail_2_c) : Vector2.zero;
                            //sLeaser.sprites[i + 13].SetPosition(Utilities.RotateAroundPoint(point - rCam.pos, Constants.spriteData[i].Offset, -Utilities.AngleBetween((Vector2)((Constants.spriteData[i].Lock - 2 == 0) ? sLeaser.sprites[1].GetPosition() : self.tail[Constants.spriteData[i].Lock - 3].pos), self.tail[Constants.spriteData[i].Lock - 2].pos, self.tail[Constants.spriteData[i].Lock - 1].pos)));
                            sLeaser.sprites[i + 13].SetPosition(Utilities.RotateAroundPoint(self.tail[Constants.spriteData[i].Lock - 2].pos - rCam.pos, Constants.spriteData[i].Offset, -Utilities.AngleBetween((Vector2)((Constants.spriteData[i].Lock - 2 == 0) ? sLeaser.sprites[1].GetPosition() : self.tail[Constants.spriteData[i].Lock - 3].pos), self.tail[Constants.spriteData[i].Lock - 2].pos, self.tail[Constants.spriteData[i].Lock - 1].pos)));
                            sLeaser.sprites[i + 13].rotation = Utilities.AngleBetween((Vector2)((Constants.spriteData[i].Lock - 2 == 0) ? sLeaser.sprites[1].GetPosition() : self.tail[Constants.spriteData[i].Lock - 3].pos), self.tail[Constants.spriteData[i].Lock - 2].pos, self.tail[Constants.spriteData[i].Lock - 1].pos) + Constants.spriteData[i].Rotation;
                        }
                    }
                }

                //scanline.sOffset++;

                // REMOVE OR COMMENT THIS AFTER ALL FUR IS IN PLACE

                if (Constants.spriteData[Constants.spriteData.Length - 1] != null)
                {
                    Constants.ScanLineMemory.TryGetValue(self.player, out var scanline);

                    if (scanline.pckp)
                    {
                        float multiplier = 0.25f;

                        scanline.xOffset += multiplier * scanline.x;
                        scanline.yOffset += multiplier * scanline.y;

                        float rotationMultiplier = 2.5f;

                        if (scanline.thrw)
                        {
                            scanline.rOffset += rotationMultiplier;
                        }
                        if (scanline.jmp)
                        {
                            scanline.rOffset -= rotationMultiplier;
                        }
                    }

                    Debug.Log($"SPRITE LOCATION INFORMATION : ({scanline.xOffset}f, {scanline.yOffset / 2}f), {scanline.rOffset % 360}f");

                    //sLeaser.sprites[Constants.spriteData.Length + 12].color = new Color(1f, 1f, 1f);

                    //sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].scaleX = 2;
                    //sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].scaleY = 2;

                    sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].color = new Color(1f, 1f, 1f);
                    sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].alpha = 0.5f;

                    //sLeaser.sprites[Constants.spriteData.Length + 12].SetPosition(Utilities.RotateAroundPoint(sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].GetPosition(), -new Vector2(scanline.xOffset, scanline.yOffset), -sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].rotation));
                    //sLeaser.sprites[Constants.spriteData.Length + 12].SetPosition(Utilities.RotateAroundPoint(sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].GetPosition(), new Vector2(scanline.xOffset, scanline.yOffset) * 2, -sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].rotation));
                    //sLeaser.sprites[Constants.spriteData.Length + 12].rotation = sLeaser.sprites[Constants.spriteData[Constants.spriteData.Length - 1].Lock].rotation + scanline.rOffset;

                    sLeaser.sprites[Constants.spriteData.Length + 12].SetPosition(Utilities.RotateAroundPoint(self.tail[Constants.spriteData[Constants.spriteData.Length - 1].Lock - 2].pos - rCam.pos, new Vector2(scanline.xOffset, scanline.yOffset), -Utilities.AngleBetween((Vector2)((Constants.spriteData[Constants.spriteData.Length - 1].Lock - 2 == 0) ? sLeaser.sprites[1].GetPosition() : self.tail[Constants.spriteData[Constants.spriteData.Length - 1].Lock - 3].pos), self.tail[Constants.spriteData[Constants.spriteData.Length - 1].Lock - 2].pos, self.tail[Constants.spriteData[Constants.spriteData.Length - 1].Lock - 1].pos)));
                    //sLeaser.sprites[Constants.spriteData.Length + 12].SetPosition(self.tail[0].pos - rCam.pos + new Vector2(30f, 0f));
                    sLeaser.sprites[Constants.spriteData.Length + 12].rotation = Utilities.AngleBetween((Vector2)((Constants.spriteData[Constants.spriteData.Length - 1].Lock - 2 == 0) ? sLeaser.sprites[1].GetPosition() : self.tail[Constants.spriteData[Constants.spriteData.Length - 1].Lock - 3].pos), self.tail[Constants.spriteData[Constants.spriteData.Length - 1].Lock - 2].pos, self.tail[Constants.spriteData[Constants.spriteData.Length - 1].Lock - 1].pos) + scanline.rOffset;
                }
            }
        }
    }
}