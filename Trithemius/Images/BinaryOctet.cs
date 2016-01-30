using System;
using System.Collections;
using System.Collections.Generic;

namespace Trithemius
{
    public struct BinaryOctet : IList<bool>, IComparable, IConvertible, IEquatable<BinaryOctet>, IEquatable<byte>, IComparable<BinaryOctet>, IComparable<byte>
    {
        private const int OCTET = 8;
        private byte bvalue;
        
        public BinaryOctet(byte value)
        {
            bvalue = value;
        }

        public BinaryOctet(bool[] bits)
        {
            if (bits.Length > OCTET)
                throw new ArgumentException("cannot have more than 8 bits in an octet", nameof(bits));
            bvalue = 0;
            for(int index = 0; index < bits.Length; ++index) {
                bvalue = SetBit(bvalue, index, bits[index]);
            }
        }

        public BinaryOctet SetBit(int index, bool value)
        {
            return new BinaryOctet(SetBit(bvalue, index, value));
        }

        private static byte SetBit(byte bvalue, int index, bool value)
        {
            if (index < 0 || index >= OCTET)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (value) {
                bvalue = (byte)(bvalue | (1 << index));
            }
            else {
                bvalue = (byte)(bvalue & ~(1 << index));
            }
            return bvalue;
        }

        public bool GetBit(int index)
        {
            return GetBit(bvalue, index);
        }

        private static bool GetBit(byte bvalue, int index)
        {
            if (index < 0 || index >= OCTET)
                throw new ArgumentOutOfRangeException(nameof(index));
            return (bvalue & (1 << index)) != 0;
        }

        public bool this[int index]
        {
            get {
                return GetBit(index);
            }
        }

        public bool[] ToBoolArray()
        {
            bool[] bits = new bool[OCTET];
            for (int index = 0; index < bits.Length; ++index) {
                bits[index] = GetBit(index);
            }
            return bits;
        }

        /// <summary>
        /// Sets all bits to 0
        /// </summary>
        public void Clear()
        {
            bvalue = 0;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            BinaryOctet? octet = obj as BinaryOctet?;
            if (octet != null)
                return CompareTo(octet.Value);

            byte? @byte = obj as byte?;
            if (@byte != null)
                return CompareTo(@byte.Value);

            throw new ArgumentException("Can only compare types BinaryOctet and byte");
        }

        public bool Contains(bool item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            ToBoolArray().CopyTo(array, arrayIndex);
        }

        public IEnumerator<bool> GetEnumerator()
        {
            foreach (bool bit in ToBoolArray())
                yield return bit;
        }

        public int IndexOf(bool item)
        {
            for(int i = 0; i < OCTET; ++i) {
                if (this[i] == item)
                    return i;
            }
            return -1;
        }

        void ICollection<bool>.Add(bool item)
        {
            throw new NotImplementedException();
        }

        int ICollection<bool>.Count
        {
            get {
                return OCTET;
            }
        }

        bool ICollection<bool>.IsReadOnly
        {
            get {
                return false;
            }
        }

        bool IList<bool>.this[int index]
        {
            get {
                return GetBit(index);
            }

            set {
                throw new NotImplementedException();
            }
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
            return ToBoolArray().GetEnumerator();
        }

        /// <summary>
        /// Sets each bit to the opposite of its current value
        /// </summary>
        public BinaryOctet Invert()
        {
            byte newval = bvalue;
            for(int index = 0; index < OCTET; ++index) {
                newval = SetBit(newval, index, !GetBit(newval, index));
            }
            return new BinaryOctet(newval);
        }

        public byte ToByte()
        {
            return bvalue;
        }

        public bool Equals(BinaryOctet other)
        {
            return bvalue == other.bvalue;
        }

        public bool Equals(byte other)
        {
            return bvalue == other;
        }

        public override bool Equals(object obj)
        {
            BinaryOctet? octet = obj as BinaryOctet?;
            if (octet != null) {
                return Equals(octet.Value);
            }
            byte? byte_value = obj as byte?;
            if (byte_value != null) {
                return Equals(byte_value.Value);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return bvalue.GetHashCode();
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

        int IList<bool>.IndexOf(bool item)
        {
            throw new NotImplementedException();
        }

        void ICollection<bool>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<bool>.Contains(bool item)
        {
            throw new NotImplementedException();
        }

        void ICollection<bool>.CopyTo(bool[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator<bool> IEnumerable<bool>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
