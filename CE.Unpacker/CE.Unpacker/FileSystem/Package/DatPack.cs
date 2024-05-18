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
            DatHashList.iLoadProject();

            using (BinaryWriter TDatStream = new BinaryWriter(File.Create(m_OutputFile)))
            {
                var files = Directory.GetFiles(m_Directory, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    byte[] fileData = File.ReadAllBytes(file);
                    UInt32 dwHash = DatHash.iGetHash(Path.GetFileName(file));
                    UInt32 dwOffset = (UInt32)TDatStream.BaseStream.Position;
                    Int32 dwSize = fileData.Length;

                    var TEntry = new DatEntry
                    {
                        dwHash = dwHash,
                        dwOffset = dwOffset,
                        dwSize = dwSize,
                    };

                    m_EntryTable.Add(TEntry);
                    TDatStream.Write(fileData);
                }

                TDatStream.Write((UInt32)0); // Write end of entries marker

                foreach (var entry in m_EntryTable)
                {
                    TDatStream.Write(entry.dwHash);
                    TDatStream.Write(entry.dwOffset);
                    TDatStream.Write(entry.dwSize);
                }
            }

            Utils.iSetInfo("[REPACKING]: Completed");
        }
    }
}