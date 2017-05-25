using System;
using System.Collections.Generic;

namespace nfc_app
{
    //Paskyra
    [Serializable]
    class User
    {
        [NonSerialized]
        public string firstName;
        [NonSerialized]
        public string lastName;
        public string email;
        [NonSerialized]
        public string password;
        public string stripeToken;
        [NonSerialized]
        List<User> friends;

        public User(string email, string password, string stripeToken)
        {
            this.email = email;
            this.password = password;
            this.stripeToken = stripeToken;
        }
    }
}