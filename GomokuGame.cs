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
    public partial class GomokuGame : Form
    {
        private Button[,] board;
        private int[,] boardState; // Trạng thái của bàn cờ, lưu ký hiệu của mỗi ô (-1: ô trống, 0: 'O', 1: 'X')
        private int currentPlayer; // Người chơi hiện tại (1 hoặc -1)
        private int size = 9; // Kích thước bàn cờ
        private int winCount = 5; // Số ô liên tiếp để chiến thắng

        public GomokuGame()
        {
            InitializeComponent();
            InitializeBoard();
        }
        private void InitializeBoard()
        {
            board = new Button[size, size];
            boardState = new int[size, size];
            currentPlayer = 1;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button btn = new Button();
                    btn.Size = new System.Drawing.Size(50, 50);
                    btn.Location = new System.Drawing.Point(50 * j, 50 * i);
                    btn.Tag = new Tuple<int, int>(i, j);
                    btn.Click += Button_Click;
                    this.Controls.Add(btn);
                    board[i, j] = btn;
                    boardState[i, j] = -1; // Ban đầu các ô đều trống
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Tuple<int, int> position = btn.Tag as Tuple<int, int>;
            int row = position.Item1;
            int col = position.Item2;

            if (boardState[row, col] != -1) // Ô đã được đánh
                return;

            if (currentPlayer == 1)
            {
                btn.BackgroundImage = Image.FromFile("C:\\Users\\admin\\source\\repos\\Caro\\Caro\\x_symbol.jpg");
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                boardState[row, col] = 1;
            }
            else
            {
                btn.BackgroundImage = Image.FromFile("C:\\Users\\admin\\source\\repos\\Caro\\Caro\\o_symbol.jpg");
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                boardState[row, col] = 0;
            }

            if (IsWinningMove(row, col))
            {
                string winner = (currentPlayer == 1) ? "Player X" : "Player O";
                DialogResult result = MessageBox.Show(winner + " wins!\n\nDo you want to play again?", "Game Over", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ResetGame();
                    return;
                }
                else
                {
                    this.Close();
                    return;
                }
            }


            // Kiểm tra hòa
            if (IsDraw())
            {
                MessageBox.Show("It's a draw!");
                ResetGame();
                return;
            }

            // Chuyển lượt đi sang người chơi khác
            currentPlayer = -currentPlayer;
        }

        private bool IsWinningMove(int row, int col)
        {
            int player = boardState[row, col];
            int count;

            // Kiểm tra hàng ngang
            count = 1;
            for (int i = col - 1; i >= 0 && boardState[row, i] == player; i--)
                count++;
            for (int i = col + 1; i < size && boardState[row, i] == player; i++)
                count++;
            if (count >= winCount)
                return true;

            // Kiểm tra hàng dọc
            count = 1;
            for (int i = row - 1; i >= 0 && boardState[i, col] == player; i--)
                count++;
            for (int i = row + 1; i < size && boardState[i, col] == player; i++)
                count++;
            if (count >= winCount)
                return true;

            // Kiểm tra đường chéo chính
            count = 1;
            for (int i = 1; i < winCount && row - i >= 0 && col - i >= 0 && boardState[row - i, col - i] == player; i++)
                count++;
            for (int i = 1; i < winCount && row + i < size && col + i < size && boardState[row + i, col + i] == player; i++)
                count++;
            if (count >= winCount)
                return true;

            // Kiểm tra đường chéo phụ
            count = 1;
            for (int i = 1; i < winCount && row - i >= 0 && col + i < size && boardState[row - i, col + i] == player; i++)
                count++;
            // Kiểm tra đường chéo phụ (tiếp tục)
            for (int i = 1; i < winCount && row + i < size && col - i >= 0 && boardState[row + i, col - i] == player; i++)
                count++;
            if (count >= winCount)
                return true;

            return false;
        }

        private bool IsDraw()
        {
            // Kiểm tra xem bàn cờ đã đầy chưa
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (boardState[i, j] == -1)
                        return false; // Còn ô trống
                }
            }
            return true; // Bàn cờ đã đầy
        }

        private void ResetGame()
        {
            // Đặt lại trạng thái của bàn cờ và các button
            foreach (Button btn in board)
            {
                btn.Text = "";
                btn.BackgroundImage = null; // Xóa hình ảnh của ô
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    boardState[i, j] = -1;
                }
            }

            // Đặt lại lượt đi cho người chơi đầu tiên
            currentPlayer = 1;
        }


    }
}
