
using System;

namespace Security
{
    public struct Int32 : IFormattable, IComparable, IComparable<Int32>, IComparable<int>, IEquatable<Int32>, IEquatable<int>
    {
        public const int MaxValue = int.MaxValue;
        public const int MinValue = int.MinValue;

#if UNITY_EDITOR
        int m_debugValue;
#endif

#if NO_SECURITY
        private int m_value;
#elif SIMPLE_SECURITY
        private Int32Chiper m_chiper;
#else
        private Int32Chiper m_chiper;
        private Int32Chiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private int GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            int value = m_chiper.GetValue();
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

        private void SetValue(int value)
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
            int v1 = m_chiper.GetValue();
            int v2 = m_chiperCmp.GetValue();
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

        public Int32(int value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper = new Int32Chiper(value);
#else
            m_chiper = new Int32Chiper(value);
            m_chiperCmp = new Int32Chiper(value);
#endif
        }

        public int Value { get { return GetValue(); } set { SetValue(value); } }

#if UNITY_EDITOR

        // (For Debugging) 에러유발: 호출하면 해킹탐지가 발동한다.
        public int ErrorValue
        {
            get
            {
#if SIMPLE_SECURITY
                m_chiper.SetValue(m_chiperCmp.GetValue() + 1);
#endif
                return GetValue();
            }

            set { SetValue(value); }
        }

#endif

        #endregion Get / Set Secure Value

        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator int(Int32 sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator Int32(int value)
        {
            Int32 sValue = new Int32();
            sValue.SetValue(value);
            return sValue;
        }*/

        public static Int32 operator ++(Int32 sValue)
        {
            int value = sValue.GetValue();
            value++;
            sValue.SetValue(value);
            return sValue;
        }

        public static Int32 operator --(Int32 sValue)
        {
            int value = sValue.GetValue();
            value--;
            sValue.SetValue(value);
            return sValue;
        }

        #endregion

        //---------------------------------------------------------------------
        //  Implement IComparable
        //---------------------------------------------------------------------
        #region Implement IComparable

        public int CompareTo(int value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(Int32 value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((Int32)value).GetValue());
        }

        #endregion

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(int obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(Int32 obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((Int32)obj).GetValue());
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