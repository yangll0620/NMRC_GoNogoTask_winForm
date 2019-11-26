using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace GonoGoTaskPresentation
{
    class ButtonEllipse : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            GraphicsPath graphics = new GraphicsPath();
            graphics.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(graphics);
            base.OnPaint(pevent);
        }
    }

    class ButtonRectangle : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            GraphicsPath graphics = new GraphicsPath();
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            graphics.AddRectangle(rect);
            this.Region = new System.Drawing.Region(graphics);
            base.OnPaint(pevent);
        }
    }
}
