using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum ServerPackets
{
    welcome = 1,
    spawnPlayer,
    playerPosition,
    playerRotation,
    playerDisconnected,
    playerHealth,
    playerRespawned
}

public enum ClientPackets
{
    welcomeReceived = 1,
    playerMovement,
    playerShoot
}

public class Packet : IDisposable
{
    private List<byte> bytespace;
    private byte[] readableBuffer;
    private int readPos;

    public Packet()
    {
        bytespace = new List<byte>(); 
        readPos = 0; 
    }

    public void SetBytes(byte[] _data)
    {
        Write(_data);
        readablebytespace = buffer.ToArray();
    }
    
    
    public Packet(int _id)
    {
        bytespace= new List<byte>(); 
        readPos = 0; 

        Write(_id); 
    }

 
    public void WriteLength()
    {
        buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count)); 
    }

    public void InsertInt(int _value)
    {
        buffer.InsertRange(0, BitConverter.GetBytes(_value)); 
    }

    public byte[] ToArray()
    {
        readablebytespace= buffer.ToArray();
        return readableBuffer;
    }

    public int Length()
    {
        return buffer.Count; 
    }

    public int UnreadLength()
    {
        return Length() - readPos; 
    }

    public void Reset(bool _shouldReset = true)
    {
        if (_shouldReset)
        {
            buffer.Clear(); 
            readablebytespace= null;
            readPos = 0; 
        }
        else
        {
            readPos -= 4; 
        }
    }
    
    
    public Packet(byte[] _data)
    {
        bytespace= new List<byte>(); 
        readPos = 0; 

        SetBytes(_data);
    }

    #region Write Data
    public void Write(byte _value)
    {
        buffer.Add(_value);
    }
    public void Write(byte[] _value)
    {
        buffer.AddRange(_value);
    }
    public void Write(short _value)
    {
        buffer.AddRange(BitConverter.GetBytes(_value));
    }
    public void Write(int _value)
    {
        buffer.AddRange(BitConverter.GetBytes(_value));
    }
    public void Write(long _value)
    {
        buffer.AddRange(BitConverter.GetBytes(_value));
    }
    public void Write(float _value)
    {
        buffer.AddRange(BitConverter.GetBytes(_value));
    }
    public void Write(bool _value)
    {
        buffer.AddRange(BitConverter.GetBytes(_value));
    }
    public void Write(string _value)
    {
        Write(_value.Length); 
        buffer.AddRange(Encoding.ASCII.GetBytes(_value));
    }
    public void Write(Vector3 _value)
    {
        Write(_value.x);
        Write(_value.y);
        Write(_value.z);
    }
    public void Write(Quaternion _value)
    {
        Write(_value.x);
        Write(_value.y);
        Write(_value.z);
        Write(_value.w);
    }
    #endregion

    #region Read Data
    public byte ReadByte(bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            byte _value = readableBuffer[readPos]; 
            if (_moveReadPos)
            {
                readPos += 1; 
            }
            return _value; 
        }
        else
        {
            throw new Exception("Could not read value of type 'byte'!");
        }
    }
    public byte[] ReadBytes(int _length, bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            byte[] _value = buffer.GetRange(readPos, _length).ToArray(); 
            if (_moveReadPos)
            {
                readPos += _length; 
            }
            return _value; 
        }
        else
        {
            throw new Exception("Could not read value of type 'byte[]'!");
        }
    }

    public short ReadShort(bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            short _value = BitConverter.ToInt16(readableBuffer, readPos); 
            if (_moveReadPos)
            {
                readPos += 2; 
            }
            return _value; 
        }
        else
        {
            throw new Exception("Could not read value of type 'short'!");
        }
    }

    public int ReadInt(bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            int _value = BitConverter.ToInt32(readableBuffer, readPos); 
            if (_moveReadPos)
            {
                readPos += 4; 
            }
            return _value; 
        }
        else
        {
            throw new Exception("Could not read value of type 'int'!");
        }
    }

    public long ReadLong(bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            long _value = BitConverter.ToInt64(readableBuffer, readPos); 
            if (_moveReadPos)
            {
                readPos += 8; 
            }
            return _value; 
        }
        else
        {
            throw new Exception("Could not read value of type 'long'!");
        }
    }
    public float ReadFloat(bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            float _value = BitConverter.ToSingle(readableBuffer, readPos); 
            if (_moveReadPos)
            {
                readPos += 4; 
            }
            return _value; 
        }
        else
        {
            throw new Exception("Could not read value of type 'float'!");
        }
    }

    public bool ReadBool(bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            bool _value = BitConverter.ToBoolean(readableBuffer, readPos); 
            if (_moveReadPos)
            {
                readPos += 1;
            }
            return _value; 
        }
        else
        {
            throw new Exception("Could not read value of type 'bool'!");
        }
    }

    public string ReadString(bool _moveReadPos = true)
    {
        try
        {
            int _length = ReadInt(); 
            string _value = Encoding.ASCII.GetString(readableBuffer, readPos, _length); 
            if (_moveReadPos && _value.Length > 0)
            {
                readPos += _length; 
            }
            return _value;
        }
        catch
        {
            throw new Exception("Could not read value of type 'string'!");
        }
    }

    public Vector3 ReadVector3(bool _moveReadPos = true)
    {
        return new Vector3(ReadFloat(_moveReadPos), ReadFloat(_moveReadPos), ReadFloat(_moveReadPos));
    }

    public Quaternion ReadQuaternion(bool _moveReadPos = true)
    {
        return new Quaternion(ReadFloat(_moveReadPos), ReadFloat(_moveReadPos), ReadFloat(_moveReadPos), ReadFloat(_moveReadPos));
    }
    #endregion

    private bool disposed = false;

    protected virtual void Dispose(bool _disposing)
    {
        if (!disposed)
        {
            if (_disposing)
            {
                bytespace= null;
                readablebytespace= null;
                readPos = 0;
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
