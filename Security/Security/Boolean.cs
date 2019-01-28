using System;

namespace Security
{
    public struct Boolean : IComparable, IComparable<Boolean>, IComparable<bool>, IEquatable<Boolean>, IEquatable<bool>
    {
        public static readonly string FalseString   = bool.FalseString;
        public static readonly string TrueString    = bool.TrueString;

#if UNITY_EDITOR
        bool m_debugValue;
#endif

#if NO_SECURITY
        private bool m_value;
#elif SIMPLE_SECURITY
        private BooleanChiper m_chiper;
#else
        private BooleanChiper m_chiper;
        private BooleanChiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private bool GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            bool value = m_chiper.GetValue();
            
            // Self error check
            if (value != m_debugValue)
            {
                string message = string.Format("[{0}] Dec({1}) != DebugV({2})"
                    , GetType().ToString()
                    , value, m_debugValue);
                SecurityListener.OnError(message);
            }
            return value;
#else
            return m_chiper.GetValue();
#endif
        }

        private void SetValue(bool value)
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
            bool v1 = m_chiper.GetValue();
            bool v2 = m_chiperCmp.GetValue();
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

        public Boolean(bool value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper = new BooleanChiper(value);
#else
            m_chiper = new BooleanChiper(value);
            m_chiperCmp = new BooleanChiper(value);
#endif
        }

        public bool Value { get { return GetValue(); } set { SetValue(value); } }

        #endregion Get / Set Secure Value
        
        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator bool(Boolean sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator KBool(bool value)
        {
            Boolean sValue = new Boolean();
            sValue.SetValue(value);
            return sValue;
        }*/

        #endregion Operator

        //---------------------------------------------------------------------
        //  Implement IComparable
        //---------------------------------------------------------------------
        #region Implement IComparable

        public int CompareTo(bool value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(Boolean value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((Boolean)value).GetValue());
        }

        #endregion Implement IComparable

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(bool obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(Boolean obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((Boolean)obj).GetValue());
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

        #endregion Implement IFormattable
    }
}