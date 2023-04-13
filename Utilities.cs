using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Slugpack
{
    public class Utilities
    {
        public static void ShowMessage(RoomCamera roomCamera, string messageContents, int messageTime)
        {
            HUD.DialogBox dialogue = roomCamera.hud.InitDialogBox();
            dialogue.NewMessage(messageContents, messageTime);
        }

        public static string GetEquivalence(string inputText, string baseAcronym, SlugcatStats.Name character)
        {
            string text = "";

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

        public static Vector2 GetPointC(Vector2 pointA, Vector2 pointB)
        {
            // Calculate the direction vector from A to B
            Vector2 direction = pointB - pointA;

            // Normalize the direction vector to get a unit vector
            direction.Normalize();

            // Calculate the distance between A and C
            float distance = Vector2.Distance(pointA, pointB);

            // Calculate the position of point C along the same line in the direction from A to B
            Vector2 pointC = pointA + direction * distance;

            return pointC;
        }

        public static Vector2 MultiplyVector2ByFloats(Vector2 vector2, float x, float y)
        {
            return new Vector2(vector2.x * x, vector2.y * y);
        }

        public static float AngleBetween(Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ab = b - a;
            Vector2 bc = c - b;

            float angle = Vector2.Angle(ab, bc);

            // Determine the direction of the angle
            Vector3 cross = Vector3.Cross(ab, bc);
            if (cross.z > 0)
            {
                angle = 360 - angle;
            }

            return angle;
        }

        public static Vector2 RotateAroundPoint(Vector2 center, Vector2 offset, float degrees)
        {
            offset += center;
            float radians = (float)(degrees * Math.PI / 180.0);
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            float x = (offset.x - center.x) * cos - (offset.y - center.y) * sin + center.x;
            float y = (offset.x - center.x) * sin + (offset.y - center.y) * cos + center.y;
            return new Vector2(x, y);
        }

        public static float NegativeAbs(float x, float n, float X, float Y)
        {
            return -Mathf.Abs(n * (X - x)) + Y;
        }

        public static bool IsBelowFunction(Vector2 point, Vector2 center)
        {
            // Calculate the y-coordinate of the function at the x-coordinate of the point
            float functionY = -Mathf.Abs(point.x - center.x);

            // Check if the y-coordinate of the point is below the y-coordinate of the function
            if (point.y < center.y + functionY)
            {
                return true;
            }

            return false;
        }

        public static (List<Vector2> positions, List<PhysicalObject> creatures, List<PhysicalObject> items) GetEverything(Room room)
        {
            /*for (int i = 0; i < room.physicalObjects.Length; i++)
            {
                for (int j = 0; j < room.physicalObjects[i].Count; j++)
                {
                    Debug.Log($"GAMER INFORMATION : LAYER {i}, ITEM {j} IS {room.physicalObjects[i][j]}");
                }
            }*/

            var positions = room.shortcuts
                .Where(element => element.destNode != -1 && element.destNode < room.abstractRoom.connections.Length && room.abstractRoom.connections[element.destNode] != -1)
                .Where(element => room.ViewedByAnyCamera(room.MiddleOfTile(element.StartTile), 0f))
                .Select(element => room.MiddleOfTile(element.StartTile))
                .ToList();

            var creatures = room.physicalObjects[1]
                .Where(element => (element as Creature) is MirosBird || (element as Creature) is VultureGrub || (element as Creature) is Vulture || (element as Creature) is MoreSlugcats.Inspector || (element as Creature) is Overseer)
                .Where(element => room.ViewedByAnyCamera((element as Creature).mainBodyChunk.pos, 0f))
                .Where(element => !(element as Creature).dead)
                .ToList();

            var items = room.physicalObjects[2]
                .Where(element => (element as PlayerCarryableItem) is DataPearl || (element as PlayerCarryableItem) is OverseerCarcass)
                .Where(element => room.ViewedByAnyCamera((element as PlayerCarryableItem).firstChunk.pos, 0f))
                .ToList();

            items.AddRange(room.physicalObjects[0]
                .Where(element => (element is SSOracleSwarmer))
                .Where(element => room.ViewedByAnyCamera(element.firstChunk.pos, 0f))
                .ToList());

            return (positions, creatures, items);
        }

        public static (string nearestObjectType, Creature nearestCreature, PhysicalObject nearestItem, ShortcutData nearestShortcut, Vector2 nearestPosition) DetermineObjectFromPosition(Vector2 position, Room room)
        {
            var (positions, creatures, items) = GetEverything(room);

            creatures.ForEach(creature => positions.Add((creature as Creature).mainBodyChunk.pos));
            items.ForEach(item => positions.Add(item.firstChunk.pos));

            foreach (var shortcut in room.shortcuts)
            {
                //Debug.Log($"LOOK HERE : FUNCTION CALL : SHORTCUT : {room.MiddleOfTile(shortcut.StartTile)} : {position}");
                if (room.MiddleOfTile(shortcut.StartTile) == position)
                {
                    return ("shortcut", null, null, shortcut, position);
                }
            }

            foreach (var creature in creatures)
            {
                //Debug.Log($"LOOK HERE : FUNCTION CALL : CREATURE : {(creature as Creature).mainBodyChunk.pos} : {position}");
                if ((creature as Creature).mainBodyChunk.pos == position)
                {
                    return ("creature", (creature as Creature), null, new ShortcutData(), position);
                }
            }

            foreach (var item in items)
            {
                //Debug.Log($"LOOK HERE : FUNCTION CALL : ITEM : {(item as PlayerCarryableItem).firstChunk.pos} : {position}");
                if (item.firstChunk.pos == position)
                {
                    return ("item", null, item, new ShortcutData(), position);
                }
            }

            return ("none", null, null, new ShortcutData(), Vector2.zero);
        }

        public static List<Vector2> GetPointsInDirection(Vector2 position, Vector2 direction, List<Vector2> searchPositions)
        {
            var result = new List<Vector2>();

            foreach (var searchPosition in searchPositions)
            {
                if (searchPosition == position) continue; // skip the position itself

                var delta = searchPosition - position;
                var angle = Vector2.Angle(delta, direction);

                //Debug.Log($"LOOK HERE : FUNCTIONCALL : {direction} : {searchPosition} : {position}");

                if ((angle <= 45f || angle >= 315f)) // within 45 degrees either way of the direction
                {
                    result.Add(searchPosition);
                }
            }

            return result;
        }


        public static (List<Vector2> positions, string nearestObjectType, Creature nearestCreature, PhysicalObject nearestItem, Vector2 nearestPosition) GetPositions(Room room, Vector2 searchPosition, bool positionsOnly)
        {
            var (positions, creatures, items) = GetEverything(room);

            creatures.ForEach(creature => positions.Add((creature as Creature).mainBodyChunk.pos));
            items.ForEach(item => positions.Add(item.firstChunk.pos));

            if (positionsOnly)
            {
                return (positions, null, null, null, Vector2.zero);
            }

            string nearestObjectType = "";
            Creature nearestCreature = null;
            PhysicalObject nearestItem = null;

            Vector2 nearestPosition = Vector2.zero;
            float nearestDistance = Mathf.Infinity;

            foreach (var shortcut in room.shortcuts)
            {
                if (shortcut.destNode != -1 && shortcut.destNode < room.abstractRoom.connections.Length && room.abstractRoom.connections[shortcut.destNode] != -1)
                {
                    Vector2 position = room.MiddleOfTile(shortcut.StartTile);
                    float distance = RWCustom.Custom.Dist(position, searchPosition);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestPosition = position;
                        nearestObjectType = "shortcut";
                    }
                }
            }

            foreach (var creature in creatures)
            {
                Vector2 position = (creature as Creature).mainBodyChunk.pos;
                float distance = RWCustom.Custom.Dist(position, searchPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPosition = position;
                    nearestObjectType = "creature";
                    nearestCreature = creature as Creature;
                }
            }

            foreach (var item in items)
            {
                Vector2 position = item.firstChunk.pos;
                float distance = RWCustom.Custom.Dist(position, searchPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPosition = position;
                    nearestObjectType = "item";
                    nearestItem = item;
                }
            }

            return (positions, nearestObjectType, nearestCreature, nearestItem, nearestPosition);
        }

        public List<Vector2> CheckIfOnScreen(List<Vector2> positions, Room room)
        {
            return positions.Where(p => room.ViewedByAnyCamera(p, 0f)).ToList();
        }

        public static Vector2 FindNearest(Vector2 input, List<Vector2> vectors, Room room, List<Vector2> vectorsToIgnore = null)
        {
            var nearest = Vector2.zero;
            var distance = Mathf.Infinity;
            vectorsToIgnore ??= new List<Vector2>(); // Set to empty list if null

            foreach (var vector in vectors.Where(v => !vectorsToIgnore.Contains(v)).Where(p => room.ViewedByAnyCamera(p, 0f)))
            {
                var newDistance = Vector2.Distance(input, vector);
                if (newDistance < distance)
                    (nearest, distance) = (vector, newDistance);
            }

            return nearest;
        }
    }
}