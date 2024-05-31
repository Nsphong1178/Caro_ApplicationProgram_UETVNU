
using Caro.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Tạo một thể hiện mới của Form1
            Choose choose = new Choose();

            // Hiển thị Form1
            choose.Show();

            // Đóng form hiện tại (trong trường hợp này là form Main)
            this.Hide();
        }

    }
}
