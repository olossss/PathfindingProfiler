﻿/*
 * File: AiBotDijkstra.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a bot able to follow a path generated by either the Dijkstra or A* algorithms.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    class AiBotDijkstra : AiBotBase
    {
        // Current index in the path given
        private int currentPathIndex;

        public AiBotDijkstra(Texture2D texture, int x, int y) : base(texture, x, y) { currentPathIndex = 0; }

        //this function is filled in by a derived class: must use SetNextGridLocation to actually move the bot
        protected override void ChooseNextGridLocation(Map level, Player plr)
        {
            try
            {
                if (level.pathfinder.GetAlgorithm() != PathfinderAlgorithm.AStar && level.pathfinder.GetAlgorithm() != PathfinderAlgorithm.Dijkstra)
                    throw new Exception("Wrong bot used for pathfinding algorithm.");

                List<Coord2> path = level.pathfinder.GetPath();
                if (path.Count > 0 && currentPathIndex != path.Count - 1)
                {
                    SetNextGridPosition(path[currentPathIndex], level);
                    currentPathIndex++;
                }
                else
                    currentPathIndex = 0;
            }
            catch(Exception e)
            {
                Console.WriteLine("Bot Exception: " + e.Message);
            }
        }
    }
}
