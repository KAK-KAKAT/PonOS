using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using Sys = Cosmos.System;

namespace ponOS
{
    public class Kernel : Sys.Kernel
    {
        Sys.FileSystem.CosmosVFS fs;
        string currentdir = @"0:\";

        protected override void BeforeRun()
        {
            fs = new Sys.FileSystem.CosmosVFS();
            Console.WriteLine("Regestration FS");
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.WriteLine("FS Registred");
            Console.WriteLine("Creating Dir List");
            Console.WriteLine("Dir List Created");
            Thread.Sleep(1000);
            Console.Clear();
            Console.Write("PonOS booted.  ");
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("OK");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("]");
            Thread.Sleep(2000);
            Console.Clear();

        }

        protected override void Run()
        {
        J:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("<PonOS>>" + currentdir + ">|");
            Console.ForegroundColor = ConsoleColor.White;
            list();
        }

        public void list()
        {
        A:
            var input = Console.ReadLine();
            string filename;
            string dirnames;
            //Dir list 
            var dirs = new ArrayList();
            try
            {
                var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(currentdir);
                foreach (var directoryEntry in directory_list)
                {
                    try
                    {
                        var entry_type = directoryEntry.mEntryType;
                        if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                        {
                            dirs.Add(directoryEntry.mName);
                        }
                    }
                    catch (Exception e)
                    {
                        
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
            //end Dir List
            switch (input)
            {
                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("<Command>: " + input + " :<not found in list>");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "help":
                    Console.WriteLine("help - list help\n" +
                        "clear - clear console\n" +
                        "power - system logoff\n" +
                        "specX - computer Specification\n" +
                        "crefl - creating file\n" +
                        "remfl - removing file\n" +
                        "credr - create dirictory\n" +
                        "remdr - removing dirictory\n" +
                        "sysArt - system art");

                    break;

                case "":
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "power":
                    Console.WriteLine("(S)hutdown/(R)eboot");
                    var power = Console.ReadLine();
                    switch (power)
                    {
                        default:
                            break;
                        case "S":
                            Cosmos.System.Power.Shutdown();
                            break;
                        case "s":
                            Cosmos.System.Power.Shutdown();
                            break;
                        case "R":
                            Cosmos.System.Power.Reboot();
                            break;
                        case "r":
                            Cosmos.System.Power.Reboot();
                            break;
                    }
                    break;
                case "sysArt":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("" +
                        "                         &\n" +
                        "                     **//*//**   /\n" +
                        "               *******////////////*  (\n" +
                        "           ,*********/////////////////* /\n" +
                        "         ************////////////////((((**##\n" +
                        "       ,**********/////////////////(((((,%(**#\n" +
                        "   % ,,,********%**////////////////(((((&&&#(**#\n" +
                        "  * ,,,*******#&&&///////////////%%%%((((#####(*##\n" +
                        " & ,,,********/&&*////%%%%%%%%%%%%%%%%%(######((*#\n" +
                        "  ,,***/*****////////%%%%%%%%%%%%%%%%%%%(((((((((*\n" +
                        "( ***///*/*//////////%%%%%#########%%%%%(((((((((*\n" +
                        " *////////////////////%%############%%(((((((((((*\n" +
                        " ,///((/////////////////###########%(##((((((((((*\n" +
                        " ,,///((((((((((((((((((((##((((((((((((((((((((**\n" +
                        "  **/(((((((((((((((((((((((((((((((((((((///((/*\n" +
                        "   ,,((((((((((((((((((((((((((((((((//////////#\n " +
                        "    (,,(((((((((((((((((((((((((((///////////(\n " +
                        "        ,**((((((((((((///////////////////%\n" +
                        "             %,/////(((((((((((/////%/%\n  ");
                    break;

                case "specX":
                    String CPU = Cosmos.Core.CPU.GetCPUVendorName() + " " + Cosmos.Core.CPU.GetCPUBrandString();
                    uint ram_amount = Cosmos.Core.CPU.GetAmountOfRAM();
                    ulong avalible_ram = Cosmos.Core.GCImplementation.GetAvailableRAM();
                    uint used_ram = Cosmos.Core.GCImplementation.GetUsedRAM();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("".PadLeft(54, '='));
                    Console.WriteLine("CPU::<" +
                        CPU + ">\nInitial System::<Cosmos OS 2022.11.21>" +
                        "\nOSVer::<PonOS 0.5>\nRAM::<"
                        + ram_amount + "^MB>\nAvalible RAM::<"
                        + avalible_ram + "^MB>\nUsed RAM::<"
                        + used_ram + "^B>");
                    Console.WriteLine("".PadLeft(54, '='));
                    break;

                case "crefl":
                    Console.WriteLine("?File name?");
                    filename = Console.ReadLine();
                    fs.CreateFile(currentdir + filename);
                    break;

                case "remfl":
                    Console.WriteLine("?File name to del?");
                    filename = Console.ReadLine();
                    Sys.FileSystem.VFS.VFSManager.DeleteFile(currentdir + filename);
                    break;

                case "credr":
                    Console.WriteLine("?Dir name?");
                    dirnames = Console.ReadLine(); ;
                    fs.CreateDirectory(currentdir + dirnames);
                    break;

                case "remdr":
                    Console.WriteLine("?Dir name to del?");
                    dirnames = Console.ReadLine(); ;
                    Sys.FileSystem.VFS.VFSManager.DeleteDirectory(currentdir + dirnames, true);
                    break;

                case "log":
                    Console.WriteLine(currentdir);
                    break;

                case "cdir":
                    var commitdir = Console.ReadLine();

                        if (dirs.Contains(commitdir))
                        {
                        currentdir += commitdir + "\\";
                        }
                        else if (commitdir == ".")
                        {
                            currentdir = @"0:\";
                        }

                    break;
                case "ls":
                    try
                    {
                        var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(currentdir);
                        foreach (var directoryEntry in directory_list)
                        {
                            try
                            {
                                var entry_type = directoryEntry.mEntryType;
                                if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("| <File> |<" + directoryEntry.mName + ">");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.WriteLine("| <Directory> |<" + directoryEntry.mName + ">");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            catch (Exception e)
                            {
                                
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        
                        break;
                    }
                    break;

                case "AppX":
                    var app = new ArrayList();
                    Console.WriteLine("Progs List");
                    Console.WriteLine("[{0}]\n", string.Join(", ", app));
                    Console.Write("?Run? ");
                    var run = Console.ReadLine();
                    if (app.Contains(run))
                    {
                        
                    }
                    break;

            }
        }
    }
}
