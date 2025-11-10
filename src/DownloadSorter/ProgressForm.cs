using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DownloadsSorter
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public void UpdateProgress(int current, int total)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int, int>(UpdateProgress), current, total);
                return;
            }

            progressBar1.Maximum = total;
            progressBar1.Value = current;
            lblStatus.Text = $"Обработано {current} из {total} файлов";
        }
    }
}
