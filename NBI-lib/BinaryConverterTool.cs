using System;

namespace TSDFF
{
    /// <summary>
    /// A lot of tools for convert bytes to data and vice versa.
    /// </summary>
    /// <see cref="Data.cs"/>
    public static class BinaryConverterTool
    {
        /// <summary>
        /// To convert a value to get a byte [] that corresponds to.
        /// </summary>
        /// <param name="value">Value a convert. (supported values: bool, byte, short, int, long, float, double, char, and all the other in TypeList)</param>
        public static byte[] ConvertToByte(object value)
        {
            if (value is bool)
                return (BitConverter.GetBytes((bool)value)); // Retourne 1 byte.

            else if (value is byte)
                return (BitConverter.GetBytes((byte)value)); // Retourne 1 byte.

            else if (value is short)
                return (BitConverter.GetBytes((short)value)); // Retourne 2 bytes.

            else if (value is int)
                return (BitConverter.GetBytes((int)value)); // Retourne 4 bytes.

            else if (value is long)
                return (BitConverter.GetBytes((long)value)); // Retourne 8 bytes.

            else if (value is float)
                return (BitConverter.GetBytes((float)value)); // Retourne 4 bytes.

            else if (value is double)
                return (BitConverter.GetBytes((double)value)); // Retourne 8 bytes.

            else if (value is char) // Char format: Unicode
                return (BitConverter.GetBytes((char)value)); // Retourne 2 bytes.
            else
                throw new InvalidOperationException("Format de valeur inconnu ou incompatible");
        }
        /// <summary>
        /// To get the value of a byte array.
        /// </summary>
        public static object GetValue(byte[] number, TypesList type) // TODO: finish this
        {
            try
            {
                switch (type)
                {
                    case TypesList.Bool:
                        return BitConverter.ToBoolean(number, 0);
                    case TypesList.Byte:
                        return number[0];
                    case TypesList.Short:
                        return BitConverter.ToInt16(number, 0);
                    case TypesList.Int:
                        return BitConverter.ToInt32(number, 0);
                    case TypesList.Long:
                        return BitConverter.ToInt64(number, 0);
                    case TypesList.Float:
                        return BitConverter.ToSingle(number, 0);
                    case TypesList.Double:
                        return BitConverter.ToDouble(number, 0);
                    case TypesList.Char:
                        return BitConverter.ToChar(number, 0);
                    /*   
                     * case TypesList.FloatA:
                           return null;
                       case TypesList.DoubleA:
                           return null;
                     */
                    case TypesList.String:
                        return null;
                    default:
                        throw new InvalidOperationException("Invalid or unsupported conversion.");
                }
            }
            catch (Exception e)
            {
                throw new BinaryConverterError("An error occurred when converting a bit array in a value : (" + e.Data + ")");
            }
        }
        /// <summary>
        /// That exception was thrown when an conversion error occcurs.
        /// </summary>
        [Serializable]
        public class BinaryConverterError : Exception
        {
            public BinaryConverterError() { }
            public BinaryConverterError(string message) : base(message) { }
            public BinaryConverterError(string message, Exception inner) : base(message, inner) { }
            protected BinaryConverterError(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
    public enum TypesList
    {
        Bool = 1, Byte = 2, Short = 3, Int=4, Long=5, Float=6, Double=7, Char=8,
        BoolA=9, ByteA=10, ShortA=11, IntA=12, LongA=13, CharA=14, /*FloatA, DoubleA,*/ String=15
        //    Sbyte, uShort, uInt, uLong
    }
}
