using System;
using System.Collections.Generic;
using System.Text;

namespace ATM
{
    class Customer
    {
        public string fName;
        public string lName;
        public string email;
        public int pin;

        public Customer(string AfName, string AlName, string Aemail, int Apin ) //The 'A' denotes the argument 
        {
            fName = AfName;
            lName = AlName;
            email = Aemail;
            pin = Apin;

        }


    }
}
