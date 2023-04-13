using System.Collections.Generic;
using UnityEngine;

namespace Slugpack
{
    static class DataStructures
    {
        public class Sprite
        {
            public Sprite(int Lock, int SpriteIndex, Vector2 Offset, float Rotation, Anchorpoint Anchor, Scale Scale)
            {
                this.Lock = Lock;
                this.SpriteIndex = $"FurTuft{SpriteIndex}";
                this.Offset = Offset;
                this.Rotation = Rotation;
                this.AnchorX = Anchor.x;
                this.AnchorY = Anchor.y;
                this.ScaleX = Scale.x;
                this.ScaleY = Scale.y;
            }

            public int Lock { get; set; }
            public string SpriteIndex { get; set; }
            public Vector2 Offset { get; set; }
            public float Rotation { get; set; }
            public float AnchorX { get; set; }
            public float AnchorY { get; set; }
            public float ScaleX { get; set; }
            public float ScaleY { get; set; }
        }

        public class Anchorpoint
        {
            public Anchorpoint(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public float x { get; set; }
            public float y { get; set; }
        }

        public class Scale
        {
            public Scale(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public float x { get; set; }
            public float y { get; set; }
        }

        public class Pixel
        {
            public Pixel(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
        }

        public class Lock
        {
            public Lock(ShortcutData[] shortcuts, Room[] rooms, int time, SlugHologram[] holograms)
            {
                this.shortcuts = shortcuts;
                this.rooms = rooms;
                this.time = time;
                this.holograms = holograms;
            }

            public ShortcutData[] shortcuts {  get; set; }
            public Room[] rooms {  get; set; }
            public int time {  get; set; }
            public SlugHologram[] holograms { get; set; }
        }
    }
}