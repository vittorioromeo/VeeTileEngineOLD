#region
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CSharp;
#endregion

namespace VeeTileEngine
{
    public static class Program
    {
        public static CSharpCodeProvider CSharpCodeProvider { get; set; } // Required for compiling
        public static CompilerParameters CompilerParameters { get; set; } // Required for compiling
        public static Game Game { get; set; } // Current game - design may change in the future

        public static void Main(string[] args)
        {
            Initialize();

            if (args.GetLength(0) == 0)
            {               
                MenuDebug();
            }
            else
            {
                Game = new Game(CompileGamemode(args[0]));
                SFML.Initialize(Game);
            }

        } // Entry point - args selects the gamemode

        public static void Initialize()
        {
            InitializeVariables();
        }
        public static void InitializeVariables()
        {
            try
            {
                CSharpCodeProvider = new CSharpCodeProvider();
                CompilerParameters = new CompilerParameters
                                         {
                                             GenerateExecutable = false,
                                             GenerateInMemory = true,
                                             CompilerOptions = "/target:library"
                                         };

                CompilerParameters.ReferencedAssemblies.Add("System.dll");
                CompilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                CompilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                CompilerParameters.ReferencedAssemblies.Add("System.Net.dll");
                CompilerParameters.ReferencedAssemblies.Add("System.Drawing.dll");
                CompilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                CompilerParameters.ReferencedAssemblies.Add("sfmlnet-audio.dll");
                CompilerParameters.ReferencedAssemblies.Add("sfmlnet-graphics.dll");
                CompilerParameters.ReferencedAssemblies.Add("sfmlnet-window.dll");
                CompilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                CompilerParameters.ReferencedAssemblies.Add("VeeTileEngine.exe");
            }
            catch
            {
                throw new Exception("Error initializing dynamic compilation.");
            }
        }

        public static void MenuDebug()
        {
            while (true)
            {
                Console.WriteLine("Insert gamemode name");
                Game = new Game(CompileGamemode(Console.ReadLine()));
                SFML.Initialize(Game);
            }
        }

        public static Gamemode CompileGamemode(string mGamemodeName)
        {
            string gamemodeName = mGamemodeName;

            DirectoryInfo directoryInfo = new DirectoryInfo(Helper.DataPath + string.Format(@"\Gamemodes\{0}", gamemodeName));
            FileInfo gamemodeFileInfo = new FileInfo(directoryInfo.FullName + string.Format(@"\{0}.cs", directoryInfo.Name));

            DirectoryInfo codeDirectory = new DirectoryInfo(Helper.DataPath + string.Format("\\Gamemodes\\{0}\\Code", gamemodeName));
            DirectoryInfo entitiesDirectory = new DirectoryInfo(Helper.DataPath + string.Format("\\Gamemodes\\{0}\\Entities", gamemodeName));

            List<FileInfo> codesFileInfos = codeDirectory.GetFiles().ToList();
            List<FileInfo> entitiesFileInfos = entitiesDirectory.GetFiles().ToList();

            List<string> filePathsToCompile = new List<string> { gamemodeFileInfo.FullName };
            filePathsToCompile.AddRange(codesFileInfos.Select(fileInfo => fileInfo.FullName));
            filePathsToCompile.AddRange(entitiesFileInfos.Select(fileInfo => fileInfo.FullName));

            CompilerResults compilerResults = CSharpCodeProvider.CompileAssemblyFromFile(CompilerParameters,
                filePathsToCompile.ToArray());

            if (!compilerResults.Errors.HasErrors)
            {
                Gamemode result =
                    Activator.CreateInstance(compilerResults.CompiledAssembly.GetType(gamemodeName)) as Gamemode;

                if (result != null)
                {
                    result.GamemodePath = directoryInfo.FullName;
                    return result;
                }
            }

            StreamWriter streamWriter = File.CreateText(Helper.DataPath + @"/CompilationErrors.txt");

            foreach (CompilerError compilerError in compilerResults.Errors)
            {
                Console.Write(compilerError);
                Console.WriteLine("");

                streamWriter.Write(compilerError);
                streamWriter.WriteLine("");
            }

            streamWriter.Flush();
            streamWriter.Close();


            return null;
        } // Compile an entire Gamemode and return a new instance of it
    }
}