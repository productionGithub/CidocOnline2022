using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.ResetPassword
{
    public class ResetPasswordManager : IInitializable
    {
        [Inject] private APIService _net;
        [Inject] private ResetPasswordController _controller;
        [Inject] private NavigationService _navService;

        public void Initialize()
        {
            _controller.Show();
            _controller.OnResetPasswordEvent += CreateNewPassword;
            _controller.OnBackEvent += BackEventClicked;
        }

        private async void CreateNewPassword(string email)
        {
            var result = await CheckEmail(email);

            if (result.DoesExist == false)//False = Did not find the email in DB
            {
                _controller.NoAccountFound();
            }
            else
            {
                var status = await CheckStatus(email);
                if (status.IsActive == true)
                {
                    //Get activation code
                    ActivationCodeModelUp emailUp = new ActivationCodeModelUp
                    {
                        Email = email
                    };
                    ActivationCodeModelDown code = await _net.PostActivationCode(emailUp);

                    //Setup data to pass to php script.
                    ResetPasswordModelUp data = new ResetPasswordModelUp
                    {
                        Email = email,
                        Code = code.Code,
                        Lang = "fr"//TODO manage multilingual game. Backend OK!
                    };
                    ResetPasswordModelDown resetLinkSent = await _net.SendResetEmail(data);

                    //Alert of return value
                    if (resetLinkSent.EmailSent == true)
                    {
                        _controller.ConfirmEmailSent();
                    }
                    else
                    {
                        _controller.EmailNotSentError();
                    }
                }
                else
                {
                    _controller.StatusAlert();
                }
            }
        }

        private async UniTask<ExistValidationDown> CheckEmail(string email)
        {
            ExistValidationDown result = await _net.CheckEmail(email);
            return result;
        }

        private async UniTask<StatusModelDown> CheckStatus(string email)
        {
            StatusModelDown result = await _net.CheckStatus(email);
            return result;
        }


        private void BackEventClicked()
        {
            _navService.Pop();
        }
    }
}