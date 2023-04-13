using System;
using UnityEngine;

namespace Slugpack
{
    public class SlugHologram : CosmeticSprite
    {
        public SlugHologram(Vector2 pos, Color colour, int lifetime)
        {
            this.pos = pos;
            this.colour = colour;
            this.lifetime = lifetime;
        }

        public override void Update(bool eu)
        {
            if (this.lifetime < 0)
            {
                //this.RemoveFromRoom();
                //this.room.RemoveObject(this);
                Destroy();
            }
            else
            {
                this.scanline += 0.3f;
                this.lifetime--;
            }
        }

        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            this.sLeaser ??= sLeaser;
            for (int i = 0; i < spritecount; i++)
            {
                sLeaser.sprites[i].element = Futile.atlasManager.GetElementWithName("pixel");
                sLeaser.sprites[i].color = colour;
                sLeaser.sprites[i].isVisible = true;

                if (Math.Sin(1.5f * (scanline + Holograms.HologramA[i].y)) - UnityEngine.Random.value < 0)
                    sLeaser.sprites[i].isVisible = false;

                sLeaser.sprites[i].SetPosition(pos - rCam.pos - new Vector2(Holograms.HologramA[i].x, Holograms.HologramA[i].y) + new Vector2(((pos - rCam.pos).x - 683) / 750f * Holograms.HologramA[i].z,
                    ((pos - rCam.pos).y - 384) / 450f * Holograms.HologramA[i].z));
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

        public RoomCamera.SpriteLeaser sLeaser;

        public Color colour;

        public float scanline;

        public int lifetime;

        public int spritecount = Holograms.HologramA.Length;
    }
}