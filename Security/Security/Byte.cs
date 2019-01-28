using System;

namespace Security
{
    public struct Byte : IFormattable, IComparable, IComparable<Byte>, IComparable<byte>, IEquatable<Byte>, IEquatable<byte>
    {
        public const byte MaxValue = byte.MaxValue;
        public const byte MinValue = byte.MinValue;

#if UNITY_EDITOR
        byte m_debugValue;
#endif

#if NO_SECURITY
        private byte m_value;
#elif SIMPLE_SECURITY
        private ByteChiper m_chiper;
#else
        private ByteChiper m_chiper;
        private ByteChiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private byte GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            byte value = m_chiper.GetValue();
            // Self error check
            if (value != m_debugValue)
            {
                string message = string.Format("[{0}] Dec({1}) != DebugV({2})"
                    , GetType().ToString()
                    , value
                    , m_debugValue);
                SecurityListener.OnError(message);
            }
            return value;
#else
            return m_chiper.GetValue();
#endif
        }

        private void SetValue(byte value)
        {
#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper.SetValue(value);
            
            // Save debug value
    #if UNITY_EDITOR
            m_debugValue = value;
    #endif
#else
            byte v1 = m_chiper.GetValue();
            byte v2 = m_chiperCmp.GetValue();
            if (v1 != v2)
            {
                // Detected hacking
                string message = string.Format("[{0}] v1({1}) != v2({2})"
                    , GetType().ToString()
                    , v1
                    , v2);
                SecurityListener.OnHackDetect(message);
            }
            m_chiper.SetValue(value);
            m_chiperCmp.SetValue(value);

            // Save debug value
    #if UNITY_EDITOR
            m_debugValue = value;
    #endif
#endif
        }

        public Byte(byte value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper = new ByteChiper(value);
#else
            m_chiper = new ByteChiper(value);
            m_chiperCmp = new ByteChiper(value);
#endif
        }

        public byte Value { get { return GetValue(); } set { SetValue(value); } }

        #endregion Get / Set Secure Value

        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator byte(Byte sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator Byte(byte value)
        {
            Byte sValue = new Byte();
            sValue.SetValue(value);
            return sValue;
        }*/

        public static Byte operator ++(Byte sValue)
        {
            byte value = sValue.GetValue();
            value++;
            sValue.SetValue(value);
            return sValue;
        }

        public static Byte operator --(Byte sValue)
        {
            byte value = sValue.GetValue();
            value--;
            sValue.SetValue(value);
            return sValue;
        }

        #endregion Operator

        //---------------------------------------------------------------------
        //  Implement IComparable
        //---------------------------------------------------------------------
        #region Implement IComparable

        public int CompareTo(byte value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(Byte value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((Byte)value).GetValue());
        }

        #endregion Implement IComparable

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(byte obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(Byte obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((Byte)obj).GetValue());
        }

        public override int GetHashCode()
        {
            return GetValue().GetHashCode();
        }

        #endregion Implement IEquatable

        //---------------------------------------------------------------------
        //  Implement IFormattable
        //---------------------------------------------------------------------
        #region Implement IFormattable

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public string ToString(IFormatProvider provider)
        {
            return GetValue().ToString(provider);
        }

        public string ToString(string format)
        {
            return GetValue().ToString(format);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return GetValue().ToString(format, provider);
        }

        #endregion Implement IFormattable
    }
}