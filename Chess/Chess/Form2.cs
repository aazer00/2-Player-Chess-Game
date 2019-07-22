using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form2 : Form
    {
        PictureBox[,] cell = new PictureBox[8, 8];

        // black figures
        const int black_rook = 22;
        const int black_knight = 23;
        const int black_bishop = 24;
        const int black_quenn = 25;
        const int black_king = 26;
        const int black_pawn = 21;

        //white figures
        const int white_rook = 12;
        const int white_knight = 13;
        const int white_bishop = 14;
        const int white_quenn = 15;
        const int white_king = 16;
        const int white_pawn = 11;

        int white_king_i = 7;
        int white_king_j = 4;

        int black_king_i = 0;
        int black_king_j = 4;

        bool add_board = true;
        int empty = 0;

        int[,] figures;

        ArrayList white_king_road_i = new ArrayList();
        ArrayList white_king_road_j = new ArrayList();

        ArrayList black_king_road_i = new ArrayList();
        ArrayList black_king_road_j = new ArrayList();

        ArrayList black_legal_moves = new ArrayList();


        public Form2()
        {
            InitializeComponent();
            
            figures = new int[8, 8]
            {
                { black_rook, black_knight, black_bishop, black_quenn, black_king, black_bishop, black_knight, black_rook},
                //{ empty, empty, empty, empty, empty, empty, empty, empty},
                { black_pawn, black_pawn, black_pawn, black_pawn, black_pawn, black_pawn, black_pawn, black_pawn,},
                { empty, empty, empty, empty, empty, empty, empty, empty},
                { empty, empty, empty, empty, empty, empty, empty, empty },
                { empty, empty, empty, empty, empty, empty, empty, empty},
                { empty, empty, empty, empty, empty, empty, empty, empty},
                //{ empty, empty, empty, empty, empty, empty, empty, empty},
               // { empty, empty, empty, empty, white_king, empty, empty, empty},
                { white_pawn, white_pawn, white_pawn, white_pawn, white_pawn, white_pawn, white_pawn, white_pawn, },
                { white_rook, white_knight, white_bishop, white_quenn, white_king, white_bishop, white_knight, white_rook}
            };
            create_board();
            assing_figures();

        }

        private void create_board()
        {
            int x = 32;
            int y = 21;
            int side = 70;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PictureBox cl = new PictureBox();
                    if ((i + j) % 2 == 0)
                    {
                        cl.BackColor = Color.White;
                    }
                    else
                    {
                        cl.BackColor = Color.Gray;
                    }
                    cl.Size = new Size(side, side);
                    cl.Location = new Point(x, y);
                    int ni = i;
                    int nj = j;
                    // cl.Image = new Bitmap(@"white_pawn.png");
                    cl.Click += (sender, e) => get_board_number(sender, e, ni, nj);
                    cell[i, j] = cl;
                    this.Controls.Add(cell[i, j]);
                    x += side;

                }
                x = 32;
                y += side;

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        int selected = 0;
        int selected_row = -1;
        int selected_column = -1;
        int selected_figure = -1;
        Color last_color;
        public static int turn = 1;

        bool white_check = false;
        bool black_check = false;

        bool white_left_rook_move = false;
        bool white_right_rook_move = false;
        bool white_king_move = false;

        bool black_left_rook_move = false;
        bool black_right_rook_move = false;
        bool black_king_move = false;

        private void get_board_number(object sender, EventArgs e, int i, int j)
        {

            if (selected == 0 && figures[i, j] != empty)
            {
                
                    if (turn == 0)
                    {
                        if (figures[i, j] != empty)
                        {
                            selected_row = i;
                            selected_column = j;
                            selected = 1;
                            selected_figure = figures[i, j];
                            last_color = cell[i, j].BackColor;
                            cell[i, j].BackColor = Color.Yellow;

                        }
                    }
                    else
                    {
                        if (figures[i, j]/10 == turn)
                        {
                            selected_row = i;
                            selected_column = j;
                            selected = 1;
                            selected_figure = figures[i, j];
                            last_color = cell[i, j].BackColor;
                            cell[i, j].BackColor = Color.Yellow;
                            if (turn == 1) turn = 2;
                            else
                            if (turn == 2) turn = 1;

                        }
                    }
                    
                }
                        
            else if (selected == 1)
            {
                bool k = place_figure(figures[selected_row, selected_column], selected_row, selected_column, i, j);
                if (k == true)
                {
                    Image temp_im = cell[i, j].Image;
                    int temp_fi = figures[i, j];
                    cell[i, j].Image = cell[selected_row, selected_column].Image;
                    figures[i, j] = selected_figure;
                    figures[selected_row, selected_column] = empty;
                    cell[selected_row, selected_column].Image = null;
                    if (selected_figure == white_king)
                    {
                        white_king_i = i;
                        white_king_j = j;
                    }
                    if (selected_figure == black_king)
                    {
                        black_king_i = i;
                        black_king_j = j;
                    }
                    white_king_road_i.Clear();
                    white_king_road_j.Clear();
                    if (!check_white(white_king_i, white_king_j))
                    {
                       // MessageBox.Show("wcheck");

                        white_check = true;
                        add_board = false;
                        if (checkmate_white(white_king_i, white_king_j) == true)
                        {
                            Form3 form3 = new Form3();
                            Form3.text = "BLACK WINS!!!";
                            form3.ShowDialog();
                        } 
                        add_board = true;
                    }
                        
                    else white_check = false;
                    black_king_road_i.Clear();
                    black_king_road_j.Clear();
                    if (!check_black(black_king_i, black_king_j))
                    {
                       // MessageBox.Show("bcheck");
                        black_check = true;
                        add_board = false;
                        if (checkmate_black(black_king_i, black_king_j) == true)
                        {
                            Form3 form3 = new Form3();
                            Form3.text = "WHITE WINS!!!";
                            form3.ShowDialog();
                        } 
                        add_board = true;
                    }
                        
                    else black_check = false;
                    if (white_check == true && selected_figure / 10 == 1)
                    {
                        cell[selected_row, selected_column].Image = cell[i, j].Image;
                        figures[selected_row, selected_column] = figures[i, j];
                        cell[i, j].Image = temp_im;
                        figures[i, j] = temp_fi;
                        if (selected_figure == white_king)
                        {
                            white_king_i = selected_row;
                            white_king_j = selected_column;
                        }
                        if (selected_figure == black_king)
                        {
                            black_king_i = selected_row;
                            black_king_j = selected_column;
                        }
                        selected = 0;
                        cell[selected_row, selected_column].BackColor = last_color;
                        if (turn == 1) turn = 2;
                        else
                        if (turn == 2) turn = 1;
                    }
                    else if (black_check == true && selected_figure / 10 == 2)
                    {
                        cell[selected_row, selected_column].Image = cell[i, j].Image;
                        figures[selected_row, selected_column] = figures[i, j];
                        cell[i, j].Image = temp_im;
                        figures[i, j] = temp_fi;
                        if (selected_figure == white_king)
                        {
                            white_king_i = selected_row;
                            white_king_j = selected_column;
                        }
                        if (selected_figure == black_king)
                        {
                            black_king_i = selected_row;
                            black_king_j = selected_column;
                        }
                        selected = 0;
                        if (turn == 1) turn = 2;
                        else
                        if (turn == 2) turn = 1;
                        cell[selected_row, selected_column].BackColor = last_color;
                    }
                    else
                    {
                        if (figures[7, 0] == empty)
                        {
                            white_left_rook_move = true;
                        }
                        if (figures[7, 4] == empty)
                        {
                            white_king_move = true;
                        }
                        if (figures[7, 7] == empty)
                        {
                            white_right_rook_move = true;
                        }


                        if (figures[0, 0] == empty)
                        {
                            black_left_rook_move = true;
                        }
                        if (figures[0, 4] == empty)
                        {
                            black_king_move = true;
                        }
                        if (figures[0, 7] == empty)
                        {
                            black_right_rook_move = true;
                        }

                        char[] alpha = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
                        listBox1.Items.Add(alpha[selected_row] + selected_column.ToString() + " - " + alpha[i] + j.ToString());
                        selected = 0;
                        cell[selected_row, selected_column].BackColor = last_color;
                     
                    }

                    pawn_to_queen();
                }
                else
                {
                    selected = 0;
                    cell[selected_row, selected_column].BackColor = last_color;
                    if (turn == 1) turn = 2;
                    else
                    if (turn == 2) turn = 1;
                }



            }

      

        }

        private void assing_figures()
        {
            //pawns
            for (int i = 0; i < 8; i++)
            {
                cell[1, i].Image = new Bitmap(@"black_pawn.png");
                cell[6, i].Image = new Bitmap(@"white_pawn.png");
            }

            //rooks
            cell[0, 0].Image = new Bitmap(@"black_rook.png");
            cell[0, 7].Image = new Bitmap(@"black_rook.png");
            cell[7, 0].Image = new Bitmap(@"white_rook.png");
            cell[7, 7].Image = new Bitmap(@"white_rook.png");

            //knights
            cell[0, 1].Image = new Bitmap(@"black_knight.png");
            cell[0, 6].Image = new Bitmap(@"black_knight.png");
            cell[7, 1].Image = new Bitmap(@"white_knight.png");
            cell[7, 6].Image = new Bitmap(@"white_knight.png");

            //bishops
            cell[0, 2].Image = new Bitmap(@"black_bishop.png");
            cell[0, 5].Image = new Bitmap(@"black_bishop.png");
            cell[7, 2].Image = new Bitmap(@"white_bishop.png");
            cell[7, 5].Image = new Bitmap(@"white_bishop.png");

            //quenns
            cell[0, 3].Image = new Bitmap(@"black_quenn.png");
            cell[7, 3].Image = new Bitmap(@"white_quenn.png");

            //kings
            cell[0, 4].Image = new Bitmap(@"black_king.png");
            cell[7, 4].Image = new Bitmap(@"white_king.png");
        }

        private bool place_figure(int fig, int prev_i, int prev_j, int curr_i, int curr_j)
        {

            switch (fig)
            {
                case white_pawn:
                    {
                        if (prev_j == curr_j && prev_i > curr_i)
                        {
                            if (prev_i == 6 && (prev_i - curr_i) <= 2)
                            {
                                for (int i = prev_i - 1; i >= curr_i; i--)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                            else if ((prev_i - curr_i) == 1 && figures[curr_i, curr_j] == empty)
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (Math.Abs(prev_i - curr_i) == 1 && Math.Abs(prev_j - curr_j) == 1 && prev_i > curr_i)
                        {
                            if (figures[curr_i, curr_j] != empty && figures[curr_i, curr_j] / 10 != 1)
                            {
                                return true;
                            }
                            return false;
                        }
                        return false;
                    }
                    break;
                case black_pawn:
                    {
                        if (prev_j == curr_j && prev_i < curr_i)
                        {
                            if (prev_i == 1 && (curr_i - prev_i) <= 2)
                            {
                                for (int i = prev_i + 1; i <= curr_i; i++)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                            else if ((curr_i - prev_i) == 1 && figures[curr_i, curr_j] == empty)
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (Math.Abs(prev_i - curr_i) == 1 && Math.Abs(prev_j - curr_j) == 1 && prev_i < curr_i)
                        {
                            if (figures[curr_i, curr_j] != empty && figures[curr_i, curr_j] / 10 != 2)
                            {
                                return true;
                            }
                            return false;
                        }
                        return false;
                    }
                    break;

                case white_rook:
                    {
                        if (figures[curr_i, curr_j] / 10 == 1)
                        {
                            return false;
                        }
                        if ((Math.Abs(prev_i - curr_i) == 1 && Math.Abs(prev_j - curr_j) == 0) ||
                            (Math.Abs(prev_i - curr_i) == 0 && Math.Abs(prev_j - curr_j) == 1))
                        {
                            return true;
                        }
                        //sira uzre
                        if (prev_i == curr_i)
                        {
                            if (prev_j < curr_j)
                            {
                                for (int i = prev_j + 1; i < curr_j; i++)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }

                            if (prev_j > curr_j)
                            {
                                for (int i = prev_j - 1; i > curr_j; i--)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        //column uzre
                        else if (prev_j == curr_j)
                        {
                            if (prev_i < curr_i)
                            {
                                for (int i = prev_i + 1; i < curr_i; i++)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }

                            if (prev_i > curr_i)
                            {
                                for (int i = prev_i - 1; i > curr_i; i--)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        return false;
                    }
                    break;

                case black_rook:
                    {
                        if (figures[curr_i, curr_j] / 10 == 2)
                        {
                            return false;
                        }
                        if ((Math.Abs(prev_i - curr_i) == 1 && Math.Abs(prev_j - curr_j) == 0) ||
                            (Math.Abs(prev_i - curr_i) == 0 && Math.Abs(prev_j - curr_j) == 1))
                        {
                            return true;
                        }
                        //sira uzre
                        if (prev_i == curr_i)
                        {
                            if (prev_j < curr_j)
                            {
                                for (int i = prev_j + 1; i < curr_j; i++)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }

                            if (prev_j > curr_j)
                            {
                                for (int i = prev_j - 1; i > curr_j; i--)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        //column uzre
                        else if (prev_j == curr_j)
                        {
                            if (prev_i < curr_i)
                            {
                                for (int i = prev_i + 1; i < curr_i; i++)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }

                            if (prev_i > curr_i)
                            {
                                for (int i = prev_i - 1; i > curr_i; i--)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        return false;
                    }
                    break;

                case white_knight:
                    {
                        if (figures[curr_i, curr_j] / 10 == 1)
                        {
                            return false;
                        }
                        if (((Math.Abs(prev_i - curr_i) == 2) && (Math.Abs(prev_j - curr_j) == 1)) || ((Math.Abs(prev_i - curr_i) == 1) && (Math.Abs(prev_j - curr_j) == 2)))
                        {
                            return true;
                        }
                        return false;
                    }
                    break;

                case black_knight:
                    {
                        if (figures[curr_i, curr_j] / 10 == 2)
                        {
                            return false;
                        }
                        if (((Math.Abs(prev_i - curr_i) == 2) && (Math.Abs(prev_j - curr_j) == 1)) || ((Math.Abs(prev_i - curr_i) == 1) && (Math.Abs(prev_j - curr_j) == 2)))
                        {
                            return true;
                        }
                        return false;
                    }
                    break;
                case white_bishop:
                    {
                        if (figures[curr_i, curr_j] / 10 == 1)
                        {
                            return false;
                        }
                        if (Math.Abs(prev_i - curr_i) == Math.Abs(prev_j - curr_j))
                        {
                            int k = Math.Abs(prev_i - curr_i) - 1;
                            int start_i = prev_i;
                            int start_j = prev_j;
                            while (k != 0)
                            {
                                int unit_i = (curr_i - prev_i) / (Math.Abs(curr_i - prev_i));
                                int unit_j = (curr_j - prev_j) / (Math.Abs(curr_j - prev_j));

                                start_i += unit_i;
                                start_j += unit_j;

                                if (figures[start_i, start_j] != empty)
                                {
                                    return false;
                                }
                                k--;

                            }
                            return true;
                        }
                        return false;
                    }
                    break;

                case black_bishop:
                    {
                        if (figures[curr_i, curr_j] / 10 == 2)
                        {
                            return false;
                        }
                        if (Math.Abs(prev_i - curr_i) == Math.Abs(prev_j - curr_j))
                        {
                            int k = Math.Abs(prev_i - curr_i) - 1;
                            int start_i = prev_i;
                            int start_j = prev_j;
                            while (k != 0)
                            {
                                int unit_i = (curr_i - prev_i) / (Math.Abs(curr_i - prev_i));
                                int unit_j = (curr_j - prev_j) / (Math.Abs(curr_j - prev_j));

                                start_i += unit_i;
                                start_j += unit_j;

                                if (figures[start_i, start_j] != empty)
                                {
                                    return false;
                                }
                                k--;

                            }
                            return true;
                        }
                        return false;
                    }
                    break;

                case white_quenn:
                    {
                        if (figures[curr_i, curr_j] / 10 == 1)
                        {
                            return false;
                        }
                        if ((Math.Abs(prev_i - curr_i) == 1 && Math.Abs(prev_j - curr_j) == 0) ||
                            (Math.Abs(prev_i - curr_i) == 0 && Math.Abs(prev_j - curr_j) == 1))
                        {
                            return true;
                        }
                        //dioqanal uzre
                        if (Math.Abs(prev_i - curr_i) == Math.Abs(prev_j - curr_j))
                        {
                            int k = Math.Abs(prev_i - curr_i) - 1;
                            int start_i = prev_i;
                            int start_j = prev_j;
                            while (k != 0)
                            {
                                int unit_i = (curr_i - prev_i) / (Math.Abs(curr_i - prev_i));
                                int unit_j = (curr_j - prev_j) / (Math.Abs(curr_j - prev_j));

                                start_i += unit_i;
                                start_j += unit_j;

                                if (figures[start_i, start_j] != empty)
                                {
                                    return false;
                                }
                                k--;

                            }
                            return true;
                        }
                        // sira uzre
                        else if (prev_i == curr_i)
                        {
                            if (prev_j < curr_j)
                            {
                                for (int i = prev_j + 1; i < curr_j; i++)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }

                            if (prev_j > curr_j)
                            {
                                for (int i = prev_j - 1; i > curr_j; i--)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        //column uzre
                        else if (prev_j == curr_j)
                        {
                            if (prev_i < curr_i)
                            {
                                for (int i = prev_i + 1; i < curr_i; i++)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }

                            if (prev_i > curr_i)
                            {
                                for (int i = prev_i - 1; i > curr_i; i--)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        return false;
                    }
                    break;

                case black_quenn:
                    {
                        if (figures[curr_i, curr_j] / 10 == 2)
                        {
                            return false;
                        }
                        if ((Math.Abs(prev_i - curr_i) == 1 && Math.Abs(prev_j - curr_j) == 0) ||
                            (Math.Abs(prev_i - curr_i) == 0 && Math.Abs(prev_j - curr_j) == 1))
                        {
                            return true;
                        }
                        //dioqanal uzre
                        if (Math.Abs(prev_i - curr_i) == Math.Abs(prev_j - curr_j))
                        {
                            int k = Math.Abs(prev_i - curr_i) - 1;
                            int start_i = prev_i;
                            int start_j = prev_j;
                            while (k != 0)
                            {
                                int unit_i = (curr_i - prev_i) / (Math.Abs(curr_i - prev_i));
                                int unit_j = (curr_j - prev_j) / (Math.Abs(curr_j - prev_j));

                                start_i += unit_i;
                                start_j += unit_j;

                                if (figures[start_i, start_j] != empty)
                                {
                                    return false;
                                }
                                k--;

                            }
                            return true;
                        }
                        // sira uzre
                        else if (prev_i == curr_i)
                        {
                            if (prev_j < curr_j)
                            {
                                for (int i = prev_j + 1; i < curr_j; i++)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }

                            if (prev_j > curr_j)
                            {
                                for (int i = prev_j - 1; i > curr_j; i--)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        //column uzre
                        else if (prev_j == curr_j)
                        {
                            if (prev_i < curr_i)
                            {
                                for (int i = prev_i + 1; i < curr_i; i++)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }

                            if (prev_i > curr_i)
                            {
                                for (int i = prev_i - 1; i > curr_i; i--)
                                {
                                    if (figures[i, prev_j] != empty)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        return false;
                    }
                    break;

                case white_king:
                    {
                        if (figures[curr_i, curr_j] / 10 == 1)
                        {
                            return false;
                        }
                        if (!white_king_move&&!white_check)
                        {
                            if (!white_left_rook_move&&curr_i==7 && curr_j==2)
                            {
                                for (int i = prev_j - 1; i > 0; i--)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }

                                figures[7, 0] = empty;
                                
                                figures[7, 3] = white_rook;
                                cell[7, 3].Image = cell[7, 0].Image;
                                cell[7, 0].Image = null;
                                return true;
                            }

                            if (!white_right_rook_move && curr_i == 7 && curr_j == 6)
                            {
                                for (int i = prev_j + 1; i < 7; i++)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }

                                figures[7, 7] = empty;
                                
                                figures[7, 5] = white_rook;
                                cell[7, 5].Image = cell[7, 7].Image;
                                cell[7, 7].Image = null;
                                return true;
                            }
                        }
                        if (Math.Abs(prev_i - curr_i) <= 1 && Math.Abs(prev_j - curr_j) <= 1)
                        {
                            //pawn attacks king
                            if (curr_i > 0)
                            {
                                if (curr_j > 0)
                                {

                                    if (figures[curr_i - 1, curr_j - 1] == black_pawn)
                                    {
                                        return false;
                                    }

                                }

                                if (curr_j < 7)
                                {
                                    if (figures[curr_i - 1, curr_j + 1] == black_pawn)
                                    {
                                        return false;
                                    }

                                }
                            }

                            //knight attacks
                            if (curr_i - 1 >= 0 && curr_i - 1 <= 7 && curr_j - 2 >= 0 && curr_j - 2 <= 7)
                            {
                                if (figures[curr_i - 1, curr_j - 2] == black_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i - 2 >= 0 && curr_i - 2 <= 7 && curr_j - 1 >= 0 && curr_j - 1 <= 7)
                            {
                                if (figures[curr_i - 2, curr_j - 1] == black_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i - 2 >= 0 && curr_i - 2 <= 7 && curr_j + 1 >= 0 && curr_j + 1 <= 7)
                            {
                                if (figures[curr_i - 2, curr_j + 1] == black_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i - 1 >= 0 && curr_i - 1 <= 7 && curr_j + 2 >= 0 && curr_j + 2 <= 7)
                            {
                                if (figures[curr_i - 1, curr_j + 2] == black_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i + 1 >= 0 && curr_i + 1 <= 7 && curr_j - 2 >= 0 && curr_j - 2 <= 7)
                            {
                                if (figures[curr_i + 1, curr_j - 2] == black_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i + 2 >= 0 && curr_i + 2 <= 7 && curr_j - 1 >= 0 && curr_j - 1 <= 7)
                            {
                                if (figures[curr_i + 2, curr_j - 1] == black_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i + 2 >= 0 && curr_i + 2 <= 7 && curr_j + 1 >= 0 && curr_j + 1 <= 7)
                            {
                                if (figures[curr_i + 2, curr_j + 1] == black_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i + 1 >= 0 && curr_i + 1 <= 7 && curr_j + 2 >= 0 && curr_j + 2 <= 7)
                            {
                                if (figures[curr_i + 1, curr_j + 2] == black_knight)
                                {
                                    return false;
                                }
                            }


                            //dioganal attacks (bishop and quenn)

                            if (check_dioganal(curr_i, curr_j, -1, -1, black_bishop, black_quenn, black_king) == false) return false;
                            if (check_dioganal(curr_i, curr_j, -1, 1, black_bishop, black_quenn, black_king) == false) return false;
                            if (check_dioganal(curr_i, curr_j, 1, -1, black_bishop, black_quenn, black_king) == false) return false;
                            if (check_dioganal(curr_i, curr_j, 1, 1, black_bishop, black_quenn, black_king) == false) return false;


                            //straight attacks (rook and quenn)

                            if (check_straight(curr_i, curr_j, -1, 0, black_rook, black_quenn, black_king) == false) return false;
                            if (check_straight(curr_i, curr_j, 0, -1, black_rook, black_quenn, black_king) == false) return false;
                            if (check_straight(curr_i, curr_j, 0, 1, black_rook, black_quenn, black_king) == false) return false;
                            if (check_straight(curr_i, curr_j, 1, 0, black_rook, black_quenn, black_king) == false) return false;



                            return true;
                        }
                        return false;
                    }
                    break;

                case black_king:
                    {
                        if (figures[curr_i, curr_j] / 10 == 2)
                        {
                            return false;
                        }
                        if (!black_king_move && !black_check)
                        {
                            if (!black_left_rook_move && curr_i == 0 && curr_j == 2)
                            {
                                for (int i = prev_j - 1; i > 0; i--)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }

                                figures[0, 0] = empty;

                                figures[0, 3] = black_rook;
                                cell[0, 3].Image = cell[0, 0].Image;
                                cell[0, 0].Image = null;
                                return true;
                            }

                            if (!black_right_rook_move && curr_i == 0 && curr_j == 6)
                            {
                                for (int i = prev_j + 1; i < 7; i++)
                                {
                                    if (figures[prev_i, i] != empty)
                                    {
                                        return false;
                                    }
                                }

                                figures[0, 7] = empty;

                                figures[0, 5] = black_rook;
                                cell[0, 5].Image = cell[0, 7].Image;
                                cell[0, 7].Image = null;
                                return true;
                            }
                        }
                        if (Math.Abs(prev_i - curr_i) <= 1 && Math.Abs(prev_j - curr_j) <= 1)
                        {

                            //pawn attacks king
                            if (curr_i < 7)
                            {
                                if (curr_j > 0)
                                {

                                    if (figures[curr_i + 1, curr_j - 1] == white_pawn)
                                    {
                                        return false;
                                    }

                                }

                                if (curr_j < 7)
                                {
                                    if (figures[curr_i + 1, curr_j + 1] == white_pawn)
                                    {
                                        return false;
                                    }

                                }
                            }

                            //knight attacks
                            if (curr_i - 1 >= 0 && curr_i - 1 <= 7 && curr_j - 2 >= 0 && curr_j - 2 <= 7)
                            {
                                if (figures[curr_i - 1, curr_j - 2] == white_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i - 2 >= 0 && curr_i - 2 <= 7 && curr_j - 1 >= 0 && curr_j - 1 <= 7)
                            {
                                if (figures[curr_i - 2, curr_j - 1] == white_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i - 2 >= 0 && curr_i - 2 <= 7 && curr_j + 1 >= 0 && curr_j + 1 <= 7)
                            {
                                if (figures[curr_i - 2, curr_j + 1] == white_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i - 1 >= 0 && curr_i - 1 <= 7 && curr_j + 2 >= 0 && curr_j + 2 <= 7)
                            {
                                if (figures[curr_i - 1, curr_j + 2] == white_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i + 1 >= 0 && curr_i + 1 <= 7 && curr_j - 2 >= 0 && curr_j - 2 <= 7)
                            {
                                if (figures[curr_i + 1, curr_j - 2] == white_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i + 2 >= 0 && curr_i + 2 <= 7 && curr_j - 1 >= 0 && curr_j - 1 <= 7)
                            {
                                if (figures[curr_i + 2, curr_j - 1] == white_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i + 2 >= 0 && curr_i + 2 <= 7 && curr_j + 1 >= 0 && curr_j + 1 <= 7)
                            {
                                if (figures[curr_i + 2, curr_j + 1] == white_knight)
                                {
                                    return false;
                                }
                            }


                            if (curr_i + 1 >= 0 && curr_i + 1 <= 7 && curr_j + 2 >= 0 && curr_j + 2 <= 7)
                            {
                                if (figures[curr_i + 1, curr_j + 2] == white_knight)
                                {
                                    return false;
                                }
                            }


                            //dioganal attacks (bishop and quenn)

                            if (check_dioganal(curr_i, curr_j, -1, -1, white_bishop, white_quenn, white_king) == false) return false;
                            if (check_dioganal(curr_i, curr_j, -1, 1, white_bishop, white_quenn, white_king) == false) return false;
                            if (check_dioganal(curr_i, curr_j, 1, -1, white_bishop, white_quenn, white_king) == false) return false;
                            if (check_dioganal(curr_i, curr_j, 1, 1, white_bishop, white_quenn, white_king) == false) return false;


                            //straight attacks (rook and quenn)

                            if (check_straight(curr_i, curr_j, -1, 0, white_rook, white_quenn, white_king) == false) return false;
                            if (check_straight(curr_i, curr_j, 0, -1, white_rook, white_quenn, white_king) == false) return false;
                            if (check_straight(curr_i, curr_j, 0, 1, white_rook, white_quenn, white_king) == false) return false;
                            if (check_straight(curr_i, curr_j, 1, 0, white_rook, white_quenn, white_king) == false) return false;

                            return true;
                        }
                        return false;
                    }
                    break;
            }

            return false;
        }

        private bool check_dioganal(int c_i, int c_j, int ui, int uj, int bp, int qun, int king)
        {
            ArrayList cl_i = new ArrayList();
            ArrayList cl_j = new ArrayList();
            c_i += ui;
            c_j += uj;

            if (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
                if (figures[c_i, c_j] == king)
                {
                    return false;
                }
            while (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
            {
                cl_i.Add(c_i);
                cl_j.Add(c_j);
                if (figures[c_i, c_j] == bp || figures[c_i, c_j] == qun)
                {
                    if(king == black_king)
                    add_white_road(cl_i, cl_j);
                    else
                    add_black_road(cl_i, cl_j);

                    return false;
                }
                else if (figures[c_i, c_j] != empty)
                {
                    return true;
                }

                c_i += ui;
                c_j += uj;
            }

            return true;
        }
        private bool check_straight(int c_i, int c_j, int ui, int uj, int rk, int qun, int king)
        {
            ArrayList cl_i = new ArrayList();
            ArrayList cl_j = new ArrayList();
            c_i += ui;
            c_j += uj;
            if (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
                if (figures[c_i, c_j] == king)
                {
                    return false;
                }
            while (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
            {
                cl_i.Add(c_i);
                cl_j.Add(c_j);
                if (figures[c_i, c_j] == rk || figures[c_i, c_j] == qun)
                {
                    if (king == black_king)
                        add_white_road(cl_i, cl_j);
                    else
                        add_black_road(cl_i, cl_j);
                    return false;
                }
                else if (figures[c_i, c_j] != empty)
                {
                    return true;
                }

                c_i += ui;
                c_j += uj;
            }

            return true;
        }

        private bool check_white(int curr_i, int curr_j)
        {
            bool res = true;
            
            if (!(curr_i >= 0 && curr_i <= 7 && curr_j >= 0 && curr_j <= 7)) return false;
            if (figures[curr_i, curr_j] / 10 == 1 && add_board == false) return false;
            // MessageBox.Show("che");
            if (curr_i > 0)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();

                if (curr_j > 0)
                {

                    if (figures[curr_i - 1, curr_j - 1] == black_pawn)
                    {
                        cl_i.Add(curr_i - 1);
                        cl_j.Add(curr_j - 1);
                        add_white_road(cl_i, cl_j);
                        res = false;
                    }

                }

                if (curr_j < 7)
                {
                    if (figures[curr_i - 1, curr_j + 1] == black_pawn)
                    {
                        cl_i.Add(curr_i - 1);
                        cl_j.Add(curr_j + 1);
                        add_white_road(cl_i, cl_j);
                        res = false;
                    }

                }


            }

            //knight attacks
            if (curr_i - 1 >= 0 && curr_i - 1 <= 7 && curr_j - 2 >= 0 && curr_j - 2 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i - 1, curr_j - 2] == black_knight)
                {
                    cl_i.Add(curr_i - 1);
                    cl_j.Add(curr_j - 2);
                    add_white_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i - 2 >= 0 && curr_i - 2 <= 7 && curr_j - 1 >= 0 && curr_j - 1 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i - 2, curr_j - 1] == black_knight)
                {
                    cl_i.Add(curr_i - 2);
                    cl_j.Add(curr_j - 1);
                    add_white_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i - 2 >= 0 && curr_i - 2 <= 7 && curr_j + 1 >= 0 && curr_j + 1 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i - 2, curr_j + 1] == black_knight)
                {
                    cl_i.Add(curr_i - 2);
                    cl_j.Add(curr_j + 1);
                    add_white_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i - 1 >= 0 && curr_i - 1 <= 7 && curr_j + 2 >= 0 && curr_j + 2 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i - 1, curr_j + 2] == black_knight)
                {
                    cl_i.Add(curr_i - 1);
                    cl_j.Add(curr_j + 2);
                    add_white_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i + 1 >= 0 && curr_i + 1 <= 7 && curr_j - 2 >= 0 && curr_j - 2 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i + 1, curr_j - 2] == black_knight)
                {
                    cl_i.Add(curr_i + 1);
                    cl_j.Add(curr_j - 2);
                    add_white_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i + 2 >= 0 && curr_i + 2 <= 7 && curr_j - 1 >= 0 && curr_j - 1 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i + 2, curr_j - 1] == black_knight)
                {
                    cl_i.Add(curr_i + 2);
                    cl_j.Add(curr_j - 1);
                    add_white_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i + 2 >= 0 && curr_i + 2 <= 7 && curr_j + 1 >= 0 && curr_j + 1 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i + 2, curr_j + 1] == black_knight)
                {
                    cl_i.Add(curr_i + 2);
                    cl_j.Add(curr_j + 1);
                    add_white_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i + 1 >= 0 && curr_i + 1 <= 7 && curr_j + 2 >= 0 && curr_j + 2 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i + 1, curr_j + 2] == black_knight)
                {
                    cl_i.Add(curr_i + 1);
                    cl_j.Add(curr_j + 2);
                    add_white_road(cl_i, cl_j);
                    res = false;
                }
            }


            //dioganal attacks (bishop and quenn)

            if (check_dioganal(curr_i, curr_j, -1, -1, black_bishop, black_quenn, black_king) == false) res = false; 
            if (check_dioganal(curr_i, curr_j, -1, 1, black_bishop, black_quenn, black_king) == false) res = false;
            if (check_dioganal(curr_i, curr_j, 1, -1, black_bishop, black_quenn, black_king) == false) res = false;
            if (check_dioganal(curr_i, curr_j, 1, 1, black_bishop, black_quenn, black_king) == false) res = false;


            //straight attacks (rook and quenn)

            if (check_straight(curr_i, curr_j, -1, 0, black_rook, black_quenn, black_king) == false) res = false;
            if (check_straight(curr_i, curr_j, 0, -1, black_rook, black_quenn, black_king) == false) res = false;
            if (check_straight(curr_i, curr_j, 0, 1, black_rook, black_quenn, black_king) == false) res = false;
            if (check_straight(curr_i, curr_j, 1, 0, black_rook, black_quenn, black_king) == false) res = false;



            return res;
        }

        private bool check_black(int curr_i, int curr_j)
        {
            bool res = true;

            
            if (!(curr_i >= 0 && curr_i <= 7 && curr_j >= 0 && curr_j <= 7)) return false;
            if (figures[curr_i, curr_j] / 10 == 2 && add_board == false) return false;
            // MessageBox.Show("che");
            //MessageBox.Show(add_board.ToString());
            if (curr_i > 0)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();

                if (curr_j > 0)
                {

                    if (figures[curr_i + 1, curr_j - 1] == white_pawn)
                    {
                        cl_i.Add(curr_i + 1);
                        cl_j.Add(curr_j - 1);
                        add_black_road(cl_i, cl_j);
                        res = false;
                    }

                }

                if (curr_j < 7)
                {
                    if (figures[curr_i + 1, curr_j + 1] == white_pawn)
                    {
                        cl_i.Add(curr_i - 1);
                        cl_j.Add(curr_j + 1);
                        add_black_road(cl_i, cl_j);
                        res = false;
                    }

                }


            }

            //knight attacks
            if (curr_i - 1 >= 0 && curr_i - 1 <= 7 && curr_j - 2 >= 0 && curr_j - 2 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i - 1, curr_j - 2] == white_knight)
                {
                    cl_i.Add(curr_i - 1);
                    cl_j.Add(curr_j - 2);
                    add_black_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i - 2 >= 0 && curr_i - 2 <= 7 && curr_j - 1 >= 0 && curr_j - 1 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i - 2, curr_j - 1] == white_knight)
                {
                    cl_i.Add(curr_i - 2);
                    cl_j.Add(curr_j - 1);
                    add_black_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i - 2 >= 0 && curr_i - 2 <= 7 && curr_j + 1 >= 0 && curr_j + 1 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i - 2, curr_j + 1] == white_knight)
                {
                    cl_i.Add(curr_i - 2);
                    cl_j.Add(curr_j + 1);
                    add_black_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i - 1 >= 0 && curr_i - 1 <= 7 && curr_j + 2 >= 0 && curr_j + 2 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i - 1, curr_j + 2] == white_knight)
                {
                    cl_i.Add(curr_i - 1);
                    cl_j.Add(curr_j + 2);
                    add_black_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i + 1 >= 0 && curr_i + 1 <= 7 && curr_j - 2 >= 0 && curr_j - 2 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i + 1, curr_j - 2] == white_knight)
                {
                    cl_i.Add(curr_i + 1);
                    cl_j.Add(curr_j - 2);
                    add_black_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i + 2 >= 0 && curr_i + 2 <= 7 && curr_j - 1 >= 0 && curr_j - 1 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i + 2, curr_j - 1] == white_knight)
                {
                    cl_i.Add(curr_i + 2);
                    cl_j.Add(curr_j - 1);
                    add_black_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i + 2 >= 0 && curr_i + 2 <= 7 && curr_j + 1 >= 0 && curr_j + 1 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i + 2, curr_j + 1] == white_knight)
                {
                    cl_i.Add(curr_i + 2);
                    cl_j.Add(curr_j + 1);
                    add_black_road(cl_i, cl_j);
                    res = false;
                }
            }


            if (curr_i + 1 >= 0 && curr_i + 1 <= 7 && curr_j + 2 >= 0 && curr_j + 2 <= 7)
            {
                ArrayList cl_i = new ArrayList();
                ArrayList cl_j = new ArrayList();
                if (figures[curr_i + 1, curr_j + 2] == white_knight)
                {
                    cl_i.Add(curr_i + 1);
                    cl_j.Add(curr_j + 2);
                    add_black_road(cl_i, cl_j);
                    res = false;
                }
            }


            //dioganal attacks (bishop and quenn)

            if (check_dioganal(curr_i, curr_j, -1, -1, white_bishop, white_quenn, white_king) == false) res = false;
            if (check_dioganal(curr_i, curr_j, -1, 1, white_bishop, white_quenn, white_king) == false) res = false;
            if (check_dioganal(curr_i, curr_j, 1, -1, white_bishop, white_quenn, white_king) == false) res = false;
            if (check_dioganal(curr_i, curr_j, 1, 1, white_bishop, white_quenn, white_king) == false) res = false;


            //straight attacks (rook and quenn)

            if (check_straight(curr_i, curr_j, -1, 0, white_rook, white_quenn, white_king) == false) res = false;
            if (check_straight(curr_i, curr_j, 0, -1, white_rook, white_quenn, white_king) == false) res = false;
            if (check_straight(curr_i, curr_j, 0, 1, white_rook, white_quenn, white_king) == false) res = false;
            if (check_straight(curr_i, curr_j, 1, 0, white_rook, white_quenn, white_king) == false) res = false;

            return res;
        }

        private void add_white_road(ArrayList cells_i, ArrayList cells_j)
        {
            if(add_board==true)
            for (int i = 0; i < cells_i.Count; i++)
            {
                
                white_king_road_i.Add(cells_i[i]);
                white_king_road_j.Add(cells_j[i]);
            }
        }

        private void add_black_road(ArrayList cells_i, ArrayList cells_j)
        {
           // MessageBox.Show("roar");
            if (add_board == true)
                for (int i = 0; i < cells_i.Count; i++)
                {
                    
                    black_king_road_i.Add(cells_i[i]);
                    black_king_road_j.Add(cells_j[i]);
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           /* listBox1.Items.Clear();
            for (int i = 0; i < 8; i++)
            {
                string s = "";
                for (int j = 0; j < 8; j++)
                {
                    s += figures[i, j].ToString().PadLeft(25) + " ";
                }
                listBox1.Items.Add(s);
            }*/
        }

        private bool checkmate_white(int white_king_i, int white_king_j)
        {
            if (check_white(white_king_i - 1, white_king_j - 1) == true) return false;
            if (check_white(white_king_i - 1, white_king_j) == true) return false;
            if (check_white(white_king_i - 1, white_king_j + 1) == true) return false;

            if (check_white(white_king_i, white_king_j - 1) == true) return false;
  
            if (check_white(white_king_i, white_king_j + 1) == true) return false;

            if (check_white(white_king_i + 1, white_king_j - 1) == true) return false;
            if (check_white(white_king_i + 1, white_king_j) == true) return false;
            if (check_white(white_king_i + 1, white_king_j + 1) == true) return false;
           // MessageBox.Show("che1");
            for (int h = 0; h < 8; h++)
            {

                for (int k = 0; k < 8; k++)
                {
                    int prev_i = h;
                    int prev_j = k;
                    //MessageBox.Show(h.ToString() + " " + k.ToString());
                    switch (figures[h, k])
                    {

                        case white_pawn:
                            {
                                //MessageBox.Show("pawn");
                                if (prev_i == 6)
                                {
                                    for (int i = prev_i - 1; i >= 4; i--)
                                    {
                                        for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                        {
                                            if (i == Convert.ToInt32(white_king_road_i[idx]) && prev_j == Convert.ToInt32(white_king_road_j[idx])&&figures[i,prev_j]==empty)
                                            {
                                                return false;
                                            }
                                            if (prev_j > 0)
                                            {

                                                if (figures[prev_i - 1, prev_j - 1] / 10 == 2 && prev_i - 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 1 == Convert.ToInt32(white_king_road_j[idx]))
                                                {
                                                    return false;
                                                }

                                            }

                                            if (prev_j < 7)
                                            {
                                                if (figures[prev_i - 1, prev_j + 1] / 10 == 2 && prev_i - 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 1 == Convert.ToInt32(white_king_road_j[idx]))
                                                {
                                                    return false;
                                                }

                                            }

                                        }

                                    }

                                }
                                else
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 1  == Convert.ToInt32(white_king_road_i[idx]) && prev_j == Convert.ToInt32(white_king_road_j[idx]) && figures[prev_i-1, prev_j] == empty)
                                        {
                                            return false;
                                        }
                                        if (prev_j > 0)
                                        {

                                            if (figures[prev_i - 1, prev_j - 1] / 10 == 2 && prev_i - 1 == Convert.ToInt32(black_king_road_i[idx]) && prev_j - 1 == Convert.ToInt32(white_king_road_j[idx]))
                                            {
                                                return false;
                                            }

                                        }

                                        if (prev_j < 7)
                                        {
                                            if (figures[prev_i - 1, prev_j + 1] / 10 == 2 && prev_i - 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 1 == Convert.ToInt32(white_king_road_j[idx]))
                                            {
                                                return false;
                                            }

                                        }

                                    }

                                }

                               
                            }
                            break;

                        case white_rook:
                            {
                               // MessageBox.Show("che2");
                                if (white_checkmate_straight(prev_i, prev_j, -1, 0) == false) return false;
                                if (white_checkmate_straight(prev_i, prev_j, 0, -1) == false) return false;
                                if (white_checkmate_straight(prev_i, prev_j, 0, 1) == false) return false;
                                if (white_checkmate_straight(prev_i, prev_j, 1, 0) == false) return false;
                            }

                            break;

                        case white_knight:
                            {
                               // MessageBox.Show("che3");

                                if (prev_i - 1 >= 0 && prev_i - 1 <= 7 && prev_j - 2 >= 0 && prev_j - 2 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 2 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                            //MessageBox.Show("che31");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i - 2 >= 0 && prev_i - 2 <= 7 && prev_j - 1 >= 0 && prev_j - 1 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 2 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 1 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                            //MessageBox.Show("che32");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i - 2 >= 0 && prev_i - 2 <= 7 && prev_j + 1 >= 0 && prev_j + 1 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 2 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 1 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                            //MessageBox.Show("che33");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i - 1 >= 0 && prev_i - 1 <= 7 && prev_j + 2 >= 0 && prev_j + 2 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 2 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                            //MessageBox.Show("che34");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i + 1 >= 0 && prev_i + 1 <= 7 && prev_j - 2 >= 0 && prev_j - 2 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 2 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                            //MessageBox.Show("che35");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i + 2 >= 0 && prev_i + 2 <= 7 && prev_j - 1 >= 0 && prev_j - 1 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 2 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 1 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                            //MessageBox.Show("che36");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i + 2 >= 0 && prev_i + 2 <= 7 && prev_j + 1 >= 0 && prev_j + 1 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 2 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 1 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                           // MessageBox.Show("che37");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i + 1 >= 0 && prev_i + 1 <= 7 && prev_j + 2 >= 0 && prev_j + 2 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 2 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                           // MessageBox.Show("che38");
                                            return false;
                                        }

                                    }
                                }
                            }
                            break;

                        case white_bishop:
                            {
                                //MessageBox.Show("che4");
                                if (white_checkmate_dioganal(prev_i, prev_j, -1, -1) == false) return false;
                                if (white_checkmate_dioganal(prev_i, prev_j, -1, 1) == false) return false;
                                if (white_checkmate_dioganal(prev_i, prev_j, 1, -1) == false) return false;
                                if (white_checkmate_dioganal(prev_i, prev_j, 1, 1) == false) return false;
                              
                            }
                            break;

                        case white_quenn:
                            {
                               // MessageBox.Show("che5");
                                if (white_checkmate_dioganal(prev_i, prev_j, -1, -1) == false) return false;
                                if (white_checkmate_dioganal(prev_i, prev_j, -1, 1) == false) return false;
                                if (white_checkmate_dioganal(prev_i, prev_j, 1, -1) == false) return false;
                                if (white_checkmate_dioganal(prev_i, prev_j, 1, 1) == false) return false;

                                //MessageBox.Show("che6");
                                if (white_checkmate_straight(prev_i, prev_j, -1, 0) == false) return false;
                                if (white_checkmate_straight(prev_i, prev_j, 0, -1) == false) return false;
                                if (white_checkmate_straight(prev_i, prev_j, 0, 1) == false) return false;
                                if (white_checkmate_straight(prev_i, prev_j, 1, 0) == false) return false;
                                //MessageBox.Show("che7");
                            }
                            break;

                        
                    }

                    
                }
            }
            return true;
        }

        private bool checkmate_black(int white_king_i, int white_king_j)
        {
            if (check_black(white_king_i - 1, white_king_j - 1) == true) return false;
            if (check_black(white_king_i - 1, white_king_j) == true) return false;
            if (check_black(white_king_i - 1, white_king_j + 1) == true) return false;

            if (check_black(white_king_i, white_king_j - 1) == true) return false;

            if (check_black(white_king_i, white_king_j + 1) == true) return false;

            if (check_black(white_king_i + 1, white_king_j - 1) == true) return false;
            if (check_black(white_king_i + 1, white_king_j) == true) return false;
            if (check_black(white_king_i + 1, white_king_j + 1) == true) return false;
           // MessageBox.Show("che1");
            for (int h = 0; h < 8; h++)
            {

                for (int k = 0; k < 8; k++)
                {
                    int prev_i = h;
                    int prev_j = k;
                    //MessageBox.Show(h.ToString() + " " + k.ToString());
                    switch (figures[h, k])
                    {

                        case black_pawn:
                            {
                                //MessageBox.Show("pawn");
                                if (prev_i == 1)
                                {
                                    for (int i = prev_i + 1; i <= 3; i++)
                                    {
                                        for (int idx = 0; idx < black_king_road_i.Count; idx++)
                                        {
                                            if (i == Convert.ToInt32(black_king_road_i[idx]) && prev_j == Convert.ToInt32(black_king_road_j[idx])&& figures[i, prev_j] == empty)
                                            {
                                                return false;
                                            }
                                            if (prev_j > 0)
                                            {

                                                if (figures[prev_i + 1, prev_j - 1] / 10 == 1&& prev_i+1 == Convert.ToInt32(black_king_road_i[idx]) && prev_j-1 == Convert.ToInt32(black_king_road_j[idx]))
                                                {
                                                    return false;
                                                }

                                            }

                                            if (prev_j < 7)
                                            {
                                                if (figures[prev_i + 1, prev_j + 1] / 10 == 1 && prev_i + 1 == Convert.ToInt32(black_king_road_i[idx]) && prev_j + 1 == Convert.ToInt32(black_king_road_j[idx]))
                                                {
                                                    return false;
                                                }

                                            }

                                        }

                                    }

                                }
                                else
                                {
                                    for (int idx = 0; idx < black_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 1 == Convert.ToInt32(black_king_road_i[idx]) && prev_j == Convert.ToInt32(black_king_road_j[idx])&& figures[prev_i+1, prev_j] == empty)
                                        {
                                            return false;
                                        }
                                        if (prev_j > 0)
                                        {

                                            if (figures[prev_i + 1, prev_j - 1] / 10 == 1 && prev_i + 1 == Convert.ToInt32(black_king_road_i[idx]) && prev_j - 1 == Convert.ToInt32(black_king_road_j[idx]))
                                            {
                                                return false;
                                            }

                                        }

                                        if (prev_j < 7)
                                        {
                                            if (figures[prev_i + 1, prev_j + 1] / 10 == 1 && prev_i + 1 == Convert.ToInt32(black_king_road_i[idx]) && prev_j + 1 == Convert.ToInt32(black_king_road_j[idx]))
                                            {
                                                return false;
                                            }

                                        }

                                    }

                                }


                                
                            }
                            break;

                        case black_rook:
                            {
                                //MessageBox.Show("che2");
                                if (black_checkmate_straight(prev_i, prev_j, -1, 0) == false) return false;
                                if (black_checkmate_straight(prev_i, prev_j, 0, -1) == false) return false;
                                if (black_checkmate_straight(prev_i, prev_j, 0, 1) == false) return false;
                                if (black_checkmate_straight(prev_i, prev_j, 1, 0) == false) return false;
                            }

                            break;

                        case black_knight:
                            {
                               // MessageBox.Show("che3");

                                if (prev_i - 1 >= 0 && prev_i - 1 <= 7 && prev_j - 2 >= 0 && prev_j - 2 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 2 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                           // MessageBox.Show("che31");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i - 2 >= 0 && prev_i - 2 <= 7 && prev_j - 1 >= 0 && prev_j - 1 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 2 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 1 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                            //MessageBox.Show("che32");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i - 2 >= 0 && prev_i - 2 <= 7 && prev_j + 1 >= 0 && prev_j + 1 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 2 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 1 == Convert.ToInt32(white_king_road_j[idx]))
                                        {//
                                           // MessageBox.Show("che33");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i - 1 >= 0 && prev_i - 1 <= 7 && prev_j + 2 >= 0 && prev_j + 2 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i - 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 2 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                           // MessageBox.Show("che34");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i + 1 >= 0 && prev_i + 1 <= 7 && prev_j - 2 >= 0 && prev_j - 2 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 2 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                           // MessageBox.Show("che35");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i + 2 >= 0 && prev_i + 2 <= 7 && prev_j - 1 >= 0 && prev_j - 1 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 2 == Convert.ToInt32(white_king_road_i[idx]) && prev_j - 1 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                          //  MessageBox.Show("che36");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i + 2 >= 0 && prev_i + 2 <= 7 && prev_j + 1 >= 0 && prev_j + 1 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 2 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 1 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                           // MessageBox.Show("che37");
                                            return false;
                                        }

                                    }
                                }


                                if (prev_i + 1 >= 0 && prev_i + 1 <= 7 && prev_j + 2 >= 0 && prev_j + 2 <= 7)
                                {
                                    for (int idx = 0; idx < white_king_road_i.Count; idx++)
                                    {
                                        if (prev_i + 1 == Convert.ToInt32(white_king_road_i[idx]) && prev_j + 2 == Convert.ToInt32(white_king_road_j[idx]))
                                        {
                                           // MessageBox.Show("che38");
                                            return false;
                                        }

                                    }
                                }
                            }
                            break;

                        case black_bishop:
                            {
                               // MessageBox.Show("che4");
                                if (black_checkmate_dioganal(prev_i, prev_j, -1, -1) == false) return false;
                                if (black_checkmate_dioganal(prev_i, prev_j, -1, 1) == false) return false;
                                if (black_checkmate_dioganal(prev_i, prev_j, 1, -1) == false) return false;
                                if (black_checkmate_dioganal(prev_i, prev_j, 1, 1) == false) return false;

                            }
                            break;

                        case black_quenn:
                            {
                                //MessageBox.Show("che5");
                                if (black_checkmate_dioganal(prev_i, prev_j, -1, -1) == false) return false;
                                if (black_checkmate_dioganal(prev_i, prev_j, -1, 1) == false) return false;
                                if (black_checkmate_dioganal(prev_i, prev_j, 1, -1) == false) return false;
                                if (black_checkmate_dioganal(prev_i, prev_j, 1, 1) == false) return false;

                              //  MessageBox.Show("che6");
                                if (black_checkmate_straight(prev_i, prev_j, -1, 0) == false) return false;
                                if (black_checkmate_straight(prev_i, prev_j, 0, -1) == false) return false;
                                if (black_checkmate_straight(prev_i, prev_j, 0, 1) == false) return false;
                                if (black_checkmate_straight(prev_i, prev_j, 1, 0) == false) return false;
                               // MessageBox.Show("che7");
                            }
                            break;


                    }


                }
            }
            return true;
        }


        private bool white_checkmate_dioganal(int c_i, int c_j, int ui, int uj)
        {

            c_i += ui;
            c_j += uj;

            
            while (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
            {

                for (int idx = 0; idx < white_king_road_i.Count; idx++)
                {
                    if (c_i == Convert.ToInt32(white_king_road_i[idx]) && c_j == Convert.ToInt32(white_king_road_j[idx]))
                    {
                        return false;
                    }

                }
          
                if (figures[c_i, c_j] != empty)
                {
                    return true;
                }

                c_i += ui;
                c_j += uj;
            }

            return true;
        }

        private bool white_checkmate_straight(int c_i, int c_j, int ui, int uj)
        {

            c_i += ui;
            c_j += uj;
            
            while (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
            {
                for (int idx = 0; idx < white_king_road_i.Count; idx++)
                {
                    if (c_i == Convert.ToInt32(white_king_road_i[idx]) && c_j == Convert.ToInt32(white_king_road_j[idx]))
                    {
                        return false;
                    }

                }
                if (figures[c_i, c_j] != empty)
                {
                    return true;
                }

                c_i += ui;
                c_j += uj;
            }

            return true;
        }

        private bool black_checkmate_dioganal(int c_i, int c_j, int ui, int uj)
        {

            c_i += ui;
            c_j += uj;


            while (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
            {

                for (int idx = 0; idx < black_king_road_i.Count; idx++)
                {
                    if (c_i == Convert.ToInt32(black_king_road_i[idx]) && c_j == Convert.ToInt32(black_king_road_j[idx]))
                    {
                        return false;
                    }

                }

                if (figures[c_i, c_j] != empty)
                {
                    return true;
                }

                c_i += ui;
                c_j += uj;
            }

            return true;
        }

        private bool black_checkmate_straight(int c_i, int c_j, int ui, int uj)
        {

            c_i += ui;
            c_j += uj;

            while (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
            {
                for (int idx = 0; idx < black_king_road_i.Count; idx++)
                {
                    if (c_i == Convert.ToInt32(black_king_road_i[idx]) && c_j == Convert.ToInt32(black_king_road_j[idx]))
                    {
                        return false;
                    }

                }
                if (figures[c_i, c_j] != empty)
                {
                    return true;
                }

                c_i += ui;
                c_j += uj;
            }

            return true;
        }

        private bool black_moves_dioganal(int c_i, int c_j, int ui, int uj)
        {
            int prev_i = c_i;
            int prev_j = c_j;
            c_i += ui;
            c_j += uj;


            while (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
            {

                if (figures[c_i, c_j] / 10 == 1 || figures[c_i, c_j] == empty)
                {
                    black_legal_moves.Add((prev_i.ToString() + prev_j.ToString()) + " " + (c_i.ToString() + c_j.ToString()));

                }

                if (figures[c_i, c_j] != empty)
                {
                    return true;
                }

                c_i += ui;
                c_j += uj;
            }

            return true;
        }

        private void black_moves_straight(int c_i, int c_j, int ui, int uj)
        {
            int prev_i = c_i;
            int prev_j = c_j;
            c_i += ui;
            c_j += uj;

            while (c_i >= 0 && c_i <= 7 && c_j >= 0 && c_j <= 7)
            {
                if (figures[c_i, c_j] / 10 == 1 || figures[c_i, c_j] == empty)
                {
                    black_legal_moves.Add((prev_i.ToString() + prev_j.ToString()) + " " + (c_i.ToString() + c_j.ToString()));

                }
                if (figures[c_i, c_j] != empty)
                {
                    break;
                }

                c_i += ui;
                c_j += uj;
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //listBox2.Items.Clear();
           // black_legal_moves.Clear();
           // add_board = false;
            
           // get_black_moves();
            /* for (int h = 0; h < /*black_legal_moves*///black_king_road_i.Count; h++)
            /* {
                 listBox2.Items.Add(black_king_road_i[h].ToString() + " " + black_king_road_j[h].ToString());
                //listBox2.Items.Add(black_legal_moves[h].ToString());

             }*/
           //listBox2.Items.Add(black_king_i.ToString() + " " + black_king_j.ToString());

        }

        private void pawn_to_queen()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (figures[i, j] == 11 && i == 0)
                    {
                        figures[i, j] = white_quenn;
                        cell[i, j].Image = new Bitmap(@"white_quenn.png");
                    }
                    

                    if (figures[i, j] == 21 && i == 7)
                    {
                        figures[i, j] = black_quenn;
                        cell[i, j].Image = new Bitmap(@"black_quenn.png");
                    }
                }

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }
    }
}
