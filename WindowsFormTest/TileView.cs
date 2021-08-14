using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormTest
{
    public partial class TileView : Form
    {
        public string Selection { get; set; }
        private List<Frame> frameList;

        public TileView()
        {
            InitializeComponent();
        }
        private void TileView_Load(object sender, EventArgs e)
        {
            EPFReader epf = new EPFReader();
            frameList = epf.ReadEPF(Selection);            
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            for(int i = 1; i < frameList.Count; i++)
            {
                int column = (int)Math.Floor(i / 16f);
                int row = i % 16;
                Bitmap bitmap = EPFRenderer.RenderEPF(frameList[i]);
                Rectangle r = new Rectangle(row * bitmap.Width, column * bitmap.Height, bitmap.Width, bitmap.Height);
                e.Graphics.DrawImage(bitmap, r);
            }
        }
    }
}
