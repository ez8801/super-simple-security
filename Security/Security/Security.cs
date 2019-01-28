// Shift the storage when get the value
//#define SHIFT_WHEN_GET

// Use faster encryption
#define FAST_FLOAT

using System;

namespace Security
{
    public class SecurityListener
    {
        static Action<string> m_onHackDetectListener;
        static Action<string> m_onErrorListener;
        
        static public void SetOnHackDetectListener(Action<string> l)
        {
            m_onHackDetectListener = l;
        }

        static internal void OnHackDetect(string message)
        {
            Console.WriteLine(string.Format("Security detected hack! message={0}", message));
            if (m_onHackDetectListener != null)
                m_onHackDetectListener.Invoke(message);
        }

        static public void SetOnErrorlistener(Action<string> l)
        {
            m_onErrorListener = l;
        }

        static internal void OnError(string message)
        {
            Console.WriteLine(string.Format("Security has error! message={0}", message));
            if (m_onErrorListener != null)
                m_onErrorListener.Invoke(message);
        }
    }

    internal class KeyGenerator
    {
        public static readonly int ValueArrayLength = 3;

        static byte m_byteXorKey;
        static short m_int16XorKey;
        static int m_int32XorKey;
        static long m_int64XorKey;
        static ushort m_uint16XorKey;
        static uint m_uint32XorKey;
        static ulong m_uint64XorKey;

        static Random m_random = new Random();
        
        static private int RandomNext(int minValue, int maxValue)
        {
            return m_random.Next(minValue, maxValue);
        }

        static public int NewIndex(int index)
        {
            if (m_byteXorKey > 128)
            {
                index++;
                if (index >= ValueArrayLength)
                    index = 0;
            }
            else
            {
                index--;
                if (index < 0)
                    index = (ValueArrayLength - 1);
            }
            return index;
        }

        static public byte GetByteXorKey()
        {
            if (m_byteXorKey == 0)
            {
                m_byteXorKey = (byte)RandomNext(0, 0xFF);
            }
            m_byteXorKey++;
            return m_byteXorKey;
        }

        static public short GetInt16XorKey()
        {
            if (m_int16XorKey == 0)
            {
                m_int16XorKey = (short)RandomNext(0, 0xFFFF);
            }
            m_int16XorKey += m_byteXorKey;
            return m_int16XorKey;
        }

        static public int GetInt32XorKey()
        {
            if (m_int32XorKey == 0)
            {
                m_int32XorKey = RandomNext(0, 0xFFFF);
            }
            m_int32XorKey += m_int16XorKey;
            return m_int32XorKey;
        }

        static public long GetInt64XorKey()
        {
            if (m_int64XorKey == 0)
            {
                m_int64XorKey = RandomNext(0, 0xFFFF);
            }
            m_int64XorKey += m_int32XorKey;
            return m_int64XorKey;
        }

        static public ushort GetUInt16XorKey()
        {
            if (m_uint16XorKey == 0)
            {
                m_uint16XorKey = (ushort)RandomNext(0, 0xFFFF);
            }
            m_uint16XorKey += m_byteXorKey;
            return m_uint16XorKey;
        }

        static public uint GetUInt32XorKey()
        {
            if (m_uint32XorKey == 0)
            {
                m_uint32XorKey = (uint)RandomNext(0, 0xFFFF);
            }
            m_uint32XorKey += m_byteXorKey;
            return m_uint32XorKey;
        }
        
        static public ulong GetUInt64XorKey()
        {
            if (m_uint64XorKey == 0)
            {
                m_uint64XorKey = (ulong)RandomNext(0, 0xFFFF);
            }
            m_uint64XorKey += m_uint16XorKey;
            return m_uint64XorKey;
        }
    }

    internal struct BooleanChiper
    {
        private byte m_xorKey;
        private int m_index;
        private ValueArray<byte> m_values;

        public BooleanChiper(bool value)
        {
            m_xorKey = 0;
            m_index = 0;
            m_values = new ValueArray<byte>();
            SetValue(value);
        }

        public bool GetValue()
        {
            byte byteValue = m_values[m_index];
            byteValue ^= m_xorKey;

            // Even: false, Odd: true
            bool value = (byteValue % 2 == 1);

#if SHIFT_WHEN_GET
            SetValue(value);
#endif
            return value;
        }

        public void SetValue(bool value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetByteXorKey();
            byte byteValue = value ? (byte)13 : (byte)20;
            byteValue ^= m_xorKey;
            m_values[m_index] = byteValue;
        }
    }

    internal struct ByteChiper
    {
        private byte m_xorKey;
        private int m_index;
        private ValueArray<byte> m_values;

        public ByteChiper(byte value)
        {
            m_xorKey = 0;
            m_index = 0;
            m_values = new ValueArray<byte>();
            SetValue(value);
        }

        public byte GetValue()
        {
            byte value = m_values[m_index];
            value ^= m_xorKey;

#if SHIFT_WHEN_GET
            SetValue(value);
#endif
            return value;
        }

        public void SetValue(byte value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetByteXorKey();
            value ^= m_xorKey;
            m_values[m_index] = value;
        }
    }

    internal struct Int16Chiper
    {
        private short m_xorKey;
        private int m_index;
        private ValueArray<short> m_values;

        public Int16Chiper(short value)
        {
            m_xorKey = 0;
            m_index = 0;
            m_values = new ValueArray<short>();
            SetValue(value);
        }

        public short GetValue()
        {
            short value = m_values[m_index];
            value ^= m_xorKey;

#if SHIFT_WHEN_GET
            SetValue(value);
#endif
            return value;
        }

        public void SetValue(short value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetInt16XorKey();
            value ^= m_xorKey;
            m_values[m_index] = value;
        }
    }


    internal struct Int32Chiper
    {
        private int m_xorKey;
        private int m_index;
        private ValueArray<int> m_values;

        public Int32Chiper(int value)
        {
            m_xorKey = 0;
            m_index = 0;
            m_values = new ValueArray<int>();
            SetValue(value);
        }

        public int GetValue()
        {
            int value = m_values[m_index] ^ m_xorKey;

#if SHIFT_WHEN_GET
            SetValue(value);
#endif
            return value;
        }

        public void SetValue(int value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetInt32XorKey();
            m_values[m_index] = value ^ m_xorKey;
        }
    }

    internal struct Int64Chiper
    {
        private long m_xorKey;
        private int m_index;
        private ValueArray<long> m_values;

        public Int64Chiper(long value)
        {
            m_xorKey = 0;
            m_index = 0;
            m_values = new ValueArray<long>();
            SetValue(value);
        }

        public long GetValue()
        {
            long value = m_values[m_index];
            value ^= m_xorKey;

#if SHIFT_WHEN_GET
            SetValue(value);
#endif

            return value;
        }

        public void SetValue(long value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetInt64XorKey();
            value ^= m_xorKey;
            m_values[m_index] = value;
        }
    }

    internal struct SingleChiper
    {
        private byte m_xorKey;
        private int m_index;

#if FAST_FLOAT
        private static byte[] s_decBytesBuff;
        private ValueArray<int> m_values;
#else
        private ValueArray<uint> m_values;
#endif

        public SingleChiper(float value)
        {
            m_xorKey = 0;
            m_index = 0;

#if FAST_FLOAT
            m_values = new ValueArray<int>();
#else
            m_values = new ValueArray<uint>();
#endif

            SetValue(value);
        }

        public float GetValue()
        {

#if FAST_FLOAT
            int v = m_values[m_index];

            if (s_decBytesBuff == null)
                s_decBytesBuff = new byte[4];
            byte[] bytes = s_decBytesBuff;

            bytes[0] = (byte)(v & 0x000000FF);
            bytes[1] = (byte)((v & 0x0000FF00) >> 8);
            bytes[2] = (byte)((v & 0x00FF0000) >> 16);
            bytes[3] = (byte)((v & 0xFF000000) >> 24);
#else
            byte[] bytes = BitConverter.GetBytes(m_values[m_index]);
#endif

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= m_xorKey;
            }

            float value = BitConverter.ToSingle(bytes, 0);

#if SHIFT_WHEN_GET
            SetValue(value);
#endif

            return value;
        }

        public void SetValue(float value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetByteXorKey();
            byte[] bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= m_xorKey;
            }

#if FAST_FLOAT
            m_values[m_index] = (bytes[0] | bytes[1] << 8 | bytes[2] << 16 | bytes[3] << 24);
#else
            m_values[m_index] = BitConverter.ToUInt32(bytes, 0);
#endif
        }
    }

    internal struct UInt16Chiper
    {
        private ushort m_xorKey;
        private int m_index;
        private ValueArray<ushort> m_values;

        public UInt16Chiper(ushort value)
        {
            m_xorKey = 0;
            m_index = 0;
            m_values = new ValueArray<ushort>();
            SetValue(value);
        }

        public ushort GetValue()
        {
            ushort value = m_values[m_index];
            value ^= m_xorKey;

#if SHIFT_WHEN_GET
            SetValue(value);
#endif
            return value;
        }

        public void SetValue(ushort value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetUInt16XorKey();
            value ^= m_xorKey;
            m_values[m_index] = value;
        }
    }

    internal struct UInt32Chiper
    {
        private uint m_xorKey;
        private int m_index;
        private ValueArray<uint> m_values;

        public UInt32Chiper(uint value)
        {
            m_xorKey = 0;
            m_index = 0;
            m_values = new ValueArray<uint>();
            SetValue(value);
        }

        public uint GetValue()
        {
            uint value = m_values[m_index] ^ m_xorKey;

#if SHIFT_WHEN_GET
            SetValue(value);
#endif
            return value;
        }

        public void SetValue(uint value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetUInt32XorKey();
            m_values[m_index] = value ^ m_xorKey;
        }
    }

    internal struct UInt64Chiper
    {
        private ulong m_xorKey;
        private int m_index;
        private ValueArray<ulong> m_values;

        public UInt64Chiper(ulong value)
        {
            m_xorKey = 0;
            m_index = 0;
            m_values = new ValueArray<ulong>();
            SetValue(value);
        }

        public ulong GetValue()
        {
            ulong value = m_values[m_index];
            value ^= m_xorKey;

#if SHIFT_WHEN_GET
            SetValue(value);
#endif

            return value;
        }

        public void SetValue(ulong value)
        {
            m_index = KeyGenerator.NewIndex(m_index);
            m_xorKey = KeyGenerator.GetUInt64XorKey();
            value ^= m_xorKey;
            m_values[m_index] = value;
        }
    }
}