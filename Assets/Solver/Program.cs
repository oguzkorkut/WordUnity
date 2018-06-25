using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using WordShuffler.Models;
using System;
using System.IO;

namespace WordShufflerConsoleTest
{
	public class Program : MonoBehaviour
	{
		public const int size = 4;
		public static int minimumwords=300;
		public static int mineasywords=10;
		public static int maxeasywords=90;
		
		public static int iterationmax=10;
		
		public WordShuffler.WordShuffler	Dictionary;
		public char[,]						matrix;
		public ShuffleModel					model;
		public string						datapath;
		
		public static string[]		lettersarray		=	new string[iterationmax];
		public static int[]		wordslength			=	new int[size*size];
		public List<int>[]	totalwordslength	=	new List<int>[iterationmax];
		public StringBuilder	convertedmatrix	=	new StringBuilder(size*size);
		
		public int		wordscount;
		public int		totalwordscount;
		public int		iterationcount;
		
		public float	averagewordsno;
		public float	medianwordsno;
		public int		averagewordslength;
		public int		medianwordslength;
		
		public int		minword;
		public int		maxword;
		
		public float	sum1;
		public float	standarddeviation;
		
		public int easywordsinset;
		
		public static Program Instance;
		
		public void Awake()
		{
			Instance = this;
		}
		
		public void Generate()
		{
			for(int i=0;i<size*size;i++)
			{
				wordslength[i]=0;
				convertedmatrix.Append(" ");
			}
			
			wordscount=0;
			totalwordscount=0;
			iterationcount=0;
			
			averagewordsno=0;
			medianwordsno=0;
			averagewordslength=0;
			medianwordslength=0;
			
			minword=0;
			maxword=0;
			
			sum1=0;
			
			easywordsinset=0;
			
			int[] counttable	= new int[iterationmax];
			
			LoadDictionary();
			
			while(iterationcount	<	iterationmax)
			{
				GetChars();
				GetWords();
				
				if((wordscount>minimumwords)
				&&(easywordsinset>mineasywords)
				&&(easywordsinset<maxeasywords))
				{
					if(minword==0)
					{
						minword=wordscount;
					}
					if(wordscount<minword)
					{
						minword=wordscount;
					}
					
					if(wordscount>maxword)
					{
						maxword=wordscount;
					}
					
					totalwordscount+=wordscount;
					counttable[iterationcount]	=	wordscount;
					
					for(int i=0;i<model.MatrixWords.Count;i++)
					{
						wordslength[model.MatrixWords[i].Word.Length]+=1;
					}
					
					iterationcount+=1;
					ConvertMatrix();
					lettersarray[iterationcount-1]	=	convertedmatrix.ToString();
				}
				easywordsinset=0;
			}
			
			SaveSet();
			
			Array.Sort(counttable);
			
			medianwordsno	=	((float)counttable[iterationmax/2]	+	(float)counttable[iterationmax/2+1])	/2.0f;
			averagewordsno	=	(float)totalwordscount	/	(float)iterationcount;
			
			for(int a=0;a<iterationmax;a++)
			{
				sum1+=	((float)counttable[a] - averagewordsno)	* ((float)counttable[a] - averagewordsno);
			}
		
			standarddeviation	=	Mathf.Sqrt((sum1/(float)iterationmax));
			
			Debug.Log	("Iterations: " 		+ iterationcount);
			Debug.Log	("Min Words Amount: " 	+ minword);
			Debug.Log	("Max Words Amount: " 	+ maxword);
			Debug.Log	("Average Words Amount: " 	+ (int)averagewordsno);
			Debug.Log	("Median Words Amount: "	+ (int)medianwordsno);
			Debug.Log 	("Standard Deviation: "		+ (int)standarddeviation);
			Debug.Log	("Words Average Lengths: ");
			
			for(int i=0;i<size*size;i++)
			{
				Debug.Log ((i+1) + ": " + (float)wordslength[i] / (float)iterationcount);
			}
		}
		
		public void ConvertMatrix()
		{
			int tempa=0;			
			for(int a=0;a<size;a++)
			{
				for(int b=0;b<size;b++)
				{
					convertedmatrix[tempa] = matrix[a,b];
					tempa+=1;
				}
			}
		}
		
		public void LoadDictionary()
		{
			System.Collections.Generic.List<string> list	=	new List<string>(tenthousand.tenthousandwords.Keys);
			
			System.Collections.Generic.List<string>	listlong =	new	List<string>(longwords2.longset);
			
			Dictionary	=	new WordShuffler.WordShuffler(list,size,listlong);
		}
		
		public bool CheckWord(string word)
		{
			bool value;
			if(commonwords.glswords.TryGetValue(word.ToLower(),out value))
			{
				return true;
			}
			else
				return false;
		}
		
		public void GetWords()
		{
			wordscount	=	model.MatrixWords.Count;
			
			for(int a=0;a<model.MatrixWords.Count;a++)
			{
				var word	=	model.MatrixWords.ElementAt(a);
				var pathString	=	"";
				
				for(int b=0;b<word.Path.Length;b++)
				{
					var charCoordinate = word.Path.ElementAt(b);
					pathString += charCoordinate.ToString() + ", ";
				}
				
				pathString = pathString.Substring(0,pathString.Length - 2);
				
				if(CheckWord(word.Word))
				{
					easywordsinset+=1;
				}
			}
		}
		
		public void GetChars()
		{
			model	=	Dictionary.GetNextModel();
			matrix	=	model.Matrix.GetMatrix();
		}
		
		public string FileName	=	"generatedset.cs";
		public StreamWriter fileWriter = null;
		public StreamReader fileReader = null;
		
		public void SaveSet()
		{
			string path = "Assets/";
			string fileName = path + FileName;
			
			fileWriter = File.CreateText(fileName);
			
			if(System.IO.File.Exists(fileName))
			{
				fileWriter.WriteLine	("using UnityEngine;");
				fileWriter.WriteLine	("using System.Collections;");
				fileWriter.WriteLine	("using System.Collections.Generic;");
				fileWriter.WriteLine	("public class generatedset {");
				fileWriter.WriteLine	("public static readonly string[] set = new string[]{");
				
				for(int i=0; i<lettersarray.Length; i++)
				{
					fileWriter.WriteLine('"' + lettersarray[i] + '"' + ',');
				}
				
				fileWriter.WriteLine("};}");
				fileWriter.Close();
			}
		}
	}
}
