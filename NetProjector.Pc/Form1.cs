using System;
using System.Drawing;
using System.Windows.Forms;

namespace NetProjector.Pc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void RefreshImage(Image image)
        {
            if (InvokeRequired)
                pbShow.Invoke(new Action<Image>(RefreshImage), image);
            else
                pbShow.Image = image;

        }

        //void AddReceivedText(string text)
        //{
        //    if (InvokeRequired)
        //        tbReceived.Invoke(new Action<string>(AddReceivedText), text);
        //    else
        //        tbReceived.AppendText(text + "\n");
        //}
    }
}
