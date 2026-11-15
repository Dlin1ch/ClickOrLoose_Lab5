using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace ClickOrLoose
{
    public class CControllerEventArgs : EventArgs
    {
        public string msg = "";
        public Ellipse sprite;

        public CControllerEventArgs(Ellipse sprite)
        {
            this.sprite = sprite;
        }
    }
}
