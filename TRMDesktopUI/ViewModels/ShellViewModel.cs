using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel :
        Conductor<object>
        , IHandle<LogOnEvent>
    {
        private readonly ILoggedInUserModel user;
        private readonly IAPIHelper helper;

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(user.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool IsLoggedOut
        {
            get
            {
                return !IsLoggedIn;
            }
        }

        public ShellViewModel(
            IEventAggregator events
            , ILoggedInUserModel user
            , IAPIHelper helper)
        {
            this.user = user;
            this.helper = helper;

            events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>()
                , new CancellationToken());
        }

        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>()
                , new CancellationToken());
        }

        public async Task LogIn()
        {
            await ActivateItemAsync(IoC.Get<LoginViewModel>()
                , new CancellationToken());
        }

        public async Task LogOut()
        {
            user.ResetUserModel();
            helper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>()
                , new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => IsLoggedOut);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => IsLoggedOut);
        }
    }
}
