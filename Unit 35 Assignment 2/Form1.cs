﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Unit_35_Assignment_2
{
    public partial class Form1 : Form
    {
        class row
        {
            public double time;
            public double acceleration;
            public double velocity;
            public double altitude;
        }

        List<row> table = new List<row>();
        public Form1()
        {
            InitializeComponent();
        }

        // calculates the velocity 
        private void calculateVelocity()
        {
            for (int i=1; i < table.Count; i++)
            {
                double dh = table[i].altitude - table[i - 1].altitude;
                double dt = table[i].time - table[i - 1].time;
                table[i].velocity = dh / dt;
            }
        }
        // calculates the acceleration
        private void calculateAcceleration()
        {
            for (int i = 2; i < table.Count; i++)
            {
                double dv = table[i].velocity - table[i - 1].velocity;
                double dt = table[i].time - table[i - 1].time;
                table[i].acceleration = dv / dt;
            }
        }

        // allows you to open the data and reads the data that you open  
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "csv Files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line = sr.ReadLine();
                        while(!sr.EndOfStream)
                        {
                            table.Add(new row());
                            string[] r = sr.ReadLine().Split(',');
                            table.Last().time = double.Parse(r[0]);
                            table.Last().altitude = double.Parse(r[1]);
                        }
                    }
                    calculateVelocity();
                    calculateAcceleration();

                    // checks for any errors in the data
                }
                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName = " Failed to open.");
                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName = " Not in required format.");
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName = " Not in required format.");
                }
                catch (DivideByZeroException)
                {
                    MessageBox.Show(openFileDialog1.FileName = " has rows that have the same time.");
                }
            }
        }

        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void currentToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        // creates and displays the velocity graph 
        private void velocityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Velocity",
                Color = Color.Gold,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2

            };
            chart1.Series.Add(series);
            foreach(row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.velocity);
            }
            chart1.ChartAreas[0].AxisX.Title = "time /s";
            chart1.ChartAreas[0].AxisY.Title = "Velocity M/s";
            chart1.ChartAreas[0].RecalculateAxesScale();


        }

        // creates and displays the current graph

        private void currentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Acceleration",
                Color = Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2

            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.acceleration);
            }
            chart1.ChartAreas[0].AxisX.Title = "time /s";
            chart1.ChartAreas[0].AxisY.Title = "Acceleration M/s2";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        // creates and displays thr altitude graph
        private void altitudeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Altitude",
                Color = Color.Violet,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2

            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time,r.altitude);
            }
            chart1.ChartAreas[0].AxisX.Title = "time /s";
            chart1.ChartAreas[0].AxisY.Title = "Altiude /m";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        // saves the graph (updates a earlier save)
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "csv Files|*. csv";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        sw.WriteLine("Time /s, altitude m, velocity m/s, acceleration m/s2");
                        foreach (row r in table)
                        {
                            sw.WriteLine(r.time + "," + r.altitude + "," + r.velocity + "," + r.acceleration);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + " failed to save.");
                }
            }
        }

        // creates a new save, allowing you to save were you like.
        private void saveAsPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "png Files|*. png";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + "failed to save.");
                }
            }
        }
    }
}
