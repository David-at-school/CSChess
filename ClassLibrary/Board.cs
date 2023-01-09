/***************************************************************
 * File: Board.cs
 * Created By: Justin Grindal		Date: 27 June, 2013
 * Description: The main chess board. Board contain the chess cell
 * which will contain the chess pieces. Board also contains the methods
 * to get and set the user moves.
 ***************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using static ChessLibrary.Piece;

namespace ChessLibrary
{
	/// <summary>
	/// he main chess board. Board contain the chess cell
	/// which will contain the chess pieces. Board also contains the methods
	/// to get and set the user moves.
	/// </summary>
	[Serializable]
	public class Board
	{
		private Side m_WhiteSide, m_BlackSide;  // Chess board site object 
		private Cells m_cells;  // collection of cells in the board

		public Board()
		{
			m_WhiteSide = new Side(Side.SideType.White);    // Makde white side
			m_BlackSide = new Side(Side.SideType.Black);    // Makde white side

			m_cells = new Cells();                  // Initialize the chess cells collection
		}

		// Initialize the chess board and place piece on thier initial positions
		public void Init(bool chess960)
		{
			m_cells.Clear();        // Remove any existing chess cells

			// Build the 64 chess board cells
			for (int row = 1; row <= 8; row++)
				for (int col = 1; col <= 8; col++)
				{
					m_cells.Add(new Cell(row, col));    // Initialize and add the new chess cell
				}

			List<Piece.PieceType> pieceTypes = new List<Piece.PieceType>();
			if (chess960)
			{
				pieceTypes = new List<Piece.PieceType> { Piece.PieceType.Empty, Piece.PieceType.Empty, Piece.PieceType.Empty, Piece.PieceType.Empty, Piece.PieceType.Empty, Piece.PieceType.Empty, Piece.PieceType.Empty, Piece.PieceType.Empty };
				Piece.PieceType[] piecesToPlace = { Piece.PieceType.Queen, Piece.PieceType.Knight, Piece.PieceType.Knight };
				Random rng = new Random();
				pieceTypes[rng.Next(3) * 2] = Piece.PieceType.Bishop;
				pieceTypes[rng.Next(3) * 2 + 1] = Piece.PieceType.Bishop;
				bool piecePlaced = false;
				foreach (var item in piecesToPlace)
				{
					piecePlaced = false;
					while (!piecePlaced)
					{
						int rand = rng.Next(7);
						if (pieceTypes[rand] == Piece.PieceType.Empty)
						{
							piecePlaced = true;
							pieceTypes[rand] = item;
						}
					}
				}
				pieceTypes[pieceTypes.LastIndexOf(Piece.PieceType.Empty)] = Piece.PieceType.Rook;
				pieceTypes[pieceTypes.LastIndexOf(Piece.PieceType.Empty)] = Piece.PieceType.King;
				pieceTypes[pieceTypes.LastIndexOf(Piece.PieceType.Empty)] = Piece.PieceType.Rook;
			}
			else
			{
				pieceTypes = new List<PieceType> { PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook };
			}
			// Now setup the board for black side
			m_cells["a1"].piece = new Piece(pieceTypes[0], m_BlackSide);
			m_cells["h1"].piece = new Piece(pieceTypes[7], m_BlackSide);
			m_cells["b1"].piece = new Piece(pieceTypes[1], m_BlackSide);
			m_cells["g1"].piece = new Piece(pieceTypes[6], m_BlackSide);
			m_cells["c1"].piece = new Piece(pieceTypes[2], m_BlackSide);
			m_cells["f1"].piece = new Piece(pieceTypes[5], m_BlackSide);
			m_cells["e1"].piece = new Piece(pieceTypes[4], m_BlackSide);
			m_cells["d1"].piece = new Piece(pieceTypes[3], m_BlackSide);
			for (int col = 1; col <= 8; col++)
				m_cells[2, col].piece = new Piece(Piece.PieceType.Pawn, m_BlackSide);

			// Now setup the board for white side
			m_cells["a8"].piece = new Piece(pieceTypes[0], m_WhiteSide);
			m_cells["h8"].piece = new Piece(pieceTypes[7], m_WhiteSide);
			m_cells["b8"].piece = new Piece(pieceTypes[1], m_WhiteSide);
			m_cells["g8"].piece = new Piece(pieceTypes[6], m_WhiteSide);
			m_cells["c8"].piece = new Piece(pieceTypes[2], m_WhiteSide);
			m_cells["f8"].piece = new Piece(pieceTypes[5], m_WhiteSide);
			m_cells["e8"].piece = new Piece(pieceTypes[4], m_WhiteSide);
			m_cells["d8"].piece = new Piece(pieceTypes[3], m_WhiteSide);
			for (int col = 1; col <= 8; col++)
				m_cells[7, col].piece = new Piece(Piece.PieceType.Pawn, m_WhiteSide);

		}

		// get the new item by rew and column
		public Cell this[int row, int col]
		{
			get
			{
				return m_cells[row, col];
			}
		}

		// get the new item by string location
		public Cell this[string strloc]
		{
			get
			{
				return m_cells[strloc];
			}
		}

		// get the chess cell by given cell
		public Cell this[Cell cellobj]
		{
			get
			{
				return m_cells[cellobj.ToString()];
			}
		}

		/// <summary>
		/// Serialize the Game object as XML String
		/// </summary>
		/// <returns>XML containing the Game object state XML</returns>
		public XmlNode XmlSerialize(XmlDocument xmlDoc)
		{
			XmlElement xmlBoard = xmlDoc.CreateElement("Board");

			// Append game state attributes
			xmlBoard.AppendChild(m_WhiteSide.XmlSerialize(xmlDoc));
			xmlBoard.AppendChild(m_BlackSide.XmlSerialize(xmlDoc));

			xmlBoard.AppendChild(m_cells.XmlSerialize(xmlDoc));

			// Return this as String
			return xmlBoard;
		}

		/// <summary>
		/// DeSerialize the Board object from XML String
		/// </summary>
		/// <returns>XML containing the Board object state XML</returns>
		public void XmlDeserialize(XmlNode xmlBoard)
		{
			// Deserialize the Sides XML
			XmlNode side = XMLHelper.GetFirstNodeByName(xmlBoard, "Side");

			// Deserialize the XML nodes
			m_WhiteSide.XmlDeserialize(side);
			m_BlackSide.XmlDeserialize(side.NextSibling);

			// Deserialize the Cells
			XmlNode xmlCells = XMLHelper.GetFirstNodeByName(xmlBoard, "Cells");
			m_cells.XmlDeserialize(xmlCells);
		}

		// get all the cell locations on the chess board
		public ArrayList GetAllCells()
		{
			ArrayList CellNames = new ArrayList();

			// Loop all the squars and store them in Array List
			for (int row = 1; row <= 8; row++)
				for (int col = 1; col <= 8; col++)
				{
					CellNames.Add(this[row, col].ToString()); // append the cell name to list
				}

			return CellNames;
		}

		// get all the cell containg pieces of given side
		public ArrayList GetSideCell(Side.SideType PlayerSide)
		{
			ArrayList CellNames = new ArrayList();

			// Loop all the squars and store them in Array List
			for (int row = 1; row <= 8; row++)
				for (int col = 1; col <= 8; col++)
				{
					// check and add the current type cell
					if (this[row, col].piece != null && !this[row, col].IsEmpty() && this[row, col].piece.Side.type == PlayerSide)
						CellNames.Add(this[row, col].ToString()); // append the cell name to list
				}

			return CellNames;
		}

		// Returns the cell on the top of the given cell
		public Cell TopCell(Cell cell)
		{
			return this[cell.row - 1, cell.col];
		}

		// Returns the cell on the left of the given cell
		public Cell LeftCell(Cell cell)
		{
			return this[cell.row, cell.col - 1];
		}

		// Returns the cell on the right of the given cell
		public Cell RightCell(Cell cell)
		{
			return this[cell.row, cell.col + 1];
		}

		// Returns the cell on the bottom of the given cell
		public Cell BottomCell(Cell cell)
		{
			return this[cell.row + 1, cell.col];
		}

		// Returns the cell on the top-left of the current cell
		public Cell TopLeftCell(Cell cell)
		{
			return this[cell.row - 1, cell.col - 1];
		}

		// Returns the cell on the top-right of the current cell
		public Cell TopRightCell(Cell cell)
		{
			return this[cell.row - 1, cell.col + 1];
		}

		// Returns the cell on the bottom-left of the current cell
		public Cell BottomLeftCell(Cell cell)
		{
			return this[cell.row + 1, cell.col - 1];
		}

		// Returns the cell on the bottom-right of the current cell
		public Cell BottomRightCell(Cell cell)
		{
			return this[cell.row + 1, cell.col + 1];
		}
	}
}
