using System;
using System.IO;

namespace CE.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Conflict Engine DAT Tool");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length < 3 || (args[0] != "unpack" && args[0] != "pack"))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    CE.Unpacker unpack <m_File> <m_Directory>");
                Console.WriteLine("    CE.Unpacker pack <m_Directory> <m_File>\n");
                Console.WriteLine("    m_File - Source or destination of DAT archive file");
                Console.WriteLine("    m_Directory - Source or destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    CE.Unpacker unpack E:\\Games\\CDS\\frontend.pc.dat D:\\Unpacked");
                Console.WriteLine("    CE.Unpacker pack D:\\Unpacked E:\\Games\\CDS\\frontend.pc.dat");
                Console.ResetColor();
                return;
            }

            String m_Mode = args[0];
            String m_FileOrDir = args[1];
            String m_Output = Utils.iCheckArgumentsPath(args[2]);

            if (m_Mode == "unpack")
            {
                if (!File.Exists(m_FileOrDir))
                {
                    Utils.iSetError("[ERROR]: Input DAT file -> " + m_FileOrDir + " <- does not exist");
                    return;
                }

                DatUnpack.iDoIt(m_FileOrDir, m_Output);
            }
            else if (m_Mode == "pack")
            {
                if (!Directory.Exists(m_FileOrDir))
                {
                    Utils.iSetError("[ERROR]: Input directory -> " + m_FileOrDir + " <- does not exist");
                    return;
                }

                DatPack.iDoIt(m_FileOrDir, m_Output);
            }
        }
    }
}
