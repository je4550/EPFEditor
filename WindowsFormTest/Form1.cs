using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormTest
{
    public partial class Form1 : Form
    {
        private List<Frame> frameList;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetEPFFiles();
            ushort count = Palette256.GetPaletteCount("body.pal");
            Object[] palettes = new Object[count];
            for(ushort i = 0; i < count; i++)
            {
                palettes[i] = i + 1;
            }
            comboBox1.Items.AddRange(palettes);
            comboBox1.SelectedIndex = 0;            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = EPFRenderer.RenderEPF(frameList[listBox1.SelectedIndex], this, comboBox1.SelectedIndex);
            string str = "";
            foreach(var data in frameList[listBox1.SelectedIndex].stencilData)
            {
                str += data.ToString("X2") + " ";
            }
            richTextBox2.Text = str;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            var width = frameList[listBox1.SelectedIndex].getWidth();
            var height = frameList[listBox1.SelectedIndex].getHeight();
            string input = richTextBox1.Text;
            pictureBox1.Image = EPFRenderer.RenderEPF(input, width, height, comboBox1.SelectedIndex);
        }

        public void GetEPFFiles()
        {
            var files = EPFFinder.EPFFiles;

            foreach (var file in files)
            {
                listBox2.Items.Add(file);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = listBox2.SelectedItem;
            if(index != null)
            {
                EPFReader epf = new EPFReader();
                if(frameList != null)
                {
                    frameList.Clear();
                    listBox1.Items.Clear();
                }
                frameList = epf.ReadEPF(index.ToString());
                progressBar1.Maximum = frameList.Count;
                progressBar1.Value = 0;
                for (int i = 0; i < frameList.Count; i++)
                {
                    listBox1.Items.Add(i);
                    progressBar1.Value += 1;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.Items.Count > 0 && listBox1.SelectedIndex > 0)
            {
                pictureBox1.Image = EPFRenderer.RenderEPF(frameList[listBox1.SelectedIndex], richTextBox1, comboBox1.SelectedIndex, this);
            }
        }

        private void loadAll_Click(object sender, EventArgs e)
        {
            TileView tv = new TileView();
            if(listBox2.SelectedItem != null)
            {
                tv.Selection = listBox2.SelectedItem.ToString();
                tv.Show();
            }            
            else
            {
                MessageBox.Show("Please select a file first.");
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
