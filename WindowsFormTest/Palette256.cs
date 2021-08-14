using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormTest
{
    public class Palette256
    {
        private readonly Color[] colors = new Color[0x100];
        public Color this[int index]
        {
            get
            {
                return colors[index];
            }
            set
            {
                colors[index] = value;
            }
        }
        public static ushort GetPaletteCount(string file)
        {
            byte[] bytes = File.ReadAllBytes(file);
            MemoryStream input = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(input);
            ushort count = 0;
            if (Encoding.ASCII.GetString(bytes, 0, 9) == "DLPalette")
            {
                count = 1;
            }
            else
            {
                count = reader.ReadUInt16();
            }
            return count;
        }
        public static Palette256[] FromFile(string file)
        {
            byte[] bytes = File.ReadAllBytes(file);
            MemoryStream input = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(input);
            Palette256[] pal;
            int offset = 0;
            int count = 0;

            if (Encoding.ASCII.GetString(bytes, 0, 9) == "DLPalette")
            {
                offset = 0;
                count = 1;
                pal = new Palette256[1];                
            }
            else
            {
                offset = 4;
                count = reader.ReadUInt16();
                pal = new Palette256[count];
            }

            input.Position = offset + 32;

            for (int x = 0; x < count; x++)
            {
                pal[x] = new Palette256();
                for (int i = 0; i < 256; i++)
                {
                    int r = reader.ReadByte();
                    int g = reader.ReadByte();
                    int b = reader.ReadByte();
                    //#pragma warning disable 168
                    reader.BaseStream.Seek(1, SeekOrigin.Current);
                    //#pragma warning restore 168
                    pal[x][i] = Color.FromArgb(r, g, b);
                }
            }
            reader.Close();
            input.Dispose();
            return pal;
        }
    }
}


