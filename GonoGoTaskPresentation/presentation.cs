using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GonoGoTaskPresentation
{
    public partial class presentation : Form
    {

        /*****parameters*******/
        // diameter for crossing, circle, square
        int objdiameter = 200; 
        int rightgap = 30;
        Graphics g;

        // the go circle button
        ButtonEllipse circleGoButton;

        // tag for go or nogo button clicked
        bool goNogoButton_IsClicked;

        // randomized Go noGo tag list, tag_gonogo ==1: go, ==0: nogo
        List<int> taglist_gonogo = new List<int>();

        // the nogo square button
        ButtonRectangle squareNogoButton;

        // wait time for click button
        double twait_click = 5;

        /*****End of parameters*******/

        public presentation()
        {
            InitializeComponent();
        }


        private void PresentScreen_Load(object sender, EventArgs e)
        {
            
            //shuffle go and nogo trials
            int gotrialnum = ((main)Owner).gotrialnum;
            int nogotrialnum = ((main)Owner).nogotrialnum;
            shuffle_gonogotrials(gotrialnum, nogotrialnum);

            // create a new thread for taskpresent
            Thread t = new Thread(taskpresent);
            t.Start();

        }

        private void shuffle_gonogotrials(int gotrialnum, int nogotrialnum)
        {/* ---- shuffle go and nogo trials, present in member variable taglist_gonogo -----*/

            // create orderred gonogo list
            List<int> tmporder_go = new List<int>(Enumerable.Repeat(1, gotrialnum));
            List<int> tmporder_nogo = new List<int>(Enumerable.Repeat(0, nogotrialnum));
            List<int> tmporder_gonogo = tmporder_go.Concat(tmporder_nogo).ToList();


            // shuffle 
            Random r = new Random();
            int randomIndex;

            while (tmporder_gonogo.Count > 0)
            {
                // choose a random index in list tmporder_gonogo
                randomIndex = r.Next(0, tmporder_gonogo.Count);

                // add the selected value into tagarray_gonogo
                taglist_gonogo.Add(tmporder_gonogo[randomIndex]);

                //remove this value
                tmporder_gonogo.RemoveAt(randomIndex);
            }
        }

        public void taskpresent()
        {
            g = CreateGraphics();

            foreach (int tag_gonogo in taglist_gonogo)
            {
                // ready interface 
                interface_ready();
                randomwait(1, 3);

                // target cue interface
                interface_targetcue();
                randomwait(1, 3);

                interface_gonongo(tag_gonogo);
                DateTime startTime = DateTime.Now;

                // wait for click for twait_click seconds
                while (goNogoButton_IsClicked == false && (DateTime.Now - startTime).TotalSeconds < twait_click) ;
                
                // random wait for [1 3]s after clicked
                if (goNogoButton_IsClicked == true)
                    randomwait(1, 3);
            }

            interface_ready();
        }

        private void randomwait(int wait_tmin, int wait_tmax)
        {/* random wait [wait_tmin wait_tmax] seconds*/

            Random random = new Random();
            System.Threading.Thread.Sleep((int)((random.NextDouble() * (wait_tmax - wait_tmin) + wait_tmin) * 1000));
        }

        private void draw_onecrossing()
        {/*draw the crossing*/

            int crosslen = objdiameter;
            Pen whitePen = new Pen(Color.White, 3);

            // horizontal line
            Point A_h = new Point(Width - crosslen - rightgap, Height / 2);
            Point B_h = new Point(Width - rightgap, Height / 2);
            g.DrawLine(whitePen, A_h, B_h);

            // vertical line
            Point A_v = new Point(Width - crosslen / 2 - rightgap, Height / 2 - crosslen / 2);
            Point B_v = new Point(Width - crosslen / 2 - rightgap, Height / 2 + crosslen / 2);
            g.DrawLine(whitePen, A_v, B_v);

            whitePen.Dispose();
        }

        private void draw_twowhitepoints()
        {/* draw the two write circle */

            int leftgap = 30;
            int topgap = 20;
            int radius = 10;
            System.Drawing.SolidBrush whiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

            g.FillEllipse(whiteBrush, leftgap, Height / 2, radius, radius);
            g.FillEllipse(whiteBrush, Width / 2, topgap, radius, radius);

            whiteBrush.Dispose();
        }

        private void draw_circleGoButton()
        {/*draw the right blue circle Button*/

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(this.draw_circleGoButton));
            }
            else
            {
                // Create a Button object 
                circleGoButton = new ButtonEllipse();

                // radius of circle button
                int diameter = objdiameter;

                // Set Button properties
                circleGoButton.Location = new Point(Width - diameter - rightgap, Height / 2 - diameter / 2);
                circleGoButton.Height = diameter;
                circleGoButton.Width = diameter;

                // Set background and foreground
                circleGoButton.BackColor = Color.Blue;
                circleGoButton.Name = "circleGoButton";

                // Add a Button Click Event handler
                circleGoButton.Click += new EventHandler(circleGoButton_Click);

                // tag for clicked or not
                goNogoButton_IsClicked = false;

                this.Controls.Add(circleGoButton);

                circleGoButton.Update();
            }  

        }

        private void draw_squareNogoButton()
        {/*draw the red blue square Button*/

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(this.draw_squareNogoButton));
            }
            else
            {

                // Create a square button
                squareNogoButton = new ButtonRectangle();

                // radius of circle button
                int diameter = objdiameter;

                // Set Button properties
                squareNogoButton.Location = new Point(Width - diameter - rightgap, Height / 2 - diameter / 2);
                squareNogoButton.Height = diameter;
                squareNogoButton.Width = diameter;

                // Set background and foreground
                squareNogoButton.BackColor = Color.Red;
                squareNogoButton.Name = "squareNogoButton";


                // Add a Button Click Event handler
                squareNogoButton.Click += new EventHandler(rectNogoButton_Click);


                // Add Button to the Form. Placement of the Button
                // will be based on the Location and Size of button
                this.Controls.Add(squareNogoButton);

                squareNogoButton.Update();
            }

        }

        private void circleGoButton_Click(object sender, EventArgs e)
        {
            ButtonEllipse btn = sender as ButtonEllipse;
            btn.BackColor = Color.Yellow;
            circleGoButton.Update();

            goNogoButton_IsClicked = true;
        }

        private void rectNogoButton_Click(object sender, EventArgs e)
        {
            ButtonRectangle btn = sender as ButtonRectangle;
            btn.BackColor = Color.YellowGreen;
            squareNogoButton.Update();
        }


        private void interface_ready()
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(this.interface_ready));
            }
            else
            {
                g.Clear(Color.Black);
                if (circleGoButton != null)
                {
                    Controls.Remove(circleGoButton);
                    circleGoButton.Dispose();
                    circleGoButton = null;
                }

                if (squareNogoButton != null)
                {
                    Controls.Remove(squareNogoButton);
                    squareNogoButton.Dispose();
                    squareNogoButton = null;
                }
            }
        }

        private void interface_targetcue()
        {
            g.Clear(Color.Black);

            // one crossing on the right middle
            draw_onecrossing();

            // two white points on left middle and top middle
            draw_twowhitepoints();
        }

        private void interface_gonongo(int tag_gonogo)
        {

            g.Clear(Color.Black);

            // set go circle button or nogo square button on the right middle visible
            if (tag_gonogo == 1)
            {
                draw_circleGoButton(); 
            }

            else
            {
                draw_squareNogoButton();
            }

            

            // draw two white points on left middle and top middle
            draw_twowhitepoints();

        }

    }
}


/*object rect = myGrid.FindName("rectNogo");

                        if(rect is Rectangle)
                        {
                            Rectangle wanted = rect as Rectangle;
wanted.Fill = Brushes.Blue;
                        }*/