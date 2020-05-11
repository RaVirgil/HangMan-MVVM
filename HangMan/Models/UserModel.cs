namespace HangMan.Models
{
    class UserModel
    {
        #region Properties
        private string _userName;
        private int _userScore;
        private string _userImagePath;
        #endregion

        #region Getter/Setter
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public int UserScore
        {
            get { return _userScore; }
            set { _userScore = value; }
        }
        public string UserImagePath
        {
            get { return _userImagePath; }
            set { _userImagePath = value; }
        }
        #endregion

        #region Ctor
        public UserModel()
        {

        }
        #endregion

        #region Methods
        public override bool Equals(object obj)
        {

            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                UserModel otherUser = (UserModel)obj;
                return UserName.Equals(otherUser.UserName);
            }
        }
        public override int GetHashCode()
        {
            return UserScore / 2;
        }
        public override string ToString()
        {
            return UserName + "\n" + UserScore.ToString() + "\n" + UserImagePath;
        }
        #endregion
    }
}
