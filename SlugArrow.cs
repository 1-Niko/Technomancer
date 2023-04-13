using System;
using UnityEngine;

namespace Slugpack
{
    public class SlugArrow : CosmeticSprite
    {
        public SlugArrow(Vector2 pos, float scanline, Color colour)
        {
            this.pos = pos;
            this.scanline = scanline;
            this.colour = colour;
        }
        public SlugArrow(Vector2 pos, float scanline, Color colour, Creature stickToCreature)
        {
            this.pos = pos;
            this.scanline = scanline;
            this.colour = colour;
            this.creature = stickToCreature;
        }
        public SlugArrow(Vector2 pos, float scanline, Color colour, PhysicalObject stickToItem)
        {
            this.pos = pos;
            this.scanline = scanline;
            this.colour = colour;
            this.item = stickToItem;
        }

        float RoundToNearest(float x, float n) => (float)Math.Round(x / n) * n;

        public override void Update(bool eu)
        {
            this.scanline -= 0.5f;
            if (this.sLeaser != null && room.ViewedByAnyCamera(this.pos, 20f))
            {
                if (this.creature != null)
                {
                    this.pos = this.creature.mainBodyChunk.pos + new Vector2(0f, 35f);

                    if (this.creature.inShortcut)
                    {
                        Destroy();
                    }
                }
                if (this.item != null)
                {
                    this.pos = this.item.firstChunk.pos + new Vector2(0f, 15f);
                }

                for (int i = 0; i < spritecount; i++)
                {
                    float addToScanline = (i < 2) ? 8 * i - 4 : ((i > 1 && i < 30) || i == 31) ? (RoundToNearest(i, 4) / 8) *
                        (((float)Math.Round((Math.Sin(Math.PI * (i + 0.5f)) + 1) / 2) * 2) - 1) : -(RoundToNearest(i, 4) / 8) + 21.5f;

                    if (Math.Sin(scanline + addToScanline) < 0)
                    {
                        sLeaser.sprites[i].isVisible = false;
                    }
                }
            }
        }

        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            this.sLeaser ??= sLeaser;
            for (int i = 0; i < spritecount; i++)
            {
                sLeaser.sprites[i].element = Futile.atlasManager.GetElementWithName("pixel");
                sLeaser.sprites[i].scaleX = (i < 2) ? 9 : 1;
                sLeaser.sprites[i].scaleY = 1;
                sLeaser.sprites[i].color = colour;
                sLeaser.sprites[i].isVisible = true;

                float depth = (i < 2) ? 8 * i - 4 : Math.Min(RoundToNearest(i, 4) / 8, (-RoundToNearest(i, 4) * 0.0275f) + 4.73f) *
                    (((float)Math.Round((Math.Sin(Math.PI * (i + 0.5f)) + 1) / 2) * 2) - 1); ;

                float newXValue = Math.Min((-RoundToNearest(i, 4) * 0.0277325f) + 4.777f, Math.Min(Math.Max(8 * (i - 1.25f), 0), 4)) *
                    Math.Min(-(((float)Math.Round((Math.Sin((Math.PI * (i + 0.5f)) / 2) + 1) / 2) * 2) - 1), Math.Abs(-8 * (i - 3)) - 1);

                float newYValue = Math.Max((RoundToNearest(i - 4, 4) / 8) - 21, -18);

                Vector2 tempVector = new(newXValue, newYValue);

                float addToScanline = (i < 2) ? 8 * i - 4 : ((i > 1 && i < 30) || i == 31) ? (RoundToNearest(i, 4) / 8) *
                    (((float)Math.Round((Math.Sin(Math.PI * (i + 0.5f)) + 1) / 2) * 2) - 1) : -(RoundToNearest(i, 4) / 8) + 21.5f;


                if (Math.Sin(scanline + addToScanline) < 0)
                    sLeaser.sprites[i].isVisible = false;

                sLeaser.sprites[i].SetPosition(pos - rCam.pos - tempVector + new Vector2(((pos - rCam.pos).x - 683) / 750f * depth,
                    ((pos - rCam.pos).y - 384) / 450f * depth));
            }
            base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
        }

        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[spritecount];
            for (int i = 0; i < spritecount; i++)
            { sLeaser.sprites[i] = new FSprite("pixel", true); }

            AddToContainer(sLeaser, rCam, null);
        }

        public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            newContatiner ??= rCam.ReturnFContainer("Foreground");
            foreach (FSprite fsprite in sLeaser.sprites)
            {
                fsprite.RemoveFromContainer();
                newContatiner.AddChild(fsprite);
            }
        }

        public PhysicalObject item;

        public Creature creature;

        public RoomCamera.SpriteLeaser sLeaser;

        public Color colour;

        public float scanline;

        public int width;

        public int spritecount = 173;
    }
}