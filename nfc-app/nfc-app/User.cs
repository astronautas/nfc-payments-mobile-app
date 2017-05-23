using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nfc_app
{
    //Paskyra
    class User
    {
        public static User Init
        {
            get
            {
                return Init;
            }
            private set
            {

            }
        }

        public string firstName, lastName;
        public string email;
        public string password;
        public string stripeToken;
        List<User> friends;

        private User(string email, string password, string stripeToken)
        {
            this.email = email;
            this.password = password;
            this.stripeToken = stripeToken;
        }

        public static void CreateUser(string email, string password, string stripeToken)
        {
            Init = new User(email, password, stripeToken);
        }
    }
}