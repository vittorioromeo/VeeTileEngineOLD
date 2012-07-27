#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace VeeTileEngine
{
    public class Manager
    {
        public Game Game { get; private set; } // Game
        public int SizeX { get; private set; } // Width
        public int SizeY { get; private set; } // Height
        public int SizeZ { get; private set; } // Depth (number of layers)
        public Entity[, ,] Entities { get; private set; } // Entity array

        public Manager(Game mGame, int mSizeX, int mSizeY, int mSizeZ)
        {
            Game = mGame;
            SizeX = mSizeX;
            SizeY = mSizeY;
            SizeZ = mSizeZ;
            Entities = new Entity[SizeX,SizeY,SizeZ];
        } // Constructor
      
        public Entity this[int iX, int iY, int iZ]
        {
            get { return Get(iX, iY, iZ); }
            set { Set(iX, iY, iZ, value); }
        } // Indexer

        public void Set(int mX, int mY, int mZ, Entity mEntity, bool mCheckOutOfBoundaries = true)
        {
            if (mCheckOutOfBoundaries)
            {
                if (IsOutOfBoundaries(mX, mY, mZ))
                {
                    throw new Exception("Set - out of boundaries!");
                }
            }

            Entities[mX, mY, mZ] = mEntity;
            mEntity.SetPosition(mX, mY, mZ);

            if (mEntity.ManagerInitialized == false)
            {
                mEntity.Initialize();
                mEntity.ManagerInitialized = true;
            }
        } // Set X, Y, Z with an entity
        public Entity Get(int mX, int mY, int mZ, bool mCheckOutOfBoundaries = true)
        {
            if (mCheckOutOfBoundaries)
            {
                if (IsOutOfBoundaries(mX, mY, mZ))
                {
                    throw new Exception("Get - out of boundaries!");
                }
            }

            return Entities[mX, mY, mZ];
        } // Get an entity at X, Y, Z

        public bool IsOutOfBoundaries(int mX, int mY, int mZ)
        {
            return mX < 0 || mY < 0 || mZ < 0 || mX > SizeX - 1 || mY > SizeY - 1 || mZ > SizeZ - 1;
        } // Check if X, Y, Z is out of boundaries

        public void Update()
        {
            List<Entity> toUpdate = new List<Entity>();

            for (int iZ = 0; iZ < SizeZ; iZ++)
            {
                for (int iY = 0; iY < SizeY; iY++)
                {
                    for (int iX = 0; iX < SizeX; iX++)
                    {
                        Entity entity = Entities[iX, iY, iZ];

                        if (entity.Alive
                            && entity.RequiresUpdate)
                        {
                            toUpdate.Add(entity);
                        }
                    }
                }
            }

            toUpdate = toUpdate.OrderBy(x => x.OrderOfUpdate).ToList();

            for (int i = 0; i < toUpdate.Count; i++)
            {
                Entity entity = toUpdate[i];
                entity.Update();
            }
        } // Called every turn to update all the entities
    }
}