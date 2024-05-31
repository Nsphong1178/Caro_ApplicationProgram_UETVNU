/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caro
{

    private void CreateBoard()
    {
        int size = 9;
        TableLayoutPanel board = new TableLayoutPanel
        {
            RowCount = size,
            ColumnCount = size,
            Dock = DockStyle.Fill
        };

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Button btn = new Button
                {
                    Dock = DockStyle.Fill,
                    Tag = i * size + j,
                    Text = "",
                    Enabled = true
                };
                btn.Click += Button_Click;
                board.Controls.Add(btn, j, i);
            }
        }

        this.Controls.Add(board);
    }

}
*/