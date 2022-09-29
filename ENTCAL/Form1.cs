using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace ENTCAL
{
    public partial class Form1 : Form
    {
        double A, B;
        double[] test_load = null;
        double[] set_load = null;
        String[] dsc_file_name = new String[10];
        String[] nexus_file_name = new String[10];
        
        double[] load = null;
        double[] henni = null;
        double[] dsc_henni = null;
        double[] a = null;
        double[] b = null;

        public Form1()
        {
            InitializeComponent();

            test_load = new double[10];
            set_load = new double[10];
            load = new double[4000];
            henni = new double[4000];
            dsc_henni = new double[4000];
            a = new double[3];
            b = new double[3];

            SetLoad0.Text = "1640";
            SetLoad1.Text = "825";
            SetLoad2.Text = "413";
            SetLoad3.Text = "165";
            SetLoad4.Text = "83";
            SetLoad5.Text = "41";
        }

        public void sai1(int st, int n, double[] x, double[] y)
        {
            double A00, A01, A02, A11, A12;

            A00 = A01 = A02 = A11 = A12 = 0.0;

            for (int i = st; i < n; i++)
            {
                A00 += 1.0;
                A01 += x[i];
                A02 += y[i];
                A11 += x[i] * x[i];
                A12 += x[i] * y[i];
            }
            /*１次式の係数の計算*/
            A = (A02 * A11 - A01 * A12) / (A00 * A11 - A01 * A01);
            B = (A00 * A12 - A01 * A02) / (A00 * A11 - A01 * A01);

            /*   double p,q;
               p = modf(A/B,&q);
               int n=(int)(A/B);
               if(fabs(p)>0.5)n=abs(n)+1;
               else n=abs(n);

               return (n-1);
               */
        }

        private void LoadCalc_Click(object sender, EventArgs e)
        {
            test_load[0] = 1640;
            test_load[1] = 825;
            test_load[2] = 413;
            test_load[3] = 165;
            test_load[4] = 83;
            test_load[5] = 41;

            set_load[0] = double.Parse(SetLoad0.Text);
            set_load[1] = double.Parse(SetLoad1.Text);
            set_load[2] = double.Parse(SetLoad2.Text);
            set_load[3] = double.Parse(SetLoad3.Text);
            set_load[4] = double.Parse(SetLoad4.Text);
            set_load[5] = double.Parse(SetLoad5.Text);

            sai1(0,6,set_load, test_load);

            tb_b.Text = A.ToString("F6");
            tb_m.Text = B.ToString("F6");

            tb_error0.Text = (((set_load[0] * B + A) / test_load[0]) * 100 - 100).ToString("F2");
            tb_error1.Text = (((set_load[1] * B + A) / test_load[1]) * 100 - 100).ToString("F2");
            tb_error2.Text = (((set_load[2] * B + A) / test_load[2]) * 100 - 100).ToString("F2");
            tb_error3.Text = (((set_load[3] * B + A) / test_load[3]) * 100 - 100).ToString("F2");
            tb_error4.Text = (((set_load[4] * B + A) / test_load[4]) * 100 - 100).ToString("F2");
            tb_error5.Text = (((set_load[5] * B + A) / test_load[5]) * 100 - 100).ToString("F2");

        }

        private void btOpenDSC01_Click(object sender, EventArgs e)
        {
            int fnum = 0;
            

            OpenFileDialog ofd = new OpenFileDialog();

            //ofd.FileName = "default.csv";
            ofd.Multiselect = true;
            //ofd.InitialDirectory = @"C:\";
            //ofd.Filter = "csvファイル(*.csv;)|*.csv";
            //ofd.FilterIndex = 2;
            //ofd.Title = "開くファイルを選択してください";
            //ofd.RestoreDirectory = true;
            //ofd.CheckFileExists = true;
            //ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string fn in ofd.FileNames)
                {
                    dsc_file_name[fnum] = fn;// Path.GetFileName(fn);
                    richTextBox1.Text += (fnum.ToString()+"--->"+Path.GetFileName(fn));
                    richTextBox1.Text += "\n";
                    fnum++;
                }
            }
        }

        private void btOpenNexusFile_Click(object sender, EventArgs e)
        {
            int fnum = 0;
          

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Multiselect = true;

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                foreach(string fn in ofd.FileNames)
                {
                    nexus_file_name[fnum] = fn;// Path.GetFileName(fn);
                    richTextBox2.Text += (fnum.ToString()+"--->"+Path.GetFileName(fn));
                    richTextBox2.Text += "\n";
                    fnum++;
                }            
            }    
        }

        private void read_nexus_dsc(int num)
        {
            int ct = 0;

            System.IO.StreamReader sr = new StreamReader(nexus_file_name[num]);
            for (int j = 0; j < 66; j++)
                sr.ReadLine();

            while (sr.EndOfStream == false)
            {
                string line = sr.ReadLine();
                string[] fields = line.Split(',');

                try
                {
                    load[ct] = double.Parse(fields[1]);
                    henni[ct] = double.Parse(fields[2]);
                    //richTextBox3.Text += load[ct].ToString("F4") + "," + henni[ct].ToString("F4") + "\n";
                    ct++;
                }
                catch (FormatException)
                {
                }
                if (ct > 1100) break;

            }
            sr.Close();

            ct = 0;
            System.IO.StreamReader sr2 = new StreamReader(dsc_file_name[num]);
            for (int j = 0; j < 7; j++)
                sr2.ReadLine();

            while (sr2.EndOfStream == false)
            {
                try
                {
                    dsc_henni[ct] = double.Parse(sr2.ReadLine()) * 1000;

                    //richTextBox4.Text += dsc_henni[ct].ToString("F6") + "\n";
                    ct++;
                }
                catch (FormatException)
                {
                }
                if (ct > 1100) break;

            }
            sr2.Close();

           
        }
        private void btDispCalc_Click(object sender, EventArgs e)
        {

            chart1.Series.Clear();
            chart1.Series.Add("1");
            chart1.Series["1"].ChartType = SeriesChartType.Line;
            Axis ax = chart1.ChartAreas[0].AxisX;
            ax.MajorGrid.LineColor = Color.LightGray;
            ax.Maximum = 16000.0;
            ax.Interval = 2000;
            ax.Minimum = 0;
            ax.Title = "[変位(nm)]";
            Axis ay = chart1.ChartAreas[0].AxisY;
            ay.MajorGrid.LineColor = Color.LightGray;
            ay.Maximum = 100;
            ay.Interval = 10;
            ay.Minimum = -100;
            ay.Title = "残差[nm]";





            read_nexus_dsc(0);
            sai1(100, 900, henni, dsc_henni);
            a[0] = B;
            b[0] = A;

            tb_henni_b1.Text = A.ToString("F6");
            tb_henni_a1.Text = B.ToString("F6");

            for (int i = 100; i < 1000; i++)
            {
                chart1.Series["1"].Points.AddXY(dsc_henni[i] * a[0] + b[0], henni[i] - (dsc_henni[i] * a[0] + b[0]));
            }



            read_nexus_dsc(1);
            sai1(100, 900, henni, dsc_henni);
            a[1] = B;
            b[1] = A;

            tb_henni_b2.Text = A.ToString("F6");
            tb_henni_a2.Text = B.ToString("F6");
            read_nexus_dsc(2);
            sai1(100, 900, henni, dsc_henni);
            a[2] = B;
            b[2] = A;

            tb_henni_b3.Text = A.ToString("F6");
            tb_henni_a3.Text = B.ToString("F6");

        

            
           
        }
    }
}
