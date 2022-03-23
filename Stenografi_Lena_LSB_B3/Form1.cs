


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stenografi_Lena_LSB_B3
{
    public partial class Form1 : Form
    {
        public List<int> calculatedbinaries = new List<int>();
        int infonum = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private int binarytodecimal(int[] n)
        {
            int sum = 0;

            for (int i = 0; i < 8; i++)
            {
                sum += n[i] * (int)Math.Pow(2, i);
            }

            return sum;
        }

        private int[] decimaltobinary(int n)
        {
            int[] binaries = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 8; i++)
            {

                if (n == 1)
                {
                    binaries[i] = 1;
                    break;
                }
                binaries[i] = n % 2;
                n = (int)n / 2;
            }
            return binaries;
        }

        private void createbits(char x)
        {
            int[] binaries = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            int charnum = (int)x;

            binaries = decimaltobinary(charnum);

            for (int i = 7; i > -1; i--)
            {
                calculatedbinaries.Add(binaries[i]);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //işle
            
            infonum = 0;
            calculatedbinaries = new List<int>();
            Boolean finished = false;
            int finishcount = 0;
            Bitmap bmp = (Bitmap)pictureBox1.Image.Clone();

            String str = "In order to embed more encrypted data and obtain lesser degeneration in the image, embedding was performed by changing 2 least significant bits (4 intotal) of the Green and Blue (G and B) channels ofthe cover-image RGB channels through a K-bit LSBalgorithm. Sensing of the colours is related to theamount of light in a given wave length. The humaneye detects light whose wave length is between 370-770 nm. The linear order of the wave lengths formsthe colour spectrum. The colour sense which thesewave lengths form in the visual system are red - 620nm, green - 530 nm and blue - 470 nm of the spectralcolours. The detection of the spectrum is calledspectral sense (Griffin, 2009). In addition, the numberof color sensors (cones) is different from the numberof luminance sensors (rods) in the human eye (rods >cones). Thus, the light sensitivity and color sensitivityof the human vision system (HVS) are different fromeach other (Yalman and Erturk., 2013; Koçak, 2015).In consideration of the spectral sensitivity of the eye,the Green (G) and the Blue (B) bits of the image,which are low, are used and by reasons of the sensitivity in the red colour, the R channel was not used.";
            richTextBox1.Text = str;
            char[] ch = new char[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                ch[i] = str[i];
            }
            String key = "88";
            char[] ch2 = new char[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                ch2[i] = key[i];
                richTextBox4.Text += ch2[i];
            }

            int j = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (j == key.Length)
                {
                    j = 0;
                }
                ch[i] = (char)(ch[i] ^ ch2[j]);
                j++;
                richTextBox5.Text += ch[i];
            }

            for (int i = 0; i < str.Length; i++)
            {
                createbits(ch[i]);
            }

            for(int i= 24 - calculatedbinaries.Count % 24; i > 0; i--)
            {
                calculatedbinaries.Add(0);
            }

           

            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {

                    Color pixel = bmp.GetPixel(x, y);
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;
                    if (!finished)
                    {
                        
                       

                        int [] binaries = decimaltobinary(b);
                        binaries[0] = calculatedbinaries[infonum];
                        infonum++;

                        binaries[1] = calculatedbinaries[infonum];
                        infonum++;

                        binaries[2] = calculatedbinaries[infonum];
                        infonum++;


                        b = binarytodecimal(binaries);
                        pixel = Color.FromArgb(r, g, b);
                        bmp.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        if (finishcount/9 >= 1 && finishcount / 9 < 2)
                        {
                            

                            int [] binaries = decimaltobinary(b);
                            binaries[0] = 0;
                            finishcount++;
                            binaries[1] = 0;
                            finishcount++;
                            binaries[2] = 0;
                            finishcount++;

                            b = binarytodecimal(binaries);
                            
                        }
                        else
                        {
                            

                            int[] binaries = decimaltobinary(b);
                            binaries[0] = 1;
                            finishcount++;
                            binaries[1] = 1;
                            finishcount++;
                            binaries[2] = 1;
                            finishcount++;
                            b = binarytodecimal(binaries);
                            
                        }
                        
                        pixel = Color.FromArgb(r, g, b);
                        bmp.SetPixel(x, y, pixel);
                       
                    }
                    
                    if (infonum == calculatedbinaries.Count) finished=true;
                    if (finishcount == 27) break;
                }
                if (finishcount == 27) break;
            }


            pictureBox2.Image = bmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"BMP|*.bmp" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image.Save(saveFileDialog.FileName);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //çöz
            infonum = 0;
            calculatedbinaries = new List<int>();
            String calculatedString = "";
            int finishedcount =0;
            bool fcase =false;
            Bitmap bmp = (Bitmap)pictureBox2.Image.Clone();



            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {

                    Color pixel = bmp.GetPixel(x, y);
                    int b = pixel.B;

                    if (infonum % 24 == 0) fcase = true;
                    

                   

                    int [] binaries = decimaltobinary(b);
                    calculatedbinaries.Add(binaries[0]);
                    infonum++;
                    calculatedbinaries.Add(binaries[1]);
                    infonum++;
                    calculatedbinaries.Add(binaries[2]);
                    infonum++;



                    if (fcase && finishedcount / 9 >= 1 && finishedcount / 9 < 2)
                    {
                        if (binaries[0] == 0 && binaries[1] == 0 && binaries [2]==0) finishedcount += 3;
                        else
                        {
                            fcase = false;
                            finishedcount = 0;
                        }

                    }
                    else if(fcase)
                    {
                        if (binaries[0] == 1 && binaries[1] == 1 && binaries[2] == 1) {
                            finishedcount += 3;
                            
                        }
                        else
                        {
                            fcase = false;
                            finishedcount = 0;


                        } 
                            
                        
                    }

                    if (finishedcount==27) break;

                }
                if (finishedcount == 27) break;
            }

            String key = "88";
            char[] ch2 = new char[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                ch2[i] = key[i];
            }

            int sum = 0;
            for (int i = 0; i < infonum -27 ; i+=8)
            {
                sum = 0;
                for (int j = 0; j<8; j++)
                {
                    sum += (int)(calculatedbinaries[i+7 - j] * Math.Pow(2, j));
                }
                if(sum!=0) calculatedString += (char)sum;
            }

            //richTextBox1.Text = calculatedString;
            //calculatedString

            char[] ch = new char[calculatedString.Length];
            for (int i = 0; i < calculatedString.Length; i++)
            {
                ch[i] = calculatedString[i];
            }

            int k = 0;
            for (int i = 0; i < calculatedString.Length; i++)
            {
                if (k == key.Length)
                {
                    k = 0;
                }
                ch[i] = (char)(ch[i] ^ ch2[k]);
                k++;
                richTextBox2.Text += ch[i];
            }


        }


        private void button3_Click(object sender, EventArgs e)
        {
            
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = @"BMP|*.bmp" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //fark hesapla
            double[] MSE = new double[3];
            double[] PSNR = new double[3];

            for (int i = 0; i < 3; i++)
            {
                MSE[i] = 0;
                PSNR[i] = 0;
            }

            int x, y; Color pixel1, pixel2;
            int dif1=0, dif2=0, dif3=0;

            Bitmap bmp1 = (Bitmap)pictureBox1.Image.Clone();
            Bitmap bmp2 = (Bitmap)pictureBox2.Image.Clone();

            for (y = 0; y < 256; y++)
            {
                for (x = 0; x < 256; x++)
                {
                    pixel1 = bmp1.GetPixel(x, y); 
                    pixel2 = bmp2.GetPixel(x, y);
                    dif1 = dif1 + (pixel1.R - pixel2.R) * (pixel1.R - pixel2.R);
                    dif2 = dif2 + (pixel1.G - pixel2.G) * (pixel1.G - pixel2.G);
                    dif3 = dif3 + (pixel1.B - pixel2.B) * (pixel1.B - pixel2.B);
                }
            }

            MSE[0] = (double)dif1 / (bmp1.Height * bmp1.Width);
            MSE[1] = (double)dif2 / (bmp1.Height * bmp1.Width);
            MSE[2] = (double)dif3 / (bmp1.Height * bmp1.Width);
            PSNR[0] = 20 * Math.Log10(255 / Math.Sqrt(MSE[0]));
            PSNR[1] = 20 * Math.Log10(255 / Math.Sqrt(MSE[1]));
            PSNR[2] = 20 * Math.Log10(255 / Math.Sqrt(MSE[2]));

            textBox1.Text = "R: " + MSE[0];
            textBox2.Text = "G: " + MSE[1];
            textBox3.Text = "B: " + MSE[2];

            textBox4.Text = "R: " + PSNR[0];
            textBox5.Text = "G: " + PSNR[1];
            textBox6.Text = "B: " + PSNR[2];
        }

        private void button6_Click(object sender, EventArgs e)
        {

            Bitmap bmp1 = (Bitmap)pictureBox1.Image.Clone();
            Color pixel;

            for(int y = 0; y < 256; y++)
            {
                for(int x = 0; x < 256; x++)
                {
                    pixel = bmp1.GetPixel(x,y);
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;
                    int sum = (int)((r + b + g) / 3);
                    r = sum;
                    g = sum;
                    b = sum;
                    pixel = Color.FromArgb(r, g, b);
                    bmp1.SetPixel(x, y, pixel);
                }

            }
            pictureBox2.Image = bmp1;
        }

       

       
    }
}