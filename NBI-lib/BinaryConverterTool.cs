using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSDFF
{
    public abstract class BinaryConverterTool
    {
        /// <summary>
        /// Permet de convertir une valeur pour obtenir un byte[] qui correspond a celui-ci.
        /// </summary>
        /// <param name="value">Valeur a convertir. (valeurs supportées: bool, byte, short, int, long, float, double, char (UTF-8))</param>
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
        /// Permet de récuperer la valeur d'un tableau de byte.
        /// </summary>
        public static object GetValue(byte[] number, TypesList type) // TODO: finish this
        {
            try
            {
                switch (type)
                {
                    case TypesList.Bool:
                        return BitConverter.ToBoolean(number , 0);
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
                    case TypesList.BoolA:
                        return null;
                    case TypesList.ByteA:
                        return null;
                    case TypesList.ShortA:
                        return null;
                    case TypesList.IntA:
                        return null;
                    case TypesList.LongA:
                        return null;
                    case TypesList.FloatA:
                        return null;
                    case TypesList.DoubleA:
                        return null;
                    case TypesList.String:
                        return null;
                    case TypesList.Sbyte:
                        return (sbyte)(number[0] + 128);
                    case TypesList.uShort:
                        return BitConverter.ToUInt16(number, 0);
                    case TypesList.uInt:
                        return BitConverter.ToUInt32(number, 0);
                    case TypesList.uLong:
                        return BitConverter.ToUInt64(number, 0);
                    default:
                        throw new BinaryConverterError("Une erreur est survenue lors de la conversion d'un tableau binaire en une valeur : (" + e.Data + ")");
                }
            }
            catch (Exception e)
            {
                throw new BinaryConverterError("Une erreur est survenue lors de la conversion d'un tableau binaire en une valeur : (" + e.Data + ")");
            }
        }
        enum TypesList
        {
            Bool, Byte, Short, Int, Long, Float, Double, Char,
            BoolA, ByteA, ShortA, IntA, LongA, FloatA, DoubleA, String,
            Sbyte, uShort, uInt, uLong
        }
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

}
