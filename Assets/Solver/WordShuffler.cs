using System.Collections.Generic;
using System.Text;
using System.IO;
using WordShuffler.Models;
using Models;
using UnityEngine;

namespace WordShuffler
{
    public class WordShuffler
    {
        private List<string> _words;
		private List<string> _longwords;
        private CharMatrix _matrix;
        private int _size;
        private WordDFA _dfa;

        private WordShuffler(int size)
        {
            setSize(size);
        }

        public WordShuffler(List<string> words, int size, List<string> longwords) : this(size)
        {
			setSize(size);
           setWords(words,longwords);
        }

        public void setSize(int size)
        {
            _size = size; 
        }

        private void setWords(List<string> words,List<string> longwords)
        {
            _words = words;
            _longwords = longwords;
            _dfa = new WordDFA(_words);
        }

        public ShuffleModel GetNextModel()
        {
            _matrix = new CharMatrix(_size, _words,_longwords);
            var model = new ShuffleModel(_matrix, _dfa);
            return  model;
        }
    }
}
