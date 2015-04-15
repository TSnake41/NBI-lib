using System;
/*
 * [ ] Simplify and optimise using BitConverter and add fonction for this.
 * [ ] Optimise and clean the code.
 * [ ] Do the short, int, long and their arrays.
 * [ ] Do the float and the double.
 * [ ] Add other data types (color, vector etc.).
 */

namespace TSDFF
{
    public class Data
    {
        DataFormat Format;
        object Value;
        string Name;

        public const byte Separator = 254;
        public const byte DataHead = 14; // Separate the Header (Name) and the Data.

        public Data(DataFormat Format, object value = null)
        {
            this.Format = Format;
            this.Value = value;
        }
        #region Translate
        /// <summary>
        /// Permet de convertir des données binaire en données.
        /// </summary>
        /// <param name="UncompressedDataArray"></param>
        /// <returns></returns>
        public static Data Translate(byte[] UncompressedDataArray)
        {
            DataFormat arrayformat = (DataFormat)UncompressedDataArray[0]; // Get the DataFormat with the 1st number.
            int CurretByte = 1;
            string Header = "";
            object value = new object();

            while (true) // Read and Set the name of the Data.
            {
                if (UncompressedDataArray[CurretByte] == 14)
                {
                    CurretByte++;
                    break;
                }
                else
                {
                    Header += UncompressedDataArray[CurretByte];
                }
                CurretByte++;
            }
            for (;CurretByte < UncompressedDataArray.Length; CurretByte++)
            {
                switch (arrayformat)
                {
                    case DataFormat.Byte:
                        value = UncompressedDataArray[CurretByte];
                        throw null;
                    case DataFormat.Short:
                        throw new NotImplementedException("Short isn't implemented on this version.");
                    case DataFormat.Int:
                        throw new NotImplementedException("Int isn't implemented on this version.");
                    case DataFormat.Long:
                        throw new NotImplementedException("Long isn't implemented on this version.");
                        break;
                    case DataFormat.String:
                        //    value = "" as string;
                        value += (char)UncompressedDataArray[CurretByte] + "" as string;
                        break;
                    case DataFormat.Char:
                        value = (char)UncompressedDataArray[CurretByte];
                        break;
                    case DataFormat.ByteA:
                        //  value = new byte[] {} as byte[];
                        //  value += (byte)UncompressedDataArray[CurretByte] + "" as byte[];
                        break;
                    case DataFormat.ShortA:
                        break;
                    case DataFormat.IntA:
                        break;
                    case DataFormat.StringA:
                        break;
                    case DataFormat.LongA:
                        break;
                    case DataFormat.CharA:
                        //   value = "" as string;
                        //   value += (char)UncompressedDataArray[CurretByte] + "" as string;
                        throw new NotImplementedException();
                        break;
                    case DataFormat.Null:
                        break;
                }
                CurretByte++;
            }
            return new Data(arrayformat, value);
        }
        #endregion
        #region DataBuilding
        /// <summary>
        /// Permet de construire des données binaire a inserer dans un fichier de données.
        /// </summary>
        /// <param name="data">La donnée utilisé pour créer le tableau binaire.</param>
        /// <returns></returns>
        public static byte[] BuildData(Data data)
        {
            byte[] ByteBuffer = new byte[] {};
            string name = data.Name;
            DataFormat format = data.Format;
            object value = data.Value;

            for (int i = 0; i < name.Length; i++)
            {
                ByteBuffer[i] = (byte)name[i];
            } // Set the name

            ByteBuffer[ByteBuffer.Length + 1] = Data.DataHead;

            switch (format)
            {
                case DataFormat.Byte:
                    ByteBuffer[ByteBuffer.Length] = (byte)value;
                    break;
                case DataFormat.Short: 
                    break;
                case DataFormat.Int:
                    break;
                case DataFormat.Long:
                    break;
                case DataFormat.String:
                    string _value = value as string;
                    for (int i = 0; i < _value.Length; i++)
                    {
                        ByteBuffer[ByteBuffer.Length + 1] = (byte)_value[i];
                    }
                    break;
                case DataFormat.Char:
                    ByteBuffer[ByteBuffer.Length + 1] = (byte)(char)value;
                    break;
                case DataFormat.ByteA:
                    byte[] _ByteA = value as byte[];
                    for (int i = 0; i < _ByteA.Length; i++)
                    {
                        ByteBuffer[ByteBuffer.Length + 1] = _ByteA[i];
                    }
                    break;
                case DataFormat.ShortA:
                    break;
                case DataFormat.IntA:
                    break;
                case DataFormat.StringA:
                    break;
                case DataFormat.LongA:
                    break;
                case DataFormat.CharA:
                    char[] _charA = value as char[];
                    for (int i = 0; i < _charA.Length; i++)
                    {
                        ByteBuffer[ByteBuffer.Length + 1] = (byte)_charA[i];
                    }
                    break;
                case DataFormat.Null:
                    throw null;
                default:
                    break;
            }
            return ByteBuffer;
        }
        #endregion
    }
    public enum DataFormat
    {
        Byte = 1, 
        Short = 2, 
        Int = 3,
        Long = 4,
        String = 5,
        Char = 6,
        ByteA = 7,
        ShortA = 8,
        IntA = 9,
        StringA = 10,
        LongA = 11,
        CharA = 12,
        Null = 13
    }
}
