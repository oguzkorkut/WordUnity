//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using WordShuffler.Models;
using System;

namespace WordShufflerConsoleTest
{
	public class RandomSet
	{
		public static int size = 4;
		public static int setsarraysize=10;
		
		public static void GetData()
		{
			int randomset = UnityEngine.Random.Range(0,setsarraysize);
			
			for(int i=0;i<size*size;i++)
			{
				GameController.templetters[i]=generatedset.set[randomset][i].ToString();
			}
		}
	}
}
