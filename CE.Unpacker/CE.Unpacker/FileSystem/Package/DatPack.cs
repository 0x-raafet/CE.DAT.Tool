using System;
using System.Collections.Generic;
using System.IO;

namespace CE.Unpacker
{
    class DatPack
    {
        static List<DatEntry> m_EntryTable = new List<DatEntry>();

        public static void iDoIt(String m_Directory, String m_OutputFile)
        {
            //DatHashList.iLoadProject();

            using (BinaryWriter TDatStream = new BinaryWriter(File.Create(m_OutputFile)))
            {
                var files = Directory.GetFiles(m_Directory, "*.*", SearchOption.AllDirectories);

                Byte[] lpReserved = new Byte[files.Length * 12]; // or 32768 bytes
                TDatStream.Write(lpReserved);

                foreach (var file in files)
                {
                    var TEntry = new DatEntry();

                    byte[] fileData = File.ReadAllBytes(file);

                    if (!file.Contains("__Unknown"))
                    {
                        String m_FileName = file.Replace(m_Directory, "");
                        TEntry.dwHash = DatHash.iGetHash(Path.GetFileName(file));
                    }
                    else
                    {
                        String m_FileName = Path.GetFileNameWithoutExtension(file);
                        TEntry.dwHash = Convert.ToUInt32(m_FileName, 16);
                    }

                    //UInt32 dwHash = DatHash.iGetHash(Path.GetFileName(file));

                    TEntry.dwOffset = (UInt32)TDatStream.BaseStream.Position;
                    TEntry.dwSize = fileData.Length;

                    //var TEntry = new DatEntry
                    //{
                    //    dwHash = dwHash,
                    //    dwOffset = dwOffset,
                    //    dwSize = dwSize,
                    //};

                    m_EntryTable.Add(TEntry);
                    TDatStream.Write(fileData);
                }

                TDatStream.Seek(0, SeekOrigin.Begin);
                foreach (var entry in m_EntryTable)
                {
                    TDatStream.Write(entry.dwHash);
                    TDatStream.Write(entry.dwOffset);
                    TDatStream.Write(entry.dwSize);
                }

                TDatStream.Write(0); // Write end of entries marker

                TDatStream.Dispose();
            }

            Utils.iSetInfo("[REPACKING]: Completed");
        }
    }
}