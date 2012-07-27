#region
using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
#endregion

namespace VeeTileEngine
{
    public static class SFML
    {
        public static bool Running { get; set; } // Makes Run loop cycle if set to true
        public static Game CurrentGame { get; set; } // Current game that is being ran
        public static Gamemode CurrentGamemode { get; set; } // Current gamemode that is being ran
        public static RenderWindow RenderWindow { get; set; } // Rendering window
        public static int InputDelay { get; set; } // Time passed from the last input 
        public static int InputDelayMax { get; set; } // Max time that needs to pass from the last input
        public static Array KeyCodeArray { get; set; } // Input
        public static Array MouseButtonArray { get; set; } // Input

        public static void Initialize(Game mCurrentGame)
        {
            CurrentGame = mCurrentGame;
            CurrentGamemode = CurrentGame.Gamemode;

            InitializeVariables();

            Run();
        }
        public static void InitializeVariables()
        {
            KeyCodeArray = Enum.GetValues(typeof(KeyCode));
            MouseButtonArray = Enum.GetValues(typeof(MouseButton));
            Running = true;
            InputDelayMax = 9;

            RenderWindow =
                new RenderWindow(
                    new VideoMode((uint)CurrentGamemode.SFMLWindowWidth, (uint)CurrentGamemode.SFMLWindowHeight),
                    CurrentGamemode.Name);

            RenderWindow.SetFramerateLimit(60);

            RenderWindow.Show(true);
        }

        public static void Run()
        {
            while (Running)
            {
                RenderWindow.Clear();
                RunInput();
                RunDraw();
                RenderWindow.Display();
            }

            RenderWindow.Close();
        }
        public static void RunDraw()
        {
            List<Entity> entitiesToDraw = new List<Entity>();

            for (int i0 = 0; i0 < CurrentGame.Manager.Entities.GetLength(0); i0++)
                for (int i1 = 0; i1 < CurrentGame.Manager.Entities.GetLength(1); i1++)
                    for (int i2 = 0; i2 < CurrentGame.Manager.Entities.GetLength(2); i2++)
                    {
                        Entity entity = CurrentGame.Manager.Entities[i0, i1, i2];
                        if (entity.RequiresDraw)
                            entitiesToDraw.Add(entity);
                    }

            entitiesToDraw = entitiesToDraw.OrderBy(x => x.OrderOfDraw).ToList();

            for (int i = 0; i < entitiesToDraw.Count; i++)
            {
                Entity entity = entitiesToDraw[i];
                entity.Draw();
            }

        }
        public static void RunInput()
        {
            RenderWindow.DispatchEvents();

            if (InputDelay == 0)
            {
                foreach (KeyCode keyCode in KeyCodeArray)
                {
                    if (RenderWindow.Input.IsKeyDown(keyCode))
                    {
                        InputDelay = InputDelayMax;
                        CurrentGame.SendInput(keyCode);
                        break;
                    }
                }

                foreach (MouseButton mouseButton in MouseButtonArray)
                {
                    if (RenderWindow.Input.IsMouseButtonDown(mouseButton))
                    {
                        InputDelay = InputDelayMax;
                        CurrentGame.SendMouseInput(mouseButton);
                        break;
                    }
                }
            }
            else
            {
                InputDelay--;
            }
        }
    }
}