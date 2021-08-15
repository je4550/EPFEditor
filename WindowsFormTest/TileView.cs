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
        private List<Bitmap> bitmapList;

        public TileView()
        {
            InitializeComponent();
        }

        private void TileView_Load(object sender, EventArgs e)
        {
            epf = new EPFReader();
            bitmapList = new List<Bitmap>();
            frameList = epf.ReadEPF(Selection);
            LoadBitmapList();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var maxSize = GetMaxFrameSize();
            for (int i = 0; i < frameList.Count; i++)
            {
                int row = (int)Math.Floor(i / 16f);
                int column = i % 16;

                Rectangle r = new Rectangle(column * maxSize, row * maxSize, bitmapList[i].Width, bitmapList[i].Height);
                e.Graphics.DrawImage(bitmapList[i], r);
            }
        }

        private void LoadBitmapList()
        {
            for (int i = 0; i < frameList.Count; i++)
            {
                bitmapList.Add(EPFRenderer.RenderEPF(frameList[i]));
            }
        }

        private int GetMaxFrameSize()
        {
            var size = 0;

            for (var i = 0; i < frameList.Count; i++)
            {
                if (bitmapList[i].Width > size)
                {
                    size = bitmapList[i].Width;
                }

                if (bitmapList[i].Height > size)
                {
                    size = bitmapList[i].Height;
                }
            }

            return size;
        }
    }
}
