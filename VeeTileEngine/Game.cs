#region
using SFML.Window;
#endregion

namespace VeeTileEngine
{
    public class Game
    {
        public int Turn { get; set; } // Current turn
        public int SizeX { get; set; } // Width
        public int SizeY { get; set; } // Height
        public int SizeZ { get; set; } // Depth (number of layers)
        public Manager Manager { get; private set; } // Game Entity manager
        public Gamemode Gamemode { get; set; } // Gamemode

        public Game(Gamemode mGamemode)
        {
            Gamemode = mGamemode;
            Gamemode.Game = this;
            Gamemode.Initialize();
        } // Constructor      

        public void SendInput(KeyCode mKeyCode)
        {
            Gamemode.SendInput(mKeyCode);
        }
        public void SendMouseInput(MouseButton mMouseButton)
        {
            Gamemode.SendMouseInput(mMouseButton);
        }

        public virtual void Initialize()
        {
            Manager = new Manager(this, SizeX, SizeY, SizeZ);
        }
        
        public virtual void Reset()
        {
            Turn = 0;
        } // Resets the game
        public virtual void Update()
        {
            Manager.Update();
            Turn++;
        } // Called every turn to update the EntityManager
        
    }
}