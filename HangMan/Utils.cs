using HangMan.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace HangMan
{
    class Utils
    {
        #region Properties
        public const string UserInfoPath = "..\\..\\UsersInfo.txt";
        #endregion

        #region Static Methods
        public static ObservableCollection<UserModel> GetUsersFromFile()
        {
            ObservableCollection<UserModel> userList = new ObservableCollection<UserModel>();
            string[] file = File.ReadAllLines(UserInfoPath);

            for (int i = 0; i < file.Length; i += 3)
                userList.Add(new UserModel
                {
                    UserName = file[i],
                    UserScore = int.Parse(file[i + 1]),
                    UserImagePath = file[i + 2]
                }); ;

            return userList;
        }
        public static void CloseThisView()
        {
            Application.Current.Windows[Application.Current.Windows.Count - 1].Close();
        }
        public static void HideMainView()
        {
            Application.Current.MainWindow.Hide();

        }
        public static void ShowMainView()
        {
            Application.Current.MainWindow.Show();
        }
        public static void CloseAllViews()
        {
            Application.Current.Shutdown();
        }
        public static void UpdateUser(UserModel user)
        {
            ObservableCollection<UserModel> userList = new ObservableCollection<UserModel>();
            string[] file = File.ReadAllLines(UserInfoPath);

            for (int i = 0; i < file.Length; i += 3)
                userList.Add(new UserModel
                {
                    UserName = file[i],
                    UserScore = int.Parse(file[i + 1]),
                    UserImagePath = file[i + 2]
                });

            foreach (UserModel i in userList)
            {
                if (i.Equals(user))
                {
                    if (i.UserScore < user.UserScore)
                    {
                        i.UserScore = user.UserScore;
                        File.WriteAllText(UserInfoPath, "");
                        using (StreamWriter sw = File.AppendText(UserInfoPath))
                        {
                            foreach (UserModel j in userList)
                            {
                                sw.WriteLine(j.ToString());

                            }
                            return;
                        }
                    }
                }
            }

        }
        #endregion
    }
}
