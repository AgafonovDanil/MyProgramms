using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace WindowsFormsAppПроект
{
    public partial class ProjectClock : Form
    {
        public ProjectClock()
        {
            InitializeComponent();
        }
       

        private void ProjectClock_Load(object sender, EventArgs e)
        {
            this.Invalidate();
            this.Paint += ProjectClock_Paint;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        private void ProjectClock_Paint(object sender, PaintEventArgs e)
        {
            this.Invalidate();
            this.Paint += ProjectClock_Paint;


            Graphics G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Color.White);
            int Mx = ClientRectangle.Width;
            int My = ClientRectangle.Height;
            Point ptCenter = new Point(Mx / 2, My / 2);
            int R = Math.Min(Mx, My) / 2 - 1;
            using (Pen P = new Pen(Color.Black, 2))
                G.DrawEllipse(P, ptCenter.X - R, ptCenter.Y - R, R + R, R + R);
           using (LinearGradientBrush br = new LinearGradientBrush
                (ClientRectangle, Color.White, Color.DarkGray,
                   LinearGradientMode.BackwardDiagonal))
               G.FillEllipse(br, ptCenter.X - R, ptCenter.Y - R, R + R, R + R);
            for (int minute = 0; minute < 60; minute++)
            {
                Point pt = RadiaPoint(ptCenter, R - 20, minute);
                using (SolidBrush BS = new SolidBrush(Color.Black))
                {
                    if ((minute % 5) == 0) G.FillRectangle(BS, pt.X - 3, pt.Y - 3, 7, 7);
                    else G.FillRectangle(BS, pt.X - 1, pt.Y - 1, 2, 2);
                }
            }
           DateTime DTime = DateTime.Now;
            using (Pen P = new Pen(Color.FromArgb(80, 20, 70, 30), 4))
            {
                P.CustomEndCap = new AdjustableArrowCap(4, 6, true);
                Point ptN = RadiaPoint(ptCenter, 20, DateTime.Now.Second);
                Point ptE = RadiaPoint(ptCenter, R - 2, DateTime.Now.Second);
                G.DrawLine(P, ptN, ptE);
            }
            using (Pen P = new Pen(Color.FromArgb(100, 0, 0, 0), 6))
            {
                P.StartCap = LineCap.RoundAnchor;
                P.EndCap = LineCap.Round;
                G.DrawLine(P, RadiaPoint(ptCenter, 15, 30 + DateTime.Now.Minute),
                              RadiaPoint(ptCenter, (int)(R * 0.8), DateTime.Now.Minute));
            }
            using (Pen P = new Pen(Color.Black, 8))
            {
                P.StartCap = LineCap.Flat;
                P.EndCap = LineCap.DiamondAnchor;
                float[] CV = new float[] { 0.0f, 0.2f, 0.5f, 0.7f, 0.9f, 1.0f };
                P.CompoundArray = CV;
                G.DrawLine(P, RadiaPoint(ptCenter, 15, 30 + DTime.Hour * 5 + DTime.Minute / 12),
                    RadiaPoint(ptCenter, (int)(R * 0.75), DTime.Hour * 5 + DTime.Minute / 12));
            }



        }



        private void timer1_Tick(object sender, EventArgs e)
        {
           Text = DateTime.Now.ToLongTimeString();
           
           

            Invalidate();

        }
        private Point RadiaPoint(Point ptCenter, int R, int seconds)
        {

            double tickAngle = -((seconds - 15) % 60) * Math.PI / 30;
            return new Point(ptCenter.X + (int)(R * Math.Cos(tickAngle)),
                ptCenter.Y - (int)(R * Math.Sin(tickAngle)));
        }

     
    }
}
