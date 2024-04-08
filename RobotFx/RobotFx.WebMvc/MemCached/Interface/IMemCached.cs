using RobotFx.DoMain.Models;
using RobotFx.WebMvc.Models;

namespace RobotFx.WebMvc.MemCached.Interface
{
    public interface IMemCached : IDisposable
    {
        void ExecuteSaveData(User userData);
        void ExecuteSaveUserPassword(LoginViewModel loginViewModel);
        void RemoveSavedData();
        User GetCurrentUser();
        LoginViewModel GetCurrentUserPassword();
    }
}
