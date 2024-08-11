using Gtk;
using Gdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    class MyWindow : Gtk.Window
    {
        private Button[,] buttons = new Button[3, 3];
        private Game game;

        public MyWindow() : base("Tic-Tac-Toe")
        {
            game = new Game();
            SetDefaultSize(1000, 1000);
            SetPosition(WindowPosition.Center);

            VBox vbox = new VBox();
            Table table = new Table(3, 3, true);

            Pango.FontDescription fontDesc = Pango.FontDescription.FromString("Arial 72");

            for (uint i = 0; i < 3; i++)
            {
                for (uint j = 0; j < 3; j++)
                {
                    buttons[i, j] = new Button(" ");
                    buttons[i, j].ModifyFont(fontDesc);
                    buttons[i, j].Clicked += OnButtonClicked;
                    buttons[i, j].Name = $"{i}{j}";
                    table.Attach(buttons[i, j], i, i + 1, j, j + 1);
                }
            }

            vbox.PackStart(table, true, true, 0);
            Add(vbox);
            DeleteEvent += delegate { Application.Quit(); };
            ShowAll();
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int i = int.Parse(clickedButton.Name[0].ToString());
            int j = int.Parse(clickedButton.Name[1].ToString());

            Move move = new Move(i, j);
            if (game.state == 1)
            {
                if (game.MakeMove(move, 1))
                {
                    clickedButton.Label = "X";
                    if (game.CheckWinner() == 1)
                        ShowMessage("Player 1 (X) wins!");
                }
             
            }
            else if (game.state == -1)
            {
                if (game.MakeMove(move, -1))
                {
                    clickedButton.Label = "O";
                    if (game.CheckWinner() == -1)
                        ShowMessage("Player 2 (O) wins!");
                }
               
            }

            game.state *= -1;

            if (game.get_possible_moves().Count == 0)
            {
                ShowMessage("It's a draw!");
            }
        }

        private void ShowMessage(string message)
        {
            MessageDialog md = new MessageDialog(this,
                DialogFlags.DestroyWithParent, MessageType.Info,
                ButtonsType.Ok, message);
            md.Run();
            md.Destroy();
            ResetGame();
        }

        private void ResetGame()
        {
            game.ResetBoard();
            for (uint i = 0; i < 3; i++)
            {
                for (uint j = 0; j < 3; j++)
                {
                    buttons[i, j].Label = " ";
                }
            }
            game.state = 1;
        }
    }

    class Game
    {
        public int[,] board;
        public int state;

        public Game()
        {
            board = new int[3, 3];
            ResetBoard();
            state = 1;
        }

        public void ResetBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = 0;
                }
            }
        }

        public bool MakeMove(Move move, int player)
        {
            var moves = get_possible_moves();
            foreach(var i in moves)
            {
                if(move.x == i.x  && move.y == i.y)
                {
                    board[move.x, move.y] = player;
                    return true;
                }
            }
            
            return false;
        }

        public int CheckWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != 0)
                    return board[i, 0];
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != 0)
                    return board[0, i];
            }
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != 0)
                return board[0, 0];
            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != 0)
                return board[0, 2];
            return 0;
        }

        public List<Move> get_possible_moves()
        {
            List<Move> possible_moves = new List<Move>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        possible_moves.Add(new Move(i, j));
                    }
                }
            }
            return possible_moves;
        }
    }

    class Move
    {
        public int x;
        public int y;

        public Move(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MyWindow w = new MyWindow();
            w.ShowAll();
            Application.Run();
        }
    }
}
