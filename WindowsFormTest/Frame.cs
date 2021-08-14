using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormTest
{
    public class Frame
    {
        public int x1 { get; set; }
        public int x2;
        public int y1;
        public int y2;
        public int pixelOffset;
        public int stencilOffset;
        public byte[] rawData;
        public List<int> stencilData;
        
        public Frame(int x1, int x2, int y1, int y2, int pixelOffset, int stencilOffset, byte[] rawData)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            this.pixelOffset = pixelOffset;
            this.stencilOffset = stencilOffset;
            this.rawData = rawData;
            calculateStencilData(rawData);
        }
        public void calculateStencilData(byte[] rawData)
        {
            var temp = 0;
            var stencilCount = 0;
            var count = 0;
            stencilData = new List<int>();

            if(rawData.Length == 0)
            {
                Console.WriteLine("Length of 0");
                return;
            }

            if(rawData[0] > 0)
            {
                temp = rawData[0];
            }

            foreach(var data in rawData)
            {
                count++;
                
                if (data == 0 && temp == 0)
                {
                    temp = data;
                    stencilCount++;
                }
                if (data > 0 && temp > 0)
                {
                    temp = data;
                    stencilCount++;
                }
                if (data > 0 && temp == 0)
                {
                    stencilData.Add(stencilCount);
                    temp = data;
                    stencilCount = 1;
                }
                if (data == 0 && temp > 0)
                {
                    stencilData.Add(stencilCount ^ (1 << 7));
                    temp = data;
                    stencilCount = 1;
                }
                if(count > 0 && count % getWidth() == 0)
                {
                    stencilData.Add(stencilCount);
                    stencilData.Add(0);
                    stencilCount = 0;
                }
            }
        }
        public int getWidth()
        {
            return x2 - x1;
        }
        public int getHeight()
        {
            return y2 - y1;
        }
    }
}
