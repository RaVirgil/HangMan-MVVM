using HangMan.ViewModels.Commands;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace HangMan.ViewModels
{
    class NewUserViewModel : BaseViewModel
    {
        #region Properties
        private string _userNameInput;
        private int _selectedImageNumber;
        private string _selectedImage;

        private ICommand _closeWindowCommand;
        private ICommand _makeNewUserCommand;
        private ICommand _nextImageCommand;
        #endregion


        #region Getter/Setter
        public string UserNameInput
        {
            get { return _userNameInput; }
            set { _userNameInput = value; OnPropertyChanged("UserNameInput"); }
        }
        public int SelectedImageNumber
        {
            get { return _selectedImageNumber; }
            set { _selectedImageNumber = value; }
        }
        public string SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; OnPropertyChanged("SelectedImage"); }
        }

        public ICommand CloseWindowCommand
        {
            get { return _closeWindowCommand ?? (_closeWindowCommand = new CommandHandler(() => CloseView(), () => true)); }

        }
        public ICommand MakeNewUserCommand
        {
            get { return _makeNewUserCommand ?? (_makeNewUserCommand = new CommandHandler(() => MakeNewUser(), () => true)); }

        }
        public ICommand NextImageCommand
        {
            get { return _nextImageCommand ?? (_nextImageCommand = new CommandHandler(() => NextImage(), () => true)); }
        }
        #endregion

        #region Ctor
        public NewUserViewModel()
        {
            SelectedImageNumber = 1;
            SelectedImage = "..\\..\\UserAvatars\\1.png";
        }
        #endregion

        #region Methods
        private void MakeNewUser()
        {

            if (UserNameInput != null && UserNameInput.Length != 0)
            {
                using (StreamWriter sw = File.AppendText("..\\..\\UsersInfo.txt"))
                {

                    sw.WriteLine(UserNameInput);
                    sw.WriteLine("0");
                    sw.WriteLine(SelectedImage);

                }
                MessageBox.Show("New user created with name " + UserNameInput + "\nChanges will be made when you restart the game");
                CloseView();
                return;
            }
            MessageBox.Show("Please enter a name");


        }
        private void CloseView()
        {
            Utils.CloseThisView();
        }
        private void NextImage()
        {
            SelectedImageNumber = (SelectedImageNumber + 1) % 4;
            SelectedImage = "..\\..\\UserAvatars\\" + SelectedImageNumber.ToString() + ".png";
        }
        #endregion
    }
}
