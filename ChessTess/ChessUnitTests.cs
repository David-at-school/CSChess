using Chess;
using ChessLibrary;
using static ChessLibrary.Piece;

namespace ChessTess
{
	[TestClass]
	public class ChessUnitTests
	{
		[TestMethod]
		public void TestNumberOfPiecesPlaced()
		{
			Board board = new Board();
			board.Init(true);

			int pieceCount = 0;

			for (int row = 1; row <= 8; row++)
				for (int col = 1; col <= 8; col++)
				{
					// check and add the current type cell
					if (board[row, col].piece != null && !board[row, col].IsEmpty())
						pieceCount++;
				}

			Assert.AreEqual(32, pieceCount);
		}

		[TestMethod]
		public void TestTypesOfPiecesPlaced()
		{
			Board board = new Board();
			board.Init(true);

			List<PieceType> expectedPieceTypes = new List<PieceType> { PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook };
			List<PieceType> actualPieceTypes = new List<PieceType>();

			for (int col = 1; col <= 8; col++)
			{
				if (board[1, col].piece != null && !board[1, col].IsEmpty())
				{
					actualPieceTypes.Add(board[1, col].piece.Type);
				}
			}

			expectedPieceTypes.Sort();
			actualPieceTypes.Sort();

			bool differenceFound = false;
			for (int i = 0; i < expectedPieceTypes.Count; i++)
			{
				if (expectedPieceTypes[i] != actualPieceTypes[i])
				{
					differenceFound = true;
				}
			}

			Assert.IsFalse(differenceFound);
			//Assert.AreEqual(expectedPieceTypes, actualPieceTypes);
		}

		[TestMethod]
		public void TestKingNotOnEdge()
		{
			Board board = new Board();
			board.Init(true);

			int king = 0;
			for (int col = 1; col <= 8; col++)
			{
				if (board[1, col].piece.IsKing())
				{
					king = col;
				}
			}

			Assert.AreNotEqual(king, 1);
			Assert.AreNotEqual(king, 8);
		}

		[TestMethod]
		public void TestKingBetweenRooks()
		{
			Board board = new Board();
			board.Init(true);

			bool rightRookFound = false;
			bool leftRookFound = false;

			for (int col = 1; col <= 8; col++)
			{
				if (board[1, col].piece.IsKing())
				{
					Cell king = board[1, col];
					Cell checkCell = king;

					for (int i = col; i < 8; i++)
					{
						if (board.RightCell(board[1, i]).piece.IsRook())
						{
							rightRookFound= true;
							break;
						}
					}

					for (int i = col; i > 1; i--)
					{
						if (board.LeftCell(board[1, i]).piece.IsRook())
						{
							leftRookFound = true;
							break;
						}
					}
				}
			}

			Assert.IsTrue(leftRookFound);
			Assert.IsTrue(rightRookFound);
		}

		[TestMethod]
		public void TestAreBishopsOnDifferentColors()
		{
			Board board = new Board();
			board.Init(true);

			bool lightBishopFound = false;
			bool darkBishopFound = false;

			for (int col = 1; col <= 8; col++)
			{
				if (board[1, col].piece.IsBishop())
				{
					if (board[1, col].IsDark)
					{
						darkBishopFound = true;
					}

					if (!board[1, col].IsDark)
					{
						lightBishopFound = true;
					}
				}
			}

			Assert.IsTrue(lightBishopFound);
			Assert.IsTrue(darkBishopFound);
		}
	}
}