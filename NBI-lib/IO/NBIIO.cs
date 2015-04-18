using System;
using System.Collections.Generic;
using System.IO;

namespace TSDFF.IO
{
    /// <summary>
    /// Used for Input/Output interaction like Import and Export data files.
    /// </summary>
    /// <see cref="Data"/>
    public static class NBIIO
    {
        const byte Signature = 1; // The curret signature.

        /// <summary>
        /// Allows to read a file and get its data.
        /// </summary>
        /// <param name="path">The file format can be .NBI, .nbir, .chki ou .nbr</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static Data[] ImportFile(string path)
        {
            FileInfo info = new FileInfo(path);
            FileExt Ext;

            switch (info.Extension) // Check and set the extention of the file.
            {
                case "nbi":
                    Ext = FileExt.DataFile;
                    break;
                case "nbir":
                    Ext = FileExt.RawFile;
                    break;
                case "nbc":
                    Ext = FileExt.Chunk;
                    break;
                case "nbr":
                    Ext = FileExt.ChunkRoot;
                    break;
                default:
                    throw new InvalidOperationException("[ERROR] Invalid format.");
            }
            switch (Ext)
            {
                case FileExt.DataFile:
                    return TranslateDataFile(File.ReadAllBytes(path), true); // Uses TranslateDataFile() with signature verification.
                case FileExt.RawFile:
                    return new Data[] { Data.Translate(File.ReadAllBytes(path)) }; // Translated the only present data.
                case FileExt.Chunk:
                    return TranslateDataFile(File.ReadAllBytes(path), false); // Uses TranslateDataFile() without signature verification
                case FileExt.ChunkRoot:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException("[FATAL] Invalid or null format forcing.");
            }


        }


        /// <summary>
        /// To translate a file of data into a data table.
        /// </summary>
        /// <param name="filedata">Array of bytes of the file or the instance.</param>
        /// <returns>Data array containing all the values of the file or the instance.</returns>
        /// <exception cref="NBIIOExeption"></exception>
        public static Data[] TranslateDataFile(byte[] FileData, bool verifySignature)
        {
            int CurretByte = 0; // The position of the band.
            List<Data> _Data = new List<Data>(); // The Data List as a buffer
            byte[] ByteBuffer = new byte[] { }; // The buffer has octect to extracts the values.
            if (verifySignature)
            {
                CurretByte++;
            }
            if (FileData[0] == Signature || !verifySignature)
            {
                for (; CurretByte < FileData.Length; CurretByte++)
                {
                    ByteBuffer[ByteBuffer.Length + 1] = FileData[CurretByte];
                    if (FileData[CurretByte] == Data.Separator) // If the byte is a separator.
                    {
                        _Data.Add(Data.Translate(ByteBuffer)); // We defined the new Data.
                        ByteBuffer = new byte[] { }; // Create a new ByteBuffer.
                    }
                }
            }
            else
            {
                throw new NBIIOExeption("[ERROR] Failed to import the file (Invalid file signature). Have you forgot to disable verifySignature ?");
            }
            return _Data.ToArray();
        }
        /// <summary>
        /// Used for Export Datas to a file.
        /// </summary>
        /// <param name="path">The output file path.</param>
        /// <param name="Datas">The Datas.</param>
        public static void ExportFile(string path, Data[] Datas)
        {
            List<byte> DataBuffer = new List<byte>();
            FileStream writer = new FileStream(path, FileMode.Create);

            try
            {
                for (int i = 0; i < Datas.Length; i++)
                {
                    byte[] TempBuffer = Data.BuildData(Datas[i]);
                    for (int i2 = 0; i2 < TempBuffer.Length; i2++)
                    {
                        DataBuffer.Add(TempBuffer[i2]);
                    }
                    DataBuffer.Add(Data.Separator);
                }
                // Starting writing file.
                writer.WriteByte(Signature); // Write the signature at the 1st byte.
                for (int i = 0; i < DataBuffer.Count; i++)
                {
                    writer.WriteByte(DataBuffer[i]); // Then write the file.
                }
                writer.Dispose(); writer.Close();
            }

            catch (Exception e)
            {
                throw new NBIIOExeption("[FATAL] Failled to Export a file + (" + e.Message + ", " + e.Data + ")");

            }
        }
        /// <summary>
        /// This exception has is thrown when an I/O error occurs.
        /// </summary>
        [Serializable]
        public class NBIIOExeption : Exception
        {
            public NBIIOExeption() { }
            public NBIIOExeption(string message) : base(message) { }
            public NBIIOExeption(string message, Exception inner) : base(message, inner) { }
            protected NBIIOExeption(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
        /// <summary>
        /// Possibles extentions.
        /// </summary>
        public enum FileExt
        {
            DataFile,
            RawFile,
            Chunk,
            ChunkRoot
        }
    }
}