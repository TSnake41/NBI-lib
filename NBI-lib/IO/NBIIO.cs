using System;
using System.IO;

namespace TSDFF.IO
{
    public class NBIIO
    {
        const byte Signature = 1; // The curret signature.
        byte[] ReadFile(string path)
        {
            return (File.ReadAllBytes(path));
        }
        /// <summary>
        /// Permet de lire un fichier et d'y récuperer ses données.
        /// </summary>
        /// <param name="path">Le format de fichier, peut être .NBI, .nbir, .chki ou .nbr</param>
        public static Data[] ReadFile(string path)
        {
            FileInfo info = new FileInfo(path);
            FileExt Ext;

            switch (info.Extension) // Verifie et place l'extention du fichier.
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
                    return TranslateDataFile(File.ReadAllBytes(path), true); // Utilise TranslateDataFile avec la vérification de signature.
                case FileExt.RawFile:
                    return new Data[] { Data.Translate(File.ReadAllBytes(path)) }; // Traduit la seule donnée presente.
                case FileExt.Chunk:
                    return TranslateDataFile(File.ReadAllBytes(path), false); // Utilise TranslateDataFile sans la vérification de signature.
                case FileExt.ChunkRoot:
                default:
                    throw new InvalidOperationException("[FATAL] Invalid or null format forcing.");
            }


        }


        /// <summary>
        /// Permet de Traduire un fichier de données en un Tableau de Données.
        /// </summary>
        /// <param name="filedata">Tableau d'octets du fichier ou de l'instance.</param>
        /// <returns>Tableau de données contenant toute les valeurs du fichier.</returns>
        static Data[] TranslateDataFile(byte[] FileData, bool verifySignature)
        {
            int CurretByte = 0; // La position de la bande passante.
            Data[] _Data = new Data[] { }; // Notre Data qui nous sert de buffer
            byte[] ByteBuffer = new byte[] { }; // Notre buffer a octect pour extraire les valeurs.
            if (verifySignature)
            {
                CurretByte++;
            }
            if (FileData[0] == Signature || !verifySignature)
            {
                for (; CurretByte < FileData.Length; CurretByte++)
                {
                    ByteBuffer[ByteBuffer.Length + 1] = FileData[CurretByte];
                    if (FileData[CurretByte] == Data.Separator) // Si notre octet est un separateur.
                    {
                        _Data[_Data.Length + 1] = Data.Translate(ByteBuffer); // On défini la nouvelle Data.
                        ByteBuffer = new byte[] { }; // On créer un nouveau ByteBuffer.
                    }
                }
            }
            else
            {
                throw new InvalidDataException("Invalid file signature. (fr: Signature de fichier invalide.)");
            }
            return _Data;
        }
    }
    public enum FileExt
    {
        DataFile,
        RawFile,
        Chunk,
        ChunkRoot
    }
}