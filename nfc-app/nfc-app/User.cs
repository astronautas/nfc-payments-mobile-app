using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nfc_app
{
    //Paskyra
    class User
    {
        public string firstName, lastName;
        public string email;
        public string password;
        public string stripeToken;
        List<User> friends;
    }
}