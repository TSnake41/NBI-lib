using System;
using System.Collections.Generic;
using System.Collections;
/*
 * [x] Simplify and optimise using BitConverter and add fonction for this.
 * [/] Optimise and clean the code.
 * [x] Do the short, int, long and their arrays.
 * [x] Do the float and the double.
 * [ ] Add other data types (color, vector etc.).
 */

namespace TSDFF
{
    /// <summary>
    /// Data objects and Data methods tools.
    /// </summary>
    public class Data
    {
        TypesList Format;
        object Value;
        string Name;

        public const byte Separator = 254;
        public const byte DataHead = 14; // Separate the Header (Name) and the Data.

        public Data(TypesList Format, string name, object value)
        {
            this.Format = Format;
            this.Name = name;
            this.Value = value;
        }
        #region Translate
        /// <summary>
        /// To convert binary data into non-binary data.
        /// </summary>
        /// <param name="UncompressedDataArray"></param>
        /// <returns></returns>
        public static Data Translate(byte[] UncompressedDataArray)
        {
            TypesList arrayformat = (TypesList)UncompressedDataArray[0]; // Get the TypesList with the 1st number.
            int CurretByte = 0;
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
                    Header += (char)UncompressedDataArray[CurretByte];
                }
                CurretByte++;
            }
            List<byte> templist = new List<byte>(8);

            List<byte> listbyte = new List<byte>();
            List<short> listshort = new List<short>();
            List<int> listint = new List<int>();
            List<long> listlong = new List<long>();

            for (; CurretByte < UncompressedDataArray.Length; CurretByte++)
            {
                switch (arrayformat)
                {
                    case TypesList.Byte:
                        value = BinaryConverterTool.GetValue(new byte[1] { UncompressedDataArray[CurretByte] }, TypesList.Byte);
                        throw null;
                    case TypesList.Short:
                        templist.Add(UncompressedDataArray[CurretByte]);
                        break;
                    case TypesList.Int:
                        templist.Add(UncompressedDataArray[CurretByte]);
                        break;
                    case TypesList.Long:
                        templist.Add(UncompressedDataArray[CurretByte]);
                        break;
                    case TypesList.Float:
                        templist.Add(UncompressedDataArray[CurretByte]);
                        break;
                    case TypesList.Double:
                        templist.Add(UncompressedDataArray[CurretByte]);
                        break;
                    case TypesList.String:
                        templist.Add(UncompressedDataArray[CurretByte]);
                        break;
                    case TypesList.Char:
                        value = BinaryConverterTool.GetValue(new byte[2] { UncompressedDataArray[CurretByte], UncompressedDataArray[CurretByte + 1] }, TypesList.Char);
                        break;
                    case TypesList.ByteA:
                        listbyte.Add(UncompressedDataArray[CurretByte]);
                        break;
                    case TypesList.ShortA:
                        if(templist.Count >= 2)
                        {
                            templist.Clear();
                            listshort.Add((short)BinaryConverterTool.GetValue(templist.ToArray(), TypesList.ShortA));
                        }
                        else
                        {
                            templist.Add(UncompressedDataArray[CurretByte]);
                        }
                        break;
                    case TypesList.IntA:
                        if (templist.Count >= 4)
                        {
                            templist.Clear();
                            listshort.Add((short)BinaryConverterTool.GetValue(templist.ToArray(), TypesList.ShortA));
                        }
                        else
                        {
                            templist.Add(UncompressedDataArray[CurretByte]);
                        }
                        break;
                    case TypesList.LongA:
                        if (templist.Count >= 8)
                        {
                            templist.Clear();
                            listshort.Add((short)BinaryConverterTool.GetValue(templist.ToArray(), TypesList.ShortA));
                        }
                        else
                        {
                            templist.Clear();
                            templist.Add(UncompressedDataArray[CurretByte]);
                        }
                        break;
                    case TypesList.CharA:
                           value = "" as string;
                           value += (char)UncompressedDataArray[CurretByte] + "" as string;
                        break;
                    case TypesList.Bool:
                        if (UncompressedDataArray[CurretByte] == 1)
                        {
                            value = true;
                        }
                        else
                        {
                            value = false;
                        }
                        break;
                }
            }
            switch (arrayformat)
            {
                case TypesList.Short:
                    value = (short)value;
                    value = BinaryConverterTool.GetValue(templist.ToArray(), TypesList.Short);
                    break;
                case TypesList.Int:
                    value = (int)value;
                    value = BinaryConverterTool.GetValue(templist.ToArray(), TypesList.Int);
                    break;
                case TypesList.Long:
                    value = (long)value;
                    value = BinaryConverterTool.GetValue(templist.ToArray(), TypesList.Long);
                    break;
                case TypesList.Float:
                    value = (float)value;
                    value = BinaryConverterTool.GetValue(templist.ToArray(), TypesList.Int);
                    break;
                case TypesList.Double:
                    value = (double)value;
                    value = BinaryConverterTool.GetValue(templist.ToArray(), TypesList.Long);
                    break;
                case TypesList.String:
                    value = ((string)value) as string;
                    value = (string)BinaryConverterTool.GetValue(templist.ToArray(), TypesList.String);
                    break;
                case TypesList.Char:
                    value = (char)value;
                    break;
                case TypesList.ByteA:
                    value = ((byte[])value) as byte[];
                    value = listbyte.ToArray();
                    break;
                case TypesList.ShortA:
                    value = ((short[])value) as short[];
                    value = listshort.ToArray();
                    break;
                case TypesList.IntA: 
                    value = ((int[])value) as int[];
                    value = listint.ToArray();
                    break;
                case TypesList.LongA:
                    value = ((long[])value) as long[];
                    value = listlong.ToArray();
                    break;
                case TypesList.CharA:
                    value = ((char[])value) as char[];
                    char[] _charvalue = (value as string).ToCharArray();
                    break;
                case TypesList.Bool:
                    value = (bool)value;
                    break;
                default:
                    break;
            }
            return new Data(arrayformat, Header, value);
        }
        #endregion
        #region DataBuilding
        /// <summary>
        /// To build binary data to be inserted into a data file.
        /// </summary>
        /// <param name="data">The data used to create the binary table.</param>
        /// <returns></returns>
        public static byte[] BuildData(Data data)
        {
            //        byte[] ByteBuffer = new byte[64000];
            List<byte> ByteBuffer = new List<byte>();

            string name = data.Name;
            TypesList format = data.Format;
            object value = data.Value;

            for (int i = 0; i < name.Length; i++)
            {
                ByteBuffer.Add((byte)name[i]);
            } // Set the name

            ByteBuffer.Add((byte)format); // Add the format.
            ByteBuffer.Add((byte)DataHead); // Add the head.
            switch (format)
            {
                case TypesList.Byte:
                    byte nbyte = (byte)Convert.ChangeType(value, typeof(byte));

                    ByteBuffer.Add(nbyte);
                    break;
                case TypesList.Short:
                    byte[] shortarray = BinaryConverterTool.ConvertToByte((short)Convert.ChangeType(value, typeof(short)));
                    for (int i = 0; i < 2; i++)
                    {
                        ByteBuffer.Add(shortarray[i]);
                    }
                    break;
                case TypesList.Int:
                    byte[] intarray = BinaryConverterTool.ConvertToByte((int)Convert.ChangeType(value, typeof(int)));
                    for (int i = 0; i < 4; i++)
                    {
                        ByteBuffer.Add(intarray[i]);
                    }
                    break;
                case TypesList.Long:
                    byte[] longarray = BinaryConverterTool.ConvertToByte((long)Convert.ChangeType(value, typeof(long)));
                    for (int i = 0; i < 8; i++)
                    {
                        ByteBuffer.Add(longarray[i]);
                    }
                    break;
                case TypesList.String:
                    string _value = value as string;
                    for (int i = 0; i < _value.Length; i++)
                    {
                        ByteBuffer.Add((byte)_value[i]);
                    }
                    break;
                case TypesList.Char:
                    ByteBuffer.Add((byte)(char)value);
                    break;
                case TypesList.ByteA:
                    byte[] _ByteA = value as byte[];
                    for (int i = 0; i < _ByteA.Length; i++)
                    {
                        ByteBuffer.Add((byte)_ByteA[i]);
                    }
                    break;
                 case TypesList.ShortA:
                    short[] shorta = value as short[];
                    for (int i = 0; i < shorta.Length; i++)
                    {
                        byte[] shorta2 = BinaryConverterTool.ConvertToByte((short)Convert.ChangeType(shorta[i], typeof(short)));
                        for (int i2 = 0; i2 < 2; i2++)
                        {
                            ByteBuffer.Add(shorta2[i2]);
                        }
                    }
                    break;
                case TypesList.IntA:
                    int[] inta = value as int[];
                    for (int i = 0; i < inta.Length; i++)
                    {
                        byte[] inta2 = BinaryConverterTool.ConvertToByte((int)Convert.ChangeType(inta[i], typeof(int)));
                        for (int i2 = 0; i2 < 2; i2++)
                        {
                            ByteBuffer.Add(inta2[i2]);
                        }
                    }
                    break;
                case TypesList.LongA:
                     int[] longa = value as int[];
                    for (int i = 0; i < longa.Length; i++)
                    {
                        byte[] longa2 = BinaryConverterTool.ConvertToByte((int)Convert.ChangeType(longa[i], typeof(int)));
                        for (int i2 = 0; i2 < 2; i2++)
                        {
                            ByteBuffer.Add(longa2[i2]);
                        }
                    }
                case TypesList.CharA:
                    char[] _charA = value as char[];
                    for (int i = 0; i < _charA.Length; i++)
                    {
                        ByteBuffer.Add((byte)_charA[i]);
                    }
                    break;
                case TypesList.Float:
                     byte[] FloatByteArray = BinaryConverterTool.ConvertToByte((float)Convert.ChangeType(value, typeof(float)));
                    for (int i = 0; i < 4; i++)
                    {
                        ByteBuffer.Add(FloatByteArray[i]);
                    }
                    break;
                case TypesList.Double:
                    byte[] DoubleByteArray = BinaryConverterTool.ConvertToByte((double)Convert.ChangeType(value, typeof(double)));
                    for (int i = 0; i < 4; i++)
                    {
                        ByteBuffer.Add(DoubleByteArray[i]);
                    }
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            return (ByteBuffer.ToArray());
        }
        #endregion
    }
  /*  public enum TypesList
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
        LongA = 10,
        CharA = 11,
        Float = 12,
        Double = 13,
        Bool = 14,
        Null
    }*/
}
