using System.Collections.Generic;
using System.IO;

namespace WindowsFormTest
{
    class EPFReader
    {
        public List<Frame> ReadEPF(string fileName)
        {
            MemoryStream ms = new MemoryStream(File.ReadAllBytes(fileName));
            BinaryReader br = new BinaryReader(ms);
            List<Frame> frameList = new List<Frame>();

            var fileLength = (int)ms.Length;
            byte[] data = new byte[fileLength];

            ms.Read(data, 0, fileLength);
            ms.Position = 0;

            var offset = br.BaseStream.Position;
            var frames = br.ReadUInt16();
            var imageX = br.ReadUInt16();
            var imageY = br.ReadUInt16();
            var unknown = br.ReadUInt16();
            var tocAddress = br.ReadUInt32() + 12;
            br.BaseStream.Position = tocAddress;

            byte[] cutArray = new byte[tocAddress];

            for (uint i = 0; i < tocAddress; i++)
            {
                cutArray[i] = data[i];
            }

            for (int x = 0; x < frames; x++)
            {
                var Y1 = br.ReadInt16();
                var X1 = br.ReadInt16();
                var Y2 = br.ReadInt16();
                var X2 = br.ReadInt16();
                var frameWidth = X2 - X1;
                var frameHeight = Y2 - Y1;
                var pixelOffset = br.ReadInt32() + 12;
                var stencilOffset = br.ReadInt32() + 12;

                var count = stencilOffset - pixelOffset;
                var position = br.BaseStream.Position;
                br.BaseStream.Seek(pixelOffset, SeekOrigin.Begin);
                byte[] rawData = new byte[count];
                rawData = br.ReadBytes(count);
                br.BaseStream.Seek(position, SeekOrigin.Begin);

                frameList.Add(new Frame(X1, X2, Y1, Y2, pixelOffset, stencilOffset, rawData));
            }

            return frameList;
        }
    }
}
