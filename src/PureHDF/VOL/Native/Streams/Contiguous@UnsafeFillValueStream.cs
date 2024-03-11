﻿namespace PureHDF;

internal class UnsafeFillValueStream : IH5ReadStream
{
    private readonly byte[]? _fillValue;
    private readonly int _length;
    private long _position;

    public UnsafeFillValueStream(byte[]? fillValue)
    {
        _fillValue = fillValue;

        _length = _fillValue is null
            ? 1
            : _fillValue.Length;
    }

    public long Position { get => _position; }

    public unsafe void ReadDataset(Span<byte> buffer)
    {
        if (_fillValue is null)
        {
            buffer.Clear();
        }

        else
        {
            unsafe
            {
                fixed (byte* ptrSrc = _fillValue, ptrDst = buffer)
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        ptrDst[i] = ptrSrc[(_position + i) % _length];
                    }
                }
            }

            _position += buffer.Length;
        }
    }

    public void Seek(long offset, SeekOrigin origin)
    {
        _position += origin switch
        {
            SeekOrigin.Begin => offset,
            _ => throw new NotImplementedException(),
        };
    }

    public void Dispose()
    {
        // do nothing
    }
}
