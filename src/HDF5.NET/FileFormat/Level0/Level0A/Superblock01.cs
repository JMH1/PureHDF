﻿namespace HDF5.NET
{
    internal class Superblock01 : Superblock
    {
        #region Constructors

        public Superblock01(H5BinaryReader reader, byte version) : base(reader)
        {
            SuperBlockVersion = version;
            FreeSpaceStorageVersion = reader.ReadByte();
            RootGroupSymbolTableEntryVersion = reader.ReadByte();
            reader.ReadByte();

            SharedHeaderMessageFormatVersion = reader.ReadByte();
            OffsetsSize = reader.ReadByte();
            LengthsSize = reader.ReadByte();
            reader.ReadByte();

            GroupLeafNodeK = reader.ReadUInt16();
            GroupInternalNodeK = reader.ReadUInt16();

            FileConsistencyFlags = (FileConsistencyFlags)reader.ReadUInt32();

            if (SuperBlockVersion == 1)
            {
                IndexedStorageInternalNodeK = reader.ReadUInt16();
                reader.ReadUInt16();
            }

            BaseAddress = ReadOffset(reader);
            FreeSpaceInfoAddress = ReadOffset(reader);
            EndOfFileAddress = ReadOffset(reader);
            DriverInfoBlockAddress = ReadOffset(reader);
            RootGroupSymbolTableEntry = new SymbolTableEntry(reader, this);
        }

        #endregion

        #region Properties

        public byte FreeSpaceStorageVersion { get; set; }
        public byte RootGroupSymbolTableEntryVersion { get; set; }
        public byte SharedHeaderMessageFormatVersion { get; set; }
        public ushort GroupLeafNodeK { get; set; }
        public ushort GroupInternalNodeK { get; set; }
        public ushort IndexedStorageInternalNodeK { get; set; }
        public ulong FreeSpaceInfoAddress { get; set; }
        public ulong DriverInfoBlockAddress { get; set; }
        public SymbolTableEntry RootGroupSymbolTableEntry { get; set; }

        public DriverInfoBlock? DriverInfoBlock
        {
            get
            {
                if (IsUndefinedAddress(DriverInfoBlockAddress))
                    return null;
                else
                    return new DriverInfoBlock(Reader);
            }
        }

        #endregion
    }
}
