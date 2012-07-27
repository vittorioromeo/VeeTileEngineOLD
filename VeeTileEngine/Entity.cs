#region
using System;
using System.Collections.Generic;
using System.Drawing;
using SFML.Graphics;
using SFML.Window;

#endregion

namespace VeeTileEngine
{
    public class Entity
    {
        public Manager Manager { get; set; } // automatically set by the EntityManager!
        public Game Game { get; set; } // Game
        public Gamemode Gamemode { get; set; } // Gamemode
        public bool ManagerInitialized { get; set; } // automatically set by the EntityManager!
        public int X { get; set; } // X coordinate - automatically set by the EntityManager!
        public int Y { get; set; } // Y coordinate - automatically set by the EntityManager!
        public int Z { get; set; } // Z coordinate - automatically set by the EntityManager!
        public string Name { get; set; } // Name       
        public bool Alive { get; set; } // This entity is alive
        public List<string> Flags { get; set; } // Entity flags
        public Sprite Sprite { get; set; } // Entity sprite
        public int OrderOfUpdate { get; set; } // Order of update (0 first)
        public int OrderOfDraw { get; set; } // Order of draw (0 first)
        public bool RequiresUpdate { get; set; } // Must be updated?
        public bool RequiresDraw { get; set; } // Must be drawn?

        public Entity(Manager mManager, string mName)
        {
            Manager = mManager;
            Game = Manager.Game;
            Gamemode = Game.Gamemode;
            Name = mName;
            Alive = true;
            RequiresUpdate = true;
            RequiresDraw = true;
            Flags = new List<string>();
            Sprite = new Sprite();
        } // Constructor

        public void SetPosition(int mX, int mY, int mZ)
        {
            X = mX;
            Y = mY;
            Z = mZ;
        } // Sets X, Y, Z variables - automatically called by the EntityManager!

        public void Swap(int mX, int mY, int mZ)
        {
            Entity startEntity = this;
            Entity endEntity = Manager[mX, mY, mZ];
            int endX = endEntity.X;
            int endY = endEntity.Y;
            int endZ = endEntity.Z;

            Manager.Set(startEntity.X, startEntity.Y, startEntity.Z, endEntity);
            Manager.Set(endX, endY, endZ, startEntity);
        } // Swaps this entity with another entity in the EntityManager
        public void Swap(Helper.Direction mDirection)
        {
            List<int> direction = Helper.DirectionToInt(mDirection);
            int x = direction[0];
            int y = direction[1];

            Swap(X + x, Y + y, Z);
        } // Swaps this entity with another entity in the EntityManager

        public List<Entity> GetAdjacentEntities(int mOffset = 1)
        {
            List<Entity> result = new List<Entity>();

            for (int iY = Y - mOffset; iY < Y + mOffset + 1; iY++)
            {
                for (int iX = X - mOffset; iX < X + mOffset + 1; iX++)
                {
                    if (iX != X
                        || iY != Y)
                    {
                        result.Add(Manager[iX, iY, Z]);
                    }
                }
            }

            return result;
        } // Returns a list containing the adjacent entities (not including this entity)

        public virtual void Initialize()
        {
            
        }
        public virtual void Update()
        {
        } // Called every turn to update the entity       
        public virtual void Draw()
        {
            if (Gamemode.Scroll)
            {
                Sprite.Position = new Vector2(X * Gamemode.TileSize + Gamemode.ScrollX,
                    Y * Gamemode.TileSize + Gamemode.ScrollY);
            }
            else
            {
                Sprite.Position = new Vector2(X * Gamemode.TileSize,
                    Y * Gamemode.TileSize);
            }
            Sprite.Width = Gamemode.TileSize;
            Sprite.Height = Gamemode.TileSize;
            SFML.RenderWindow.Draw(Sprite);
        } // Draws entity to SFML.RenderWindow

        public virtual void Destroy(Entity mEntity)
        {
            Alive = false;
            Manager.Set(X, Y, Z, mEntity);
        } // Replaces this entity with another one
    }
}