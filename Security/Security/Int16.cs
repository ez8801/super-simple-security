using System;

namespace Security
{
    public struct Int16 : IFormattable, IComparable, IComparable<Int16>, IComparable<short>, IEquatable<Int16>, IEquatable<short>
    {
        public const short MaxValue = short.MaxValue;
        public const short MinValue = short.MinValue;

#if UNITY_EDITOR
        short m_debugValue;
#endif

#if NO_SECURITY
        private short m_value;
#elif SIMPLE_SECURITY
        private Int16Chiper m_chiper;
#else
        private Int16Chiper m_chiper;
        private Int16Chiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private short GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            short value = m_chiper.GetValue();
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

        private void SetValue(short value)
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
            short v1 = m_chiper.GetValue();
            short v2 = m_chiperCmp.GetValue();
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

        public Int16(short value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper = new Int16Chiper(value);
#else
            m_chiper = new Int16Chiper(value);
            m_chiperCmp = new Int16Chiper(value);
#endif
        }

        public short Value { get { return GetValue(); } set { SetValue(value); } }

        #endregion Get / Set Secure Value

        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator short(Int16 sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator Int16(short value)
        {
            Int16 sValue = new Int16();
            sValue.SetValue(value);
            return sValue;
        }*/

        public static Int16 operator ++(Int16 sValue)
        {
            short value = sValue.GetValue();
            value++;
            sValue.SetValue(value);
            return sValue;
        }

        public static Int16 operator --(Int16 sValue)
        {
            short value = sValue.GetValue();
            value--;
            sValue.SetValue(value);
            return sValue;
        }

        #endregion Operator

        //---------------------------------------------------------------------
        //  Implement IComparable
        //---------------------------------------------------------------------
        #region Implement IComparable

        public int CompareTo(short value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(Int16 value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((Int16)value).GetValue());
        }

        #endregion Implement IComparable

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(short obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(Int16 obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((Int16)obj).GetValue());
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