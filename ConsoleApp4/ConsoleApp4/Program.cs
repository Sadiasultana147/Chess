using System.Drawing;

namespace ConsoleApp4
{

    public enum PieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public enum PieceColor
    {
        White,
        Black
    }
    public abstract class ChessPiece
    {
        public PieceType Type { get; private set; }
        public PieceColor Color { get; private set; }

        protected ChessPiece(PieceType type, PieceColor color)
        {
            Type = type;
            Color = color;
        }

        public abstract bool IsValidMove(ChessBoard board, int currentRow, int currentCol, int newRow, int newCol);

        public PieceColor GetColor()
        {
            return Color;
        }
        public PieceType GetType()
        {
            return Type;
        }
    }

    public class Pawn : ChessPiece
    {
        public Pawn(PieceColor color) : base(PieceType.Pawn, color) { }

        public override bool IsValidMove(ChessBoard board, int currentRow, int currentCol, int newRow, int newCol)
        {
            int rowDiff = newRow - currentRow;
            int colDiff = Math.Abs(newCol - currentCol);

            if (Color == PieceColor.White)
            {
                // Check if the pawn is moving forward one or two spaces
                if (rowDiff == -1 || (rowDiff == -2 && currentRow == 6))
                {
                    // Check if there is a piece directly in front of the pawn
                    if (board.GetPiece(newRow, newCol) == null)
                    {
                        return true;
                    }
                }
                // Check if the pawn is capturing a piece diagonally
                else if (rowDiff == -1 && colDiff == 1)
                {
                    ChessPiece piece = board.GetPiece(newRow, newCol);
                    if (piece != null && piece.Color == PieceColor.Black)
                    {
                        return true;
                    }
                }
            }
            else if (Color == PieceColor.Black)
            {
                // Check if the pawn is moving forward one or two spaces
                if (rowDiff == 1 || (rowDiff == 2 && currentRow == 1))
                {
                    // Check if there is a piece directly in front of the pawn
                    if (board.GetPiece(newRow, newCol) == null)
                    {
                        return true;
                    }
                }
                // Check if the pawn is capturing a piece diagonally
                else if (rowDiff == 1 && colDiff == 1)
                {
                    ChessPiece piece = board.GetPiece(newRow, newCol);
                    if (piece != null && piece.Color == PieceColor.White)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }


    public class Knight : ChessPiece
    {
        public Knight(PieceColor color) : base(PieceType.Knight, color) { }

        public override bool IsValidMove(ChessBoard board, int currentRow, int currentCol, int newRow, int newCol)
        {
            int rowDiff = Math.Abs(newRow - currentRow);
            int colDiff = Math.Abs(newCol - currentCol);

            if ((rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2))
            {
                ChessPiece piece = board.GetPiece(newRow, newCol);
                if (piece == null || piece.Color != Color)
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class Bishop : ChessPiece
    {
        public Bishop(PieceColor color) : base(PieceType.Bishop, color) { }

        public override bool IsValidMove(ChessBoard board, int currentRow, int currentCol, int newRow, int newCol)
        {
            int rowDiff = Math.Abs(newRow - currentRow);
            int colDiff = Math.Abs(newCol - currentCol);

            if (rowDiff == colDiff)
            {
                int rowDir = (newRow > currentRow) ? 1 : -1;
                int colDir = (newCol > currentCol) ? 1 : -1;

                int r = currentRow + rowDir;
                int c = currentCol + colDir;

                while (r != newRow && c != newCol)
                {
                    ChessPiece piece = board.GetPiece(r, c);
                    if (piece != null)
                    {
                        return false;
                    }

                    r += rowDir;
                    c += colDir;
                }

                ChessPiece targetPiece = board.GetPiece(newRow, newCol);
                if (targetPiece == null || targetPiece.Color != Color)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class Rook : ChessPiece
    {
        public Rook(PieceColor color) : base(PieceType.Rook, color) { }

        public override bool IsValidMove(ChessBoard board, int currentRow, int currentCol, int newRow, int newCol)
        {
            int rowDiff = Math.Abs(newRow - currentRow);
            int colDiff = Math.Abs(newCol - currentCol);

            if ((rowDiff == 0 && colDiff != 0) || (rowDiff != 0 && colDiff == 0))
            {
                int rowDir = (newRow > currentRow) ? 1 : (newRow < currentRow) ? -1 : 0;
                int colDir = (newCol > currentCol) ? 1 : (newCol < currentCol) ? -1 : 0;

                int r = currentRow + rowDir;
                int c = currentCol + colDir;

                while (r != newRow || c != newCol)
                {
                    ChessPiece piece = board.GetPiece(r, c);
                    if (piece != null)
                    {
                        return false;
                    }

                    r += rowDir;
                    c += colDir;
                }

                ChessPiece targetPiece = board.GetPiece(newRow, newCol);
                if (targetPiece == null || targetPiece.Color != Color)
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class King : ChessPiece
    {
        public King(PieceColor color) : base(PieceType.King, color) { }

        public override bool IsValidMove(ChessBoard board, int currentRow, int currentCol, int newRow, int newCol)
        {
            int rowDiff = Math.Abs(newRow - currentRow);
            int colDiff = Math.Abs(newCol - currentCol);

            if (rowDiff <= 1 && colDiff <= 1)
            {
                ChessPiece piece = board.GetPiece(newRow, newCol);
                if (piece == null || piece.Color != Color)
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class Queen : ChessPiece
    {
        public Queen(PieceColor color) : base(PieceType.Queen, color) { }

        public override bool IsValidMove(ChessBoard board, int currentRow, int currentCol, int newRow, int newCol)
        {
            Bishop bishop = new Bishop(Color);
            if (bishop.IsValidMove(board, currentRow, currentCol, newRow, newCol))
            {
                return true;
            }

            Rook rook = new Rook(Color);
            if (rook.IsValidMove(board, currentRow, currentCol, newRow, newCol))
            {
                return true;
            }

            return false;
        }
    }
    public class ChessBoard
    {
        private ChessPiece[,] board = new ChessPiece[8, 8];
       
      
            public  const int BoardSize = 8;

            // Rest of the implementation
        

        public ChessBoard()
        {
            // Create the 2D array to represent the board
            board = new ChessPiece[8, 8];

            // Place the pawns for both colors
            for (int col = 0; col < 8; col++)
            {
                board[1, col] = new Pawn(PieceColor.Black);
                board[6, col] = new Pawn(PieceColor.White);
            }

            // Place the rooks for both colors
            board[0, 0] = new Rook(PieceColor.Black);
            board[0, 7] = new Rook(PieceColor.Black);
            board[7, 0] = new Rook(PieceColor.White);
            board[7, 7] = new Rook(PieceColor.White);

            // Place the knights for both colors
            board[0, 1] = new Knight(PieceColor.Black);
            board[0, 6] = new Knight(PieceColor.Black);
            board[7, 1] = new Knight(PieceColor.White);
            board[7, 6] = new Knight(PieceColor.White);

            // Place the bishops for both colors
            board[0, 2] = new Bishop(PieceColor.Black);
            board[0, 5] = new Bishop(PieceColor.Black);
            board[7, 2] = new Bishop(PieceColor.White);
            board[7, 5] = new Bishop(PieceColor.White);

            // Place the queens for both colors
            board[0, 3] = new Queen(PieceColor.Black);
            board[7, 3] = new Queen(PieceColor.White);

            // Place the kings for both colors
            board[0, 4] = new King(PieceColor.Black);
            board[7, 4] = new King(PieceColor.White);
        }
        private bool IsPositionOnBoard(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }


        public ChessPiece GetPiece(int row, int col)
        {
            if (!IsPositionOnBoard(row, col))
            {
                return null;
            }

            return board[row, col];
        }


        public void MovePiece(int currentRow, int currentCol, int newRow, int newCol)
        {
            // Retrieve the piece at the current position
            ChessPiece piece = GetPiece(currentRow, currentCol);

            // Check if the move is valid for the piece
            if (piece != null && piece.IsValidMove(this, currentRow, currentCol, newRow, newCol))
            {
                // Move the piece to the new position
                board[newRow, newCol] = piece;
                board[currentRow, currentCol] = null;
            }
            else
            {
                // The move is invalid, throw an exception or handle it appropriately
                throw new InvalidOperationException("Invalid move.");
            }
        }

    }
    public class ChessGame
    {
        private ChessBoard board;
        private PieceColor currentPlayer;
        private int movesPlayed;

        public ChessGame()
        {
            board = new ChessBoard();
            currentPlayer = PieceColor.White;
            movesPlayed = 0;
        }
        public void DrawBoard()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // enable Unicode characters
            char[][] board = new char[8][];
            for (int i = 0; i < 8; i++)
            {
                board[i] = new char[8];
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        board[i][j] = '\u25A0'; // black square
                    }
                    else
                    {
                        board[i][j] = '\u25A1'; // white square
                    }
                }
            }

            // set up pieces on the board
            board[0][0] = '\u265C'; // black rook
            board[0][1] = '\u265E'; // black knight
            board[0][2] = '\u265D'; // black bishop
            board[0][3] = '\u265B'; // black queen
            board[0][4] = '\u265A'; // black king
            board[0][5] = '\u265D'; // black bishop
            board[0][6] = '\u265E'; // black knight
            board[0][7] = '\u265C'; // black rook
            for (int i = 0; i < 8; i++)
            {
                board[1][i] = '\u265F'; // black pawn
            }
            board[7][0] = '\u2656'; // white rook
            board[7][1] = '\u2658'; // white knight
            board[7][2] = '\u2657'; // white bishop
            board[7][3] = '\u2655'; // white queen
            board[7][4] = '\u2654'; // white king
            board[7][5] = '\u2657'; // white bishop
            board[7][6] = '\u2658'; // white knight
            board[7][7] = '\u2656'; // white rook
            for (int i = 0; i < 8; i++)
            {
                board[6][i] = '\u2659'; // white pawn
            }

            // draw the board
            Console.WriteLine("   A B C D E F G H");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(board[i][j] + " ");
                }
                Console.WriteLine(8 - i);
            }
            Console.WriteLine("   A B C D E F G H");
        }

        public void Play(int fromRow, int fromCol, int toRow, int toCol)
        {
            // Retrieve the piece at the starting position
            ChessPiece piece = board.GetPiece(fromRow, fromCol);

            // Check if the move is valid for the selected piece
            if (piece != null && piece.IsValidMove(board, fromRow, fromCol, toRow, toCol))
            {
                // Move the piece to the new position
                board.MovePiece(fromRow, fromCol, toRow, toCol);

                // Update the game state
                movesPlayed++;
                currentPlayer = currentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White; // Set the next player
            }
            else
            {
                // The move is invalid, throw an exception or handle it appropriately
                throw new InvalidOperationException("Invalid move!");
            }
        }


        public ChessBoard GetBoard()
        {
            return board;
        }

        public PieceColor GetCurrentPlayer()
        {
            return currentPlayer;
        }

        public int GetMovesPlayed()
        {
            return movesPlayed;
        }

        public bool IsOver()
        {
            // Check if the current player's king is in check
            if (IsKingInCheck(currentPlayer))
            {
                // Check if the current player has any legal moves
                if (!HasLegalMoves(currentPlayer))
                {
                    // If the current player has no legal moves, the game is over and it's a checkmate
                    return true;
                }
            }
            else
            {
                // Check if the current player has any legal moves
                if (!HasLegalMoves(currentPlayer))
                {
                    // If the current player has no legal moves and their king is not in check, it's a stalemate
                    return true;
                }
            }

            // If we reach this point, the game is not over yet
            return false;
        }

        private bool IsKingInCheck(PieceColor color)
        {
            // Get the position of the king of the given color
            int kingRow = -1;
            int kingCol = -1;

            for (int row = 0; row < ChessBoard.BoardSize; row++)
            {
                for (int col = 0; col < ChessBoard.BoardSize; col++)
                {
                    ChessPiece piece = board.GetPiece(row, col);

                    if (piece != null && piece.GetColor() == color && piece.GetType() == PieceType.King)
                    {
                        kingRow = row;
                        kingCol = col;
                        break;
                    }
                }

                if (kingRow != -1 && kingCol != -1)
                {
                    break;
                }
            }

            // Check if any of the opponent's pieces can attack the king's position
            for (int row = 0; row < ChessBoard.BoardSize; row++)
            {
                for (int col = 0; col < ChessBoard.BoardSize; col++)
                {
                    ChessPiece piece = board.GetPiece(row, col);

                    if (piece != null && piece.GetColor() != color && piece.IsValidMove(board, row, col, kingRow, kingCol))
                    {
                        return true;
                    }
                }
            }

            // If we reach this point, the king is not in check
            return false;
        }

        private bool HasLegalMoves(PieceColor color)
        {
            // Check if any of the player's pieces have legal moves
            for (int row = 0; row < ChessBoard.BoardSize; row++)
            {
                for (int col = 0; col < ChessBoard.BoardSize; col++)
                {
                    ChessPiece piece = board.GetPiece(row, col);

                    if (piece != null && piece.GetColor() == color)
                    {
                        // Check if the piece has any legal moves
                        for (int toRow = 0; toRow < ChessBoard.BoardSize; toRow++)
                        {
                            for (int toCol = 0; toCol < ChessBoard.BoardSize; toCol++)
                            {
                                if (piece.IsValidMove(board, row, col, toRow, toCol))
                                {
                                    // If the piece has at least one legal move, return true
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            // If we reach this point, the player has no legal moves
            return false;
        }

    }


    internal class Program
    {
        static void Main(string[] args)
        {

            ChessGame game = new ChessGame();
            ChessBoard board = new ChessBoard();
            game.GetBoard(); // set up the initial chess board
            bool isWhiteTurn = true;
            game.DrawBoard();
            while (true)
            {
                // display the chess board
                Console.WriteLine(game.GetBoard().ToString());

                // get input for the current player's move
                string currentPlayer = isWhiteTurn ? "White" : "Black";
                Console.WriteLine($"{currentPlayer} player's turn.");
                Console.Write("Enter the starting row: ");
                int startRow = int.Parse(Console.ReadLine());
                Console.Write("Enter the starting column: ");
                int startCol = int.Parse(Console.ReadLine());
                Console.Write("Enter the ending row: ");
                int endRow = int.Parse(Console.ReadLine());
                Console.Write("Enter the ending column: ");
                int endCol = int.Parse(Console.ReadLine());

                try
                {
                    // move the piece
                    board.MovePiece(startRow, startCol, endRow, endCol);

                    // switch player turn
                    isWhiteTurn = !isWhiteTurn;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid move: " + ex.Message);
                }

                // check if the game is over
                if (game.IsOver())
                {
                    Console.WriteLine("Game over!");
                    break;
                }
            }


        }
    }
}