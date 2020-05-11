using HangMan.Models;
using HangMan.ViewModels.Commands;
using HangMan.Views;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace HangMan.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        #region Properties       
        private UserModel _selectedUser;

        private ObservableCollection<UserModel> _userList = new ObservableCollection<UserModel>();

        private ICommand _openNewUserViewCommand;
        private ICommand _deleteUserCommand;
        private ICommand _openGameViewCommand;
        #endregion

        #region Getter/Setter
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }
        public ObservableCollection<UserModel> UserList
        {
            get { return _userList; }
            set { _userList = value; OnPropertyChanged("UserList"); }
        }

        public ICommand OpenGameViewCommand
        {
            get { return _openGameViewCommand ?? (_openGameViewCommand = new CommandHandler(() => OpenGameView(), () => true)); }

        }
        public ICommand OpenNewUserViewCommand
        {
            get { return _openNewUserViewCommand ?? (_openNewUserViewCommand = new CommandHandler(() => OpenNewUserView(), () => true)); }

        }
        public ICommand DeleteUserCommand
        {
            get { return _deleteUserCommand ?? (_deleteUserCommand = new CommandHandler(() => DeleteUser(), () => true)); }
        }
        #endregion

        #region Ctor
        public LoginViewModel()
        {
            UserList = Utils.GetUsersFromFile();
        }




        #endregion

        #region Methods
        private void OpenNewUserView()
        {
            NewUserView view = new NewUserView();
            view.Show();
        }
        private void OpenGameView()
        {
            if (SelectedUser != null)
            {
                GameViewModel viewModel = new GameViewModel(SelectedUser);
                GameView view = new GameView();
                view.DataContext = viewModel;
                Utils.HideMainView();
                view.Show();
                return;
            }
            MessageBox.Show("Please select an user");
        }
        private void DeleteUser()
        {
            foreach (UserModel i in UserList)
            {
                if (i.Equals(SelectedUser))
                {
                    UserList.Remove(i);
                    File.WriteAllText(Utils.UserInfoPath, "");
                    using (StreamWriter sw = File.AppendText("..\\..\\UsersInfo.txt"))
                    {
                        foreach (UserModel j in UserList)
                        {
                            sw.WriteLine(j.ToString());

                        }
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
