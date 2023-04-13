using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

namespace Slugpack
{
    static class WeakTables
    {
        public class ScanLine
        {
            public int holdTime = 0;
            public Vector2 position = Vector2.zero;
            public SlugArrow arrow = null;

            public bool heldup = false;
            public bool helddown = false;
            public bool heldleft = false;
            public bool heldright = false;

            public int x;
            public int y;

            public bool jmp;
            public bool thrw;

            public bool pckp;

            public bool inputHoldThrw = false;
            public bool inputHoldJmp = false;

            public float debugTimer = 0f;

            public Vector2 tail_0_c = Vector2.zero;
            public Vector2 tail_0_p = Vector2.zero;

            public Vector2 tail_1_c = Vector2.zero;
            public Vector2 tail_1_p = Vector2.zero;

            public Vector2 tail_2_c = Vector2.zero;
            public Vector2 tail_2_p = Vector2.zero;

            public int murdered_neurons = 0;

            // Debug Variables

            public float xOffset = 0f;
            public float yOffset = 0f;
            public float rOffset = 0f;

            public float sOffset = 0f;
        }

        public class VultureStuff
        {
            public int timer = 0;
            public int thruster = -1;
        }

        public class ShortcutList
        {
            public List<DataStructures.Lock> locks = new();
        }

        public class GraphicsData
        {
            public int firstIndex = 0;

            public FSprite[] sprites = null;

            public TailSegment[] tail = null;
        }

        public class OracleData
        {
            public bool unlockShortcuts = false;

            public bool endMain = false;

            public int overstayTimer = -400;

            public bool setTimer = false;

            public int timerOffset = 0;

            public bool seenPlayer = false;

            public int timer = 0;
        }
    }
}