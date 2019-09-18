using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;

namespace Tieba
{
    class imghash
    {
        Image SourceImg;
        bool dctbool;
        public imghash(string filePath,bool dctbool=false)
        {
            if (filePath.StartsWith("http://"))
            {
                SourceImg = Image.FromStream(new WebClient().OpenRead(filePath));
            }
            else
            {
                SourceImg = Image.FromFile(filePath);
            }

            this.dctbool = dctbool;

        }

        //public imghash(Stream stream)
        //{
        //    SourceImg = Image.FromStream(stream);
        //}

        public String GetHash()
        {
            Image image = ReduceSize();
            int[] grayValues = ReduceColor(image);
                 if(dctbool) grayValues = dct(grayValues, 8);
            int average = CalcAverage(grayValues);
            String reslut = ComputeBits(grayValues, average);
            return reslut;
        }

        // Step 1 : Reduce size to 8*8  
        private Image ReduceSize(int width = 8, int height = 8)
        {
            Image image = SourceImg.GetThumbnailImage(width, height, () => { return false; }, IntPtr.Zero);
            return image;
        }

        // Step 2 : Reduce Color  
        private int[] ReduceColor(Image image)
        {
            Bitmap bitMap = new Bitmap(image);
            int[] grayValues = new int[image.Width * image.Height];

            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                {
                    Color color = bitMap.GetPixel(x, y);
                    int grayValue = (color.R * 30 + color.G * 59 + color.B * 11) / 100;
                    grayValues[x * image.Width + y] = grayValue;
                }
            return grayValues;
        }

        //dct
        private int[] dct(int[] values,int n)
        {

            return Transformation.DCT(values, n);
        }


        // Step 3 : Average the colors  
        private int CalcAverage(int[] values)
        {
            int sum = 0;
            for (int i = 0; i < values.Length; i++)
                sum += (int)values[i];
            return Convert.ToInt16(sum / values.Length);
        }

        // Step 4 : Compute the bits  
        private String ComputeBits(int[] values, int averageValue)
        {
            char[] result = new char[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] < averageValue)
                    result[i] = '0';
                else
                    result[i] = '1';
            }
            return new String(result);
        }

        //Compare hash  
        public static Int32 CalcSimilarDegree(string a, string b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException();
            int count = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    count++;
            }
            return count;
        } 
    }
}
