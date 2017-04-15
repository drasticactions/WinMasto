using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WinMasto.Services;

namespace WinMasto.ViewModels
{
    public class ShellViewModel : WinMastoViewModel
    {
        #region Properties
        //private UserAuth _user = default(UserAuth);

        //public UserAuth User
        //{
        //    get { return _user; }
        //    set
        //    {
        //        Set(ref _user, value);
        //    }
        //}
        #endregion

        #region Methods

        public void CheckLogin()
        {
            try
            {
                IsLoggedIn = SettingsService.Instance.UserAccount != null;
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion
    }
}
