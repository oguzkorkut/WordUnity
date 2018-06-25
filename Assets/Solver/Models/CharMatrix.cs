using System;
using System.Collections.Generic;
using System.Linq;
using WordShuffler.Models;
using UnityEngine;

namespace Models
{
	public class CharMatrix
	{
		#region fields
		
		private char[] 	_validRandomCharacters = new char[26] 	{'e', 't', 'a', 'o', 'i', 'n', 's', 'r', 						'h', 'd', 'l', 'u', 'c', 'm', 				'f', 'y', 'w', 'g', 'p', 'b', 					'v', 'k', 'x', 			'q', 'j', 'z'};
		private float[] _letterschance 		= new float[26]		{12.02f,21.12f,29.24f,36.92f,44.23f,51.18f,57.46f,63.48f,		69.4f,73.72f,77.7f,80.58f,83.29f,85.9f,		88.2f,90.31f,92.4f,94.43f,96.25f,97.74f,		98.85f,99.54f,99.71f,	99.82f,99.92f,100.00f};
		private Dictionary<char, int> _lettersused		= new Dictionary<char, int>();
		private const int maxlettersused	= 3;
		
		private readonly char[,] _matrix;
		private readonly List<string> _longwords;
		private readonly int _size;
		private int CellCount { get { return _size * _size; } }
		
		#endregion
		
		public char[,] GetMatrix()
		{
			return _matrix;
		}
		
		public char GetCharAt(CharCoordinate coordinate)
		{
			return _matrix[coordinate.Row, coordinate.Column];
		}
		
		#region Constructor
		
		public CharMatrix(int size, List<string> words, List<string> longwords)
		{        	
			_size = size;
			_longwords = longwords;
			
			for(int i=0;i<_validRandomCharacters.Length;i++)
			{
				_lettersused.Add(_validRandomCharacters[i], 0);
			}
			
			_matrix = new char[_size, _size];
			
			var baseWord = GetRadnomWord(CellCount - size);
			
			for(int i=0;i<baseWord.Length;i++)
			{
				_lettersused[baseWord[i]] = _lettersused[baseWord[i]] + 1;
			}
			
			WriteWord(baseWord.ToCharArray());
			FillEmptyCellsRandom();
		}
		
		#endregion
		
		#region methods
		
		public int Size
		{
			get
			{
				return _size;
			}
			
		}
		
		public IEnumerable<CharCoordinate> GetAllCoordinates()
		{
			return GetAllCoordinates(Size);
		}
		
		public static IEnumerable<CharCoordinate> GetAllCoordinates(int size)
		{
			var all = new List<CharCoordinate>();
			
			for (var i = 0; i < size; i++)
			{
				for (var j = 0; j < size; j++)
				{
					all.Add(new CharCoordinate(i, j));
				}
			}
			
			return all;
		}
		
		private string GetRadnomWord(int maxLength)
		{
			var longWords = _longwords.ToList();
			
			int count = longWords.Count();
			var index = UnityEngine.Random.Range(0, count);
			while (longWords[index].Length >_size*_size)
			{
				count = longWords.Count();
				index = UnityEngine.Random.Range(0, count);
			}
			return longWords[index];
		}
		
		#endregion
		
		#region building the matrix
		
		private void WriteWord(char[] word)
		{
			var charCount = word.Length;
			
			if (charCount > GetEmptyCells().Count())
				throw new InvalidOperationException("Data is longer then empty cells amount");
			
			WriteWord(word, new CharCoordinate[charCount], new List<CharCoordinate>[charCount], -1, false); //Initial conditions
		}
		
		private void WriteWord(char[] word, CharCoordinate[] path, List<CharCoordinate>[] triedCoordinatesAtIndex, int lastCoordinateIndex, bool steppedBack, int runCount = 0)
		{
			runCount++;
			
			var charCount = word.Length;
			
			var currentCoordinateIndex = lastCoordinateIndex + 1;
			
			if (currentCoordinateIndex == 0)
				path[0] = GetRandomEmptyCell();
			
			var currentIsTrapped = (currentCoordinateIndex > 0) && (IsTrapped(path[currentCoordinateIndex - 1], triedCoordinatesAtIndex[currentCoordinateIndex]));
			
			if (currentIsTrapped)
			{
				DelCharAt(path[lastCoordinateIndex]);
				path[currentCoordinateIndex] = null;
				triedCoordinatesAtIndex[currentCoordinateIndex] = null;
				currentCoordinateIndex--;
				
				if (steppedBack)
				{
					currentCoordinateIndex--;
				}
			}
			else
			{
				var currentCoordinate = currentCoordinateIndex == 0
					? path[currentCoordinateIndex]
					: GetRandomEmptySibling(path[currentCoordinateIndex - 1], except: triedCoordinatesAtIndex[currentCoordinateIndex]);
				path[currentCoordinateIndex] = currentCoordinate;
				triedCoordinatesAtIndex[currentCoordinateIndex] = triedCoordinatesAtIndex[currentCoordinateIndex] ?? new List<CharCoordinate>();
				triedCoordinatesAtIndex[currentCoordinateIndex].Add(currentCoordinate);
				SetCharAt(currentCoordinate, word[currentCoordinateIndex]);
			}
			
			if (currentCoordinateIndex < charCount - 1 || currentIsTrapped)
			{
				WriteWord(word, path, triedCoordinatesAtIndex, currentCoordinateIndex, currentIsTrapped, runCount);
			}
			
		}
		
		private void FillEmptyCellsRandom()
		{
			var emptyCells = GetEmptyCells();
			
			foreach (var cell in emptyCells)
			{
				SetCharAt(cell, GetRandomCharacter());
			}
		}
		
		#endregion
		
		#region character operations
		
		private char GetRandomCharacter()
		{
			var randomnr = UnityEngine.Random.Range(0.00f,100.00f);
			
			for(int a=0;a<_letterschance.Length;a++)
			{
				if(randomnr<=_letterschance[a])
				{
					if(_lettersused[_validRandomCharacters[a]]<maxlettersused)
					{
						_lettersused[_validRandomCharacters[a]]=_lettersused[_validRandomCharacters[a]] + 1;
						
						return _validRandomCharacters[a];
					}
					else
					{
						return GetRandomCharacter();
					}
				}
			}
			return _validRandomCharacters[0];
		}
		
		private void DelCharAt(CharCoordinate coordinate)
		{
			_matrix[coordinate.Row, coordinate.Column] = '\0';
		}
		
		private void SetCharAt(CharCoordinate coordinate, char c)
		{
			_matrix[coordinate.Row, coordinate.Column] = c;
		}
		
		#endregion
		
		#region empty sibling search
		
		private IEnumerable<CharCoordinate> GetEmptySiblings(CharCoordinate coordinate)
		{
			var emptyCells = GetEmptyCells();
			var emptySiblings = emptyCells.Where(x => x.IsSiblingOf(coordinate)).ToList();
			return emptySiblings;
		}
		
		private CharCoordinate GetRandomEmptySibling(CharCoordinate coordinate, List<CharCoordinate> except = null)
		{
			except = except ?? new List<CharCoordinate>();
			var siblingEmptyCells = GetEmptySiblings(coordinate).Where(x => !except.Contains(x));
			
			var randomIndex = UnityEngine.Random.Range(0, siblingEmptyCells.Count());
			var nextSibling = siblingEmptyCells.ElementAt(randomIndex);
			
			return nextSibling;
		}
		
		#endregion
		
		#region empty cell search
		
		private List<CharCoordinate> GetEmptyCells()
		{
			var emptyCells = new List<CharCoordinate>();
			for (var i = 0; i < _size; i++)
			{
				for (var j = 0; j < _size; j++)
				{
					var current = new CharCoordinate(i, j);
					
					if (IsEmpty(current))
						emptyCells.Add(current);
				}
				
			}
			return emptyCells;
		}
		
		private CharCoordinate GetRandomEmptyCell()
		{
			var emptyCells = GetEmptyCells();
			var randomIndex = UnityEngine.Random.Range(0, emptyCells.Count);
			return emptyCells.ElementAt(randomIndex);
		}
		
		#endregion
		
		#region boolean checkers
		
		public bool IsTrapped(CharCoordinate coordinate, List<CharCoordinate> previouslyUsedPath = null)
		{
			previouslyUsedPath = previouslyUsedPath ?? new List<CharCoordinate>();
			var empty = GetEmptySiblings(coordinate);
			var trapped = empty.All(x => previouslyUsedPath.Contains(x));
			
			return trapped;
		}
		public bool HasEmptySibling(CharCoordinate coordinate)
		{
			var emptyCells = GetEmptyCells();
			var siblingEmptyCells = emptyCells.Where(x => x.IsSiblingOf(coordinate));
			return siblingEmptyCells.Any();
		}
		private bool IsEmpty(CharCoordinate coordinate)
		{
			return _matrix[coordinate.Row, coordinate.Column] == '\0';
		}
		
		#endregion
	}
}
