using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace WindowsFormsApp1_test1._16
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //ImageShow_Load(pic/*tureBox1 );*/
            string s = System.Environment.CurrentDirectory;

            pictureBox1.Image = Image.FromFile(s + "\\1.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            Bitmap a = new Bitmap(pictureBox1.Image);
            Mat mat = ConvertFile.BitmapToMat(a);
            Mat newa = new Mat();

            OpenCvSharp.Size ZoomDsize = new OpenCvSharp.Size(pictureBox1.Width, a.Height * pictureBox1.Width / a.Width);

            Cv2.Resize(mat, newa, ZoomDsize);
            Bitmap finala = new Bitmap(ConvertFile.MatToBitmap(newa));
            pictureBox1.Image = finala;



        }

        // variable Zoompicture
        private bool WheelReadyZoomPicture = false;  //给滚轮使用 初始值为false
        private Mat picbox1_ImageMat = null;
        //private Mat picbox2_ImageMat = null;
        private Bitmap ButtonClickOriginalBitmap = null; // 给button使 记录原始数据
        //private Bitmap ButtonClickOriginalBitmap2 = null;
        private double scale_double = 1;  // 用于记录滚轮滚动带来的乘子变化
        //private double scale_double2 = 1;




        // 添加滚轮控件
     



        void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            System.Drawing.Point p = e.Location;
            if (WheelReadyZoomPicture)
            {
                int i = e.Delta * SystemInformation.MouseWheelScrollLines;

                /// Zoom the picture
                Mat ZoomPictureMat = new Mat();
                OpenCvSharp.Size ZoomDsize = new OpenCvSharp.Size();

                // zoom scale
                //float scale_float = 1 + i / 3600f;
                scale_double = scale_double + i / 3600.0; //表示当前乘子

                int ZoomWidth = (int)Convert.ToInt32(ButtonClickOriginalBitmap.Width * scale_double);
                int ZoomHeight = (int)Convert.ToInt32(ButtonClickOriginalBitmap.Height * scale_double);
                if (ZoomHeight <= ButtonClickOriginalBitmap.Height || ZoomWidth <= ButtonClickOriginalBitmap.Width)
                {
                    
                    pictureBox1.Image = ButtonClickOriginalBitmap;

                    scale_double = 1;
                }
                else
                {
                    ZoomDsize = new OpenCvSharp.Size(ZoomWidth, ZoomHeight);
                    Cv2.Resize(picbox1_ImageMat, ZoomPictureMat, ZoomDsize, 0, 0, InterpolationFlags.Cubic);
                    picbox1_ImageMat = ZoomPictureMat;

                    //获得放缩后的中心点坐标
                    int compensation_Y_Value = (pictureBox1.Height - ButtonClickOriginalBitmap.Height) / 2;
                    Rect rect = new Rect((int)(scale_double * p.X) - p.X, (int)((scale_double-1) * (p.Y-compensation_Y_Value)) , ButtonClickOriginalBitmap.Width, ButtonClickOriginalBitmap.Height);
                    Mat matfinal = new Mat(ZoomPictureMat, rect);
                    Bitmap bitmapFinal_2 = ConvertFile.MatToBitmap(matfinal);
                    pictureBox1.Image = bitmapFinal_2;
                    matfinal = null;
                }
                ZoomPictureMat = null;
                GC.Collect();
            }



        }
        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            WheelReadyZoomPicture = !WheelReadyZoomPicture;

            if (WheelReadyZoomPicture)
            {
                button1.BackColor = Color.Azure;

                ButtonClickOriginalBitmap = new Bitmap(pictureBox1.Image); // recording the raw data

                picbox1_ImageMat = ConvertFile.BitmapToMat(ButtonClickOriginalBitmap).Clone(); // 用於以後更改的MAT数据
            }
            else
            {
                button1.BackColor = Color.Bisque;

                picbox1_ImageMat = null;
                pictureBox1.Image = ButtonClickOriginalBitmap;


                //picbox2_ImageMat = null;
                //pictureBox2.Image = ButtonClickOriginalBitmap2;


            }


        }

        void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            System.Drawing.Point p = e.Location;
            PictureBox pic = (PictureBox)sender;
            Bitmap picbit = new Bitmap(pic.Image); // recording the raw data


            Mat picMat = ConvertFile.BitmapToMat(picbit);
            //new Mat(, ImreadModes.Color);
            picMat = picMat.CvtColor(ColorConversionCodes.RGBA2RGB, 3);

            OpenCvSharp.Point p2 = new OpenCvSharp.Point(0, p.Y);
            OpenCvSharp.Point p3 = new OpenCvSharp.Point(pic.Width, p.Y);

            picMat.Line(p2, p3, Scalar.Red,5 , LineTypes.Link8);

            //OpenCvSharp.conver
            Bitmap bitmap = ConvertFile.MatToBitmap(picMat);


            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.Image = bitmap;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();

            form2.change();
            
            form2.Show();
            //this.Close();
        }
    }

}