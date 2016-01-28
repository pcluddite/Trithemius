using System;
using System.Collections;
using System.Collections.Generic;

namespace Trithemius
{
    public class BinaryOctet : IList<bool>, IComparable, IConvertible, IEquatable<BinaryOctet>, IEquatable<byte>, IComparable<BinaryOctet>, IComparable<byte>
    {
        private const int OCTET = 8;
        private bool[] bits = new bool[OCTET];

        public BinaryOctet()
        {
        }

        public BinaryOctet(byte value)
        {
            for (int index = 0; index < bits.Length; ++index) {
                bits[index] = (value & (1 << index)) != 0;
            }
        }

        public bool this[int index]
        {
            get {
                return bits[index];
            }
            set {
                bits[index] = value;
            }
        }

        public int Count
        {
            get {
                return bits.Length;
            }
        }

        public bool IsReadOnly
        {
            get {
                return false;
            }
        }

        public bool[] ToBoolArray()
        {
            return (bool[])bits.Clone();
        }

        void ICollection<bool>.Add(bool item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            for(int i = 0; i < bits.Length; ++i) {
                bits[i] = false;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            BinaryOctet octet = obj as BinaryOctet;
            if (octet != null)
                return CompareTo(octet);

            byte? @byte = obj as byte?;
            if (@byte != null)
                return CompareTo(@byte);

            throw new ArgumentException("Can only compare types BinaryOctet and byte");
        }

        public bool Contains(bool item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            bits.CopyTo(array, arrayIndex);
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return ((IEnumerable<bool>)bits).GetEnumerator();
        }

        public int IndexOf(bool item)
        {
            for(int i = 0; i < bits.Length; ++i) {
                if (bits[i] == item)
                    return i;
            }
            return -1; 
        }

        void IList<bool>.Insert(int index, bool item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<bool>.Remove(bool item)
        {
            throw new NotImplementedException();
        }

        void IList<bool>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return bits.GetEnumerator();
        }

        /// <summary>
        /// Sets each bit to the opposite of its current value
        /// </summary>
        public void Invert()
        {
            for (int index = 0; index < bits.Length; ++index) {
                bits[index] = !bits[index];
            }
        }

        public byte ToByte()
        {
            byte value = 0;

            for (byte index = 0; index < bits.Length; ++index) {
                if (bits[index]) {
                    value |= (byte)(1 << index);
                }
            }

            return value;
        }

        public bool Equals(BinaryOctet other)
        {
            for (int index = 0; index < bits.Length; ++index) {
                if (bits[index] != other.bits[index])
                    return false;
            }
            return true;
        }

        public bool Equals(byte other)
        {
            for(int index = 0; index < bits.Length; ++index) {
                if (bits[index] != ((other & (1 << index)) > 0))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            BinaryOctet octet = obj as BinaryOctet;
            if (octet != null) {
                return Equals(octet);
            }
            byte? byte_value = obj as byte?;
            if (byte_value != null) {
                return Equals(byte_value.Value);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return bits.GetHashCode();
        }

        public int CompareTo(BinaryOctet other)
        {
            return ToByte().CompareTo(other.ToByte());
        }

        public int CompareTo(byte other)
        {
            return ToByte().CompareTo(other);
        }

        #region implicit operators

        public static implicit operator BinaryOctet(byte value)
        {
            return new BinaryOctet(value);
        }

        public static implicit operator byte(BinaryOctet octet)
        {
            return octet.ToByte();
        }

        #endregion

        #region equality operators

        public static bool operator ==(BinaryOctet octet1, BinaryOctet octet2)
        {
            if ((object)octet1 == null || (object)octet2 == null)
                return object.Equals(octet1, octet2);

            return octet1.Equals(octet2);
        }

        public static bool operator !=(BinaryOctet octet1, BinaryOctet octet2)
        {
            if ((object)octet1 == null || (object)octet2 == null)
                return !object.Equals(octet1, octet2);

            return !octet1.Equals(octet2);
        }

        public static bool operator ==(BinaryOctet octet, byte @byte)
        {
            if (octet == null)
                return false;
            return octet.Equals(@byte);
        }

        public static bool operator !=(BinaryOctet octet, byte @byte)
        {
            if (octet == null)
                return true;
            return !octet.Equals(@byte);
        }

        public static bool operator ==(byte @byte, BinaryOctet octet)
        {
            if (octet == null)
                return false;
            return octet.Equals(@byte);
        }

        public static bool operator !=(byte @byte, BinaryOctet octet)
        {
            if (octet == null)
                return true;
            return !octet.Equals(@byte);
        }

        #endregion

        #region IConvertable

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(ToByte());
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(ToByte());
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(ToByte());
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(ToByte());
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(ToByte());
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(ToByte());
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(ToByte());
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(ToByte());
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(ToByte());
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(ToByte());
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(ToByte());
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return Convert.ToString(ToByte(), provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(ToByte(), conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(ToByte());
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(ToByte());
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(ToByte());
        }

        #endregion
    }
}
