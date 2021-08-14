using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsFormTest
{
    public class EPFRenderer
    {
        private Form form;
        public EPFRenderer(Form form) => this.form = form;

        ///<summary>
        ///Convert an EPF Frame to Bitmap
        ///</summary>        
        public static Bitmap RenderEPF(Frame epf)
        {
            int width = epf.getWidth();
            int height = epf.getHeight();
            Palette256[] tilePal = Palette256.FromFile("body.pal");
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bitmap.Palette;
            palette.Entries[0] = Color.Transparent;

            for (int i = 1; i < 256; i++)
            {
                Color color = tilePal[0][i];
                palette.Entries[i] = color;
            }

            bitmap.Palette = palette;
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            byte[] pixelData = new byte[bitmap.Height * bitmapdata.Stride];
            Marshal.Copy(bitmapdata.Scan0, pixelData, 0, pixelData.Length);

            for (int i = 0; i < height; i++)
            {
                int index = i * bitmapdata.Stride;
                for (int j = 0; j < width; j++)
                {
                    pixelData[index + j] = epf.rawData[(i * width) + j];
                }
            }
            Marshal.Copy(pixelData, 0, bitmapdata.Scan0, pixelData.Length);
            bitmap.UnlockBits(bitmapdata);

            return bitmap;
        }

        ///<summary>
        ///Take in raw Frame EPF data and then return a bitmap and display the bytes in a textbox
        ///</summary>
        public static Bitmap RenderEPF(Frame epf, RichTextBox textbox)
        {
            int width = epf.getWidth();
            int height = epf.getHeight();
            Palette256[] tilePal = Palette256.FromFile("body.pal");
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bitmap.Palette;
            palette.Entries[0] = Color.Transparent;

            for (int i = 1; i < 256; i++)
            {
                Color color = tilePal[0][i];
                palette.Entries[i] = color;
            }

            bitmap.Palette = palette;
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            byte[] pixelData = new byte[bitmap.Height * bitmapdata.Stride];
            Marshal.Copy(bitmapdata.Scan0, pixelData, 0, pixelData.Length);

            for (int i = 0; i < height; i++)
            {
                int index = i * bitmapdata.Stride;
                for (int j = 0; j < width; j++)
                {
                    pixelData[index + j] = epf.rawData[(i * width) + j];
                }
            }
            Marshal.Copy(pixelData, 0, bitmapdata.Scan0, pixelData.Length);
            bitmap.UnlockBits(bitmapdata);

            StringBuilder text = new StringBuilder();
            for (int j = 0; j < pixelData.Length; j++)
            {
                if (j % width == 0 && j > 0)
                {
                    text.Append("\n");
                }
                text.Append(pixelData[j].ToString("X2") + " ");
            }
            textbox.Text = text.ToString();
            
            return bitmap;
        }
        ///<summary>
        ///Take in raw Frame EPF data and then return a bitmap and display the bytes in a textbox.
        ///Requires palette
        ///</summary>
        public static Bitmap RenderEPF(Frame epf, RichTextBox textbox, int paletteNum, Form form)
        {
            Control panel = form.Controls.Find("panel1", true)[0];
            int width = epf.getWidth();
            int height = epf.getHeight();
            Palette256[] tilePal = Palette256.FromFile("body.pal");
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bitmap.Palette;
            palette.Entries[0] = Color.Transparent;
            ClearPalette((Panel)panel);
            for (int i = 1; i < 256; i++)
            {
                Color color = tilePal[paletteNum][i];
                palette.Entries[i] = color;
                //DrawPalette(form, i, color);
            }
            DrawPalette(palette.Entries, form);

            bitmap.Palette = palette;
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            byte[] pixelData = new byte[bitmap.Height * bitmapdata.Stride];
            Marshal.Copy(bitmapdata.Scan0, pixelData, 0, pixelData.Length);

            for (int i = 0; i < height; i++)
            {
                int index = i * bitmapdata.Stride;
                for (int j = 0; j < width; j++)
                {
                    pixelData[index + j] = epf.rawData[(i * width) + j];
                }
            }
            
            Marshal.Copy(pixelData, 0, bitmapdata.Scan0, pixelData.Length);
            bitmap.UnlockBits(bitmapdata);

            StringBuilder text = new StringBuilder();
            for (int j = 0; j < pixelData.Length; j++)
            {
                if (j % width == 0 && j > 0)
                {
                    text.Append("\n");
                }
                text.Append(pixelData[j].ToString("X2") + " ");
            }
            textbox.Text = text.ToString();
            
            return bitmap;
        }

        public static Bitmap RenderEPF(Frame epf, Form form, int paletteNum)
        {
            int width = epf.getWidth();
            int height = epf.getHeight();
            Palette256[] tilePal = Palette256.FromFile("body.pal");
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bitmap.Palette;
            palette.Entries[0] = Color.Transparent;
            //DrawPalette(form, 0, palette.Entries[0]);
            for (int i = 1; i < 256; i++)
            {
                Color color = tilePal[paletteNum][i];
                palette.Entries[i] = color;
                //DrawPalette(form, i, color);
            }
            DrawPalette(palette.Entries, form);

            bitmap.Palette = palette;
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            byte[] pixelData = new byte[bitmap.Height * bitmapdata.Stride];
            Marshal.Copy(bitmapdata.Scan0, pixelData, 0, pixelData.Length);

            for (int i = 0; i < height; i++)
            {
                int index = i * bitmapdata.Stride;
                for (int j = 0; j < width; j++)
                {
                    pixelData[index + j] = epf.rawData[(i * width) + j];
                }
            }

            Marshal.Copy(pixelData, 0, bitmapdata.Scan0, pixelData.Length);
            bitmap.UnlockBits(bitmapdata);

            StringBuilder text = new StringBuilder();
            for (int j = 0; j < pixelData.Length; j++)
            {
                if (j % width == 0 && j > 0)
                {
                    text.Append("\n");
                }
                text.Append(pixelData[j].ToString("X2") + " ");
            }
            //form.texttextbox.Text = text.ToString();
            form.Controls.Find("richTextBox1", true)[0].Text = text.ToString();

            return bitmap;
        }

        ///<summary>
        ///Returns a bitmap based on a string, converts to bytes, and then returns a bitmap
        ///</summary>
        public static Bitmap RenderEPF(string input, int width, int height, int paletteNum)
        {
            Palette256[] tilePal = Palette256.FromFile("body.pal");

            byte[] pixelData = StringToByteArray(input);

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bitmap.Palette;
            palette.Entries[0] = Color.Transparent;

            for (int i = 1; i < 256; i++)
            {
                Color color = tilePal[paletteNum][i];
                palette.Entries[i] = color;
            }

            bitmap.Palette = palette;

            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(pixelData, 0, bitmapdata.Scan0, pixelData.Length);
            bitmap.UnlockBits(bitmapdata);

            return bitmap;
        }

        ///<summary>
        ///Returns a bitmap based on a pixel data, and then returns a bitmap
        ///</summary>
        public static Bitmap RenderEPF(byte[] pixelData, int width, int height, int paletteNum)
        {
            Palette256[] tilePal = Palette256.FromFile("body.pal");

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bitmap.Palette;
            palette.Entries[0] = Color.Transparent;

            for (int i = 1; i < 256; i++)
            {
                Color color = tilePal[paletteNum][i];
                palette.Entries[i] = color;
            }

            bitmap.Palette = palette;

            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(pixelData, 0, bitmapdata.Scan0, pixelData.Length);
            bitmap.UnlockBits(bitmapdata);

            return bitmap;
        }
        private static byte[] StringToByteArray(String hex)
        {
            hex = hex.Replace(" ", "");
            hex = hex.Replace("\n", "");
            int NumberChars = hex.Length;
            if (hex.Length % 2 == 0)
            {
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
            }
            else
            {
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars - 1; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
            }
        }

        private static void ClearPalette(Panel panel)
        {
            while (panel.Controls.Count > 0) panel.Controls[0].Dispose();
        }

        private static void DrawPalette(Form form, int i, Color color)
        {
            PictureBox pb1 = new PictureBox();
            int column = (int)Math.Floor(i / 16f);
            int row = i % 16;
            pb1.Location = new Point(row * 16, column * 16);
            pb1.Name = i.ToString();
            pb1.Size = new Size(16, 16);
            pb1.BackColor = color;
            pb1.Click += (sender, EventArgs) => { PaletteClick(sender, EventArgs, form); };
            form.Controls.Find("panel1", true)[0].Controls.Add(pb1);
        }
        private static void DrawPalette(Color[] color, Form form)
        {
            Graphics g = form.Controls.Find("panel1", true)[0].CreateGraphics();
            form.Controls.Find("panel1", true)[0].Click += (sender, EventArgs) => { PaletteClick(sender, EventArgs, form); };
            for (int i = 0; i < 256; i++)
            {
                int column = (int)Math.Floor(i / 16f);
                int row = i % 16;
                Rectangle r = new Rectangle(row * 16, column * 16, 16, 16);
                
                g.FillRectangle(new SolidBrush(color[i]), r);
            }
        }
        private static void PaletteClick(object sender, EventArgs e, Form form)
        {
            Point pos = form.Controls.Find("panel1", true)[0].PointToClient(Cursor.Position);
            int x = (int)Math.Ceiling(pos.X / 16f);
            int y = (int)Math.Ceiling(pos.Y / 16f);
            int tile = ((y - 1) * 16) + x - 1;
            form.Controls.Find("label1", true)[0].Text = tile.ToString();
        }
    }
}
