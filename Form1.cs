using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Caro
{
    public partial class Form1 : Form
    {
        private HttpClient client;
        private Button[,] buttons;

        public Form1()
        {
            InitializeComponent();
            client = new HttpClient();
            CreateBoard();
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int row = (int)btn.Tag / 9;
            int col = (int)btn.Tag % 9;

            btn.Enabled = false;

            // Thực hiện nước đi của người chơi và cập nhật giao diện
            btn.BackgroundImage = Image.FromFile("C:\\Users\\admin\\source\\repos\\Caro\\Caro\\o_symbol.jpg"); // Đặt hình ảnh cho ô người chơi
            btn.BackgroundImageLayout = ImageLayout.Stretch;

            var response = await MakeMove(row, col);
            if (response != null)
            {
                int ai_row = response.ai_row;
                int ai_col = response.ai_col;

                // Thực hiện nước đi của AI và cập nhật giao diện
                buttons[ai_row, ai_col].BackgroundImage = Image.FromFile("C:\\Users\\admin\\source\\repos\\Caro\\Caro\\x_symbol.jpg"); // Đặt hình ảnh cho ô AI
                buttons[ai_row, ai_col].BackgroundImageLayout = ImageLayout.Stretch;
                buttons[ai_row, ai_col].Enabled = false;

                if (response.finished)
                {
                    MessageBox.Show("Game Over");
                }
            }
        }


        private async Task<dynamic> MakeMove(int row, int col)
        {
            var move = new { row = row, col = col };
            var json = JsonConvert.SerializeObject(move);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://127.0.0.1:5000/play", content);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject<dynamic>(responseString);
                // Check if the 'finished' field is true
                bool gameFinished = responseData.finished; // Explicit cast to bool
                return new { ai_row = responseData.ai_row, ai_col = responseData.ai_col, finished = gameFinished };
            }
            return null;
        }



        private void CreateBoard()
        {
            int size = 9;
            TableLayoutPanel board = new TableLayoutPanel
            {
                RowCount = size,
                ColumnCount = size,
                Dock = DockStyle.Fill
            };

            buttons = new Button[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button btn = new Button
                    {
                        Width = 50,  // Đặt chiều rộng là 100
                        Height = 50, // Đặt chiều cao là 100
                        Tag = i * size + j,
                        Text = "",
                        Enabled = true
                    };
                    btn.Click += Button_Click;
                    buttons[i, j] = btn;
                    board.Controls.Add(btn, j, i);
                }
            }

            this.Controls.Add(board);
        }

    }
}
