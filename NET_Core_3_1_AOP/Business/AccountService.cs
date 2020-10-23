using NET_Core_3_1_AOP.Attribute;
using System;
using System.Threading.Tasks;

namespace NET_Core_3_1_AOP.Business
{
    public class AccountService : IAccountService
    {
        public Task Login()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Login Success");

                Console.WriteLine("Login Finish");
            });
        }

        public Task GetAll()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("GetAll");

                Console.WriteLine("GetAll finish");
            });
        }
    }

    public interface IAccountService
    {
        [Logger]
        Task Login();


        [Logger]
        Task GetAll();
    }
}
