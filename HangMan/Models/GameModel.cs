using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace HangMan.Models
{
    class GameModel
    {
        #region Properties
        public string[] Words { get; private set; }
        public string CurrentWord { get; set; }
        public int Lenght { get; private set; }
        private int Stage { get; set; }

        private string _selectedCategory = "All Categories";

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { ChangeCategory(); _selectedCategory = value; }
        }
        public char[] Alphabet { get; private set; }
        #endregion

        #region Ctor
        public GameModel()
        {
            InitializeAlphabet();
        }
        #endregion

        #region Methods
        public void SelectNewWord()
        {
            Random rand = new Random();
            int randomindex = rand.Next(Words.Length);

            CurrentWord = Words[randomindex];

            Lenght = CurrentWord.Length;
            Stage = 0;
        }
        public void ChangeCategory()
        {
            Words = File.ReadAllLines("..\\..\\WordCategories\\" + SelectedCategory + ".txt");
            Random rand = new Random();
            int randomindex = rand.Next(Words.Length);
            CurrentWord = Words[randomindex];
            Lenght = CurrentWord.Length;
            Stage = 0;
        }
        private void InitializeAlphabet()
        {
            Alphabet = new char[] {'A', 'B', 'C', 'D', 'E',
            'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W',
            'X', 'Y', 'Z'};
        }
        public BitmapImage GetStageImage()
        {
            return new BitmapImage(
                new Uri("..\\..\\GameStages\\"+Stage.ToString()+".png",UriKind.Relative));
        }
        public int[] TakeCharacter(char ch)
        {
            int[] temp = new int[CurrentWord.Length];

            for (int i = 0; i < CurrentWord.Length; i++)
            {
                if (CurrentWord.ToUpper()[i] == ch)
                {
                    temp[i] = 1;
                }
                else
                {
                    temp[i] = 0;
                }
            }

            if (temp.Count(i => i == 1) == 0)
            {
                Stage++;
            }

            return temp;
        }
        public bool IsGameOver()
        {
            return Stage == 9 ? true : false;
        }
        #endregion
    }


}
