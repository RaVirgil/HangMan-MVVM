using HangMan.Models;
using HangMan.ViewModels.Commands;
using HangMan.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace HangMan.ViewModels
{
    class GameViewModel : BaseViewModel
    {
        #region Properties
        private UserModel _selectedUser;
        private GameModel _game;
        private string _gameCategory;
        private int _round;
        private int _score;
        private DispatcherTimer _timer;
        private string _timerText;

        private Grid _gameGrid;
        private List<Button> _buttons;
        private List<Label> _labels;
        private Image _stageImage;

        private ICommand _newGameCommand;
        private ICommand _categoryClickCommand;
        private ICommand _openAboutViewCommand;
        private ICommand _returnToLoginCommand;
        #endregion

        #region Getter/Setter
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }
        public GameModel Game
        {
            get { return _game; }
            set { _game = value; }
        }
        public string GameCategory
        {
            get { return _gameCategory; }
            set { _gameCategory = value; }
        }
        public int Round
        {
            get { return _round; }
            set
            {
                _round = value;
                if (_round == 5)
                    WonGame();
                OnPropertyChanged("Round");
            }
        }
        public int Score
        {
            get { return _score; }
            set { _score = value; OnPropertyChanged("Score"); }
        }
        public DispatcherTimer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }
        public string TimerText
        {
            get { return _timerText; }
            set { _timerText = value; OnPropertyChanged("TimerText"); }
        }

        public Grid GameGrid
        {
            get { return _gameGrid; }
            set { _gameGrid = value; OnPropertyChanged("GameGrid"); }
        }
        public List<Button> Buttons
        {
            get { return _buttons; }
            set { _buttons = value; }
        }
        public List<Label> Labels
        {
            get { return _labels; }
            set { _labels = value; }
        }
        public Image StageImage
        {
            get { return _stageImage; }
            set { _stageImage = value; }
        }
        public ICommand NewGameCommand
        {
            get { return _newGameCommand ?? (_newGameCommand = new CommandHandler(() => NewGame(), () => true)); }
        }
        public ICommand CategoryClickCommand
        {
            get { return _categoryClickCommand ?? (_categoryClickCommand = new CommandHandler(parameter => CategoryChanged(parameter), () => true)); }
        }
        public ICommand OpenAboutViewCommand
        {
            get { return _openAboutViewCommand ?? (_openAboutViewCommand = new CommandHandler(() => OpenAboutView(), () => true)); }
        }
        public ICommand ReturnToLoginCommand
        {
            get { return _returnToLoginCommand ?? (_returnToLoginCommand = new CommandHandler(() => ReturnToLogin(), () => true)); }
        }
        #endregion

        #region Ctor
        public GameViewModel()
        {
            Score = 0;
            Round = 0;

            GameGrid = new Grid();
            Labels = new List<Label>();
            Buttons = new List<Button>();
            Timer = new DispatcherTimer();
            TimerSetup();

            Game = new GameModel();
            Game.SelectedCategory = "All Categories";
            NewGame();

        }
        public GameViewModel(UserModel selectedUser)
        {
            this.SelectedUser = selectedUser;
            Score = 0;
            Round = 0;

            GameGrid = new Grid();
            Labels = new List<Label>();
            Buttons = new List<Button>();
            Timer = new DispatcherTimer();
            TimerSetup();

            Game = new GameModel();
            Game.SelectedCategory = "All Categories";
            NewGame();
        }
        #endregion

        #region Methods
        private void BtnClick(object sender, RoutedEventArgs e)
        {
            int[] temp = Game.TakeCharacter((sender as Button).Content.ToString()[0]);

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == 1)
                {
                    Score += 10;
                    Labels[i].Content = Game.CurrentWord[i];
                }
            }

            StageImage.Source = Game.GetStageImage();

            if (Labels.Count(l => l.Content == null) == 0)
            {
                Round++;
                Game.SelectNewWord();
                MakePlayground();
            }

            if (Game.IsGameOver())
            {
                SelectedUser.UserScore = Score;
                Utils.UpdateUser(SelectedUser);
                FinishGame("You Lose! The word was " + Game.CurrentWord);
            }

            (sender as Button).IsEnabled = false;

        }
        private void TimerSetup()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMinutes(1);
            Timer.Tick += TimerTick;
            Timer.Start();
        }
        private void TimerTick(object send, EventArgs e)
        {
            TimerText = DateTime.Now.ToString("ss");
        }
        private void CategoryChanged(object parameter)
        {
            string category = (string)parameter;
            Game.SelectedCategory = category;
            MakePlayground();
            MessageBox.Show("Category changed to " + category);
        }
        private void CreateCharacterBtns()
        {
            double bot = 0;
            int count = 0;
            for (int i = 0; i < Game.Alphabet.Length; i++, count++)
            {
                Button button = new Button();
                button.FontSize = 20;
                button.HorizontalContentAlignment = HorizontalAlignment.Center;
                button.VerticalContentAlignment = VerticalAlignment.Center;
                button.Height = button.Width = 38;
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.VerticalAlignment = VerticalAlignment.Bottom;

                button.Content = Game.Alphabet[i].ToString();

                if ((count + 1) * button.Width > GameGrid.Width)
                {
                    count = 0;
                    bot += button.Height;
                }

                button.Click += new RoutedEventHandler(BtnClick);
                button.Margin = new Thickness(count * button.Width, 0, 0, bot);

                Buttons.Add(button);

                GameGrid.Children.Add(button);
            }
        }
        private void CreateCharacterLbl()
        {
            for (int i = 0; i < Game.Lenght; i++)
            {
                Label label = new Label();
                label.FontSize = 40;

                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.BorderThickness = new Thickness(1, 1, 1, 1);
                label.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                label.Height = 70;
                label.Width = 70;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Center;



                label.Margin = new Thickness(i * label.Width, 0d, 0d, 0d);
                if (Game.CurrentWord[i] == ' ')
                    label.Content = ' ';
                Labels.Add(label);

                GameGrid.Children.Add(label);
            }
        }
        private void CreateImage()
        {
            StageImage = new Image();

            StageImage.Name = "StageImage";
            StageImage.VerticalAlignment = VerticalAlignment.Center;
            StageImage.HorizontalAlignment = HorizontalAlignment.Right;
            StageImage.Width = 300;
            StageImage.Height = 300;

            GameGrid.Children.Add(StageImage);
        }
        private void CurrentTimeText(object sender, EventArgs e)
        {
            TimerText = DateTime.Now.Second.ToString();
        }
        private void FinishGame(string message)
        {
            MessageBox.Show(message);
            Buttons.ForEach(b => b.IsEnabled = false);
        }
        private void MakePlayground()
        {
            Labels.Clear();
            Buttons.Clear();
            GameGrid.Children.Clear();

            CreateImage();
            StageImage.Source = Game.GetStageImage();

            CreateCharacterBtns();
            CreateCharacterLbl();
        }
        private void NewGame()
        {
            Score = 0;
            Round = 0;
            Game.SelectNewWord();
            MakePlayground();
            MessageBox.Show("Starting a new game!\nCategory is " + Game.SelectedCategory);
        }
        private void OpenAboutView()
        {
            AboutView view = new AboutView();
            view.Show();
        }
        private void ReturnToLogin()
        {
            SelectedUser.UserScore = Score;
            Utils.UpdateUser(SelectedUser);
            Utils.ShowMainView();
            Utils.CloseThisView();
        }
        private void WonGame()
        {
            MessageBox.Show("You won the game!");
            SelectedUser.UserScore = Score;
            Utils.UpdateUser(SelectedUser);
            Buttons.ForEach(b => b.IsEnabled = false);
        }
        #endregion
    }
}
