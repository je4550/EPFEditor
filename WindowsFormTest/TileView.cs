using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormTest
{
    public partial class TileView : Form
    {
        public string Selection { get; set; }
        private EPFReader epf;
        private List<Frame> frameList;        

        public TileView()
        {
            InitializeComponent();
        }

        private void TileView_Load(object sender, EventArgs e)
        {
            epf = new EPFReader();
            frameList = epf.ReadEPF(Selection);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var maxSize = GetMaxFrameSize();
            for (int i = 0; i < frameList.Count; i++)
            {
                int row = (int)Math.Floor(i / 16f);
                int column = i % 16;
                Bitmap bitmap = EPFRenderer.RenderEPF(frameList[i]);
                Rectangle r = new Rectangle(column * maxSize, row * maxSize, bitmap.Width, bitmap.Height);
                e.Graphics.DrawImage(bitmap, r);
            }
        }

        private int GetMaxFrameSize()
        {
            var size = 0;

            for (var i = 0; i < frameList.Count; i++)
            {
                Bitmap bitmap = EPFRenderer.RenderEPF(frameList[i]);

                if (bitmap.Width > size)
                {
                    size = bitmap.Width;
                }

                if (bitmap.Height > size)
                {
                    size = bitmap.Height;
                }
            }

            return size;
        }
    }
}
