using BFuel.Domain.Models;
using BFuel.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BFuel.Servicos
{
    public class RegisterService
    {
        public string Message {get; set;}
        public bool IsSucessful;

        public async Task Register(string nome, string email, string senha, int userType)
        {
            UserService _service = new UserService();

            string _message = "Sucesso!";

            User user = new User()
            {
                Name = nome,
                Email = email,
                Password = senha,
                TotalExpenses_Config = 1,
                TotalSupplied_Config = 1,
                UserType = userType
            };

            ResponseService<User> responseService = await _service.AddUser(user);

            IsSucessful = responseService.IsSucess;

            if (!responseService.IsSucess)
            {
                if (responseService.StatusCode == 400)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var dicKey in responseService.Errors)
                    {
                        foreach (var message in dicKey.Value)
                        {
                            sb.AppendLine(message);
                        }
                    }

                    _message = sb.ToString();
                }
                else
                    _message = "Ocorreu um erro inesperado! Tente novamente mais tarde";
            }
            else
            {
                App.Current.Properties.Add("User", JsonConvert.SerializeObject(responseService.Data));
                await App.Current.SavePropertiesAsync();
                App.Current.MainPage = new Paginas.Inicial();
            }

            Message = _message;
        }
    }
}
