#region
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
#endregion

namespace VeeTileEngine
{
    public class Gamemode
    {
        public string Name { get; set; } // Gamemode name
        public Game Game { get; set; } // Current game
        public int SFMLWindowWidth { get; set; } // Desired rendering window width
        public int SFMLWindowHeight { get; set; } // Desired rendering window height
        public string GamemodePath { get; set; } // Current gamemode path

        public Dictionary<string, Image> ResourcesImages { get; set; } // Resources images
        public Dictionary<string, Sound> ResourcesSounds { get; set; } // Resources sounds
        public Dictionary<string, Music> ResourcesMusic { get; set; } // Resources music
   
        public int TileSize { get; set; } // Tile size in pixels
        public bool Scroll { get; set; } // Scrolling
        public int ScrollX { get; set; } // Scrolling
        public int ScrollY { get; set; } // Scrolling

        public Gamemode(string mName)
        {
            Name = mName;
            ResourcesImages = new Dictionary<string, Image>();
            ResourcesSounds = new Dictionary<string, Sound>();
            LoadResources();
        }

        public void LoadResources()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Helper.DataPath + string.Format(@"\Gamemodes\{0}\Resources", Name));

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Name.EndsWith(".png"))
                {
                    ResourcesImages.Add(Path.GetFileNameWithoutExtension(fileInfo.Name), new Image(fileInfo.FullName));
                }

                if (fileInfo.Name.EndsWith(".wav"))
                {
                    ResourcesSounds.Add(Path.GetFileNameWithoutExtension(fileInfo.Name),
                        new Sound(new SoundBuffer(fileInfo.FullName)));
                }

                if (fileInfo.Name.EndsWith(".ogg"))
                {
                    ResourcesMusic.Add(Path.GetFileNameWithoutExtension(fileInfo.Name),
                        new Music(fileInfo.FullName));
                }
            }
        }

        public virtual void Initialize()
        {
        }
        public virtual void SendInput(KeyCode mKeyCode)
        {
        }
        public virtual void SendMouseInput(MouseButton mMouseButton)
        {
            
        }       
    }
}