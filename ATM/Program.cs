using System;
using System.Linq; //Language integrated query, specifically for .ALL in makeNewCustomer() to insure proper input
using MySql.Data.MySqlClient;


namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {

            //---- GetAccountNumber() ----  This bit of code returns the account number for a given pin;
            
            Console.Write("Enter Pin Number: ");
            string pin = Console.ReadLine();

            string connString = @"server=localhost;userid=root;password=password;database=test_atm";
            using var con = new MySqlConnection(connString);
            con.Open();


            
            string returnAcct = "SELECT account_number FROM customers WHERE pin_num = @pin_num";
            MySqlCommand getAcct = new MySqlCommand(returnAcct, con);

            getAcct.Parameters.AddWithValue("@pin_num", pin);

            var acctReturn = getAcct.ExecuteScalar();

            Console.Write($"/n/nYour account number is: {acctReturn} ");





            //Close Connections
            con.Close();
            Console.WriteLine("\nDone\nConnection Closed");

            //---- End GetAccountNumber() -----

        }





        //YOU DID IT YOU MADE A DAMN CONNECTION!
        //NEXT WE GOTTA TEST THAT IT IS ACTUALLY CONNECTING TO THE DESIRED DATABASE









        //TODO - Return Acct Number back to customer from SQL DB, opens new window to simulate an email or something like that

        public static Customer MakeNewCustomer()
    {


        /*          This method is designed to ask the user to input details to make them a new customer
         *          It is very much not secure.
         *          
         *          TODO: Make More Secure
         *          TODO: Send input info to the SQL database
         *          TODO: Launch new screen showing account number
         * 
         */

        Console.WriteLine("Welcome to the New Customer Page!");

        //A pause for aesthetics
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine(".");
            System.Threading.Thread.Sleep(500);

        }

        int pin = 0;
        string fName = "", lName = "", email = "";
        bool check1 = true, check2 = true, check3 = true, check4 = true;
        // First Name Check
        while (check1 == true)
        {

            Console.Write("Please Enter First Name: ");
            fName = Console.ReadLine();

            if (fName.All(char.IsLetter))
            {
                break;

            }
            else
            {
                Console.WriteLine("A name can only containes letter!");
            }


        }

        // Last Name Check
        while (check2 == true)
        {

            Console.Write("Please Enter Last Name: ");
            lName = Console.ReadLine();

            if (lName.All(char.IsLetter))
            {
                break;

            }
            else
            {
                Console.WriteLine("A name can only containes letter!");
            }


        }

        // Email Check
        while (check3 == true)
        {

            Console.Write("Please Enter Email: ");
            email = Console.ReadLine();

            if (email.Contains('@'))
            {
                break;

            }
            else
            {
                Console.WriteLine("Email must contain '@'");
            }


        }

        while (check4 == true)
        {
            try
            {
                Console.Write("Please Enter Your Desired PIN: ");
                pin = Convert.ToInt32(Console.ReadLine());
                break;

            }

            catch
            {
                Console.WriteLine("PIN can only contain numbers");

            }
        }

    

    Customer newCustomer = new Customer(fName, lName, email, pin);
            CommitNewCustomer(newCustomer);
            Console.WriteLine("\n\nWelcome {0} {1}!\n Your Account Number will be sent to {2}\n\n", newCustomer.fName, newCustomer.lName, newCustomer.email);


            return newCustomer;
            
        }
    
        public static void WelcomeScreen()
        {
            /*
             * 
             *      This Method works as the welcome screen. It is the starting place and points to the next part of the ATM processes
             *      This method is not secure
             *      
             *      
             */
            
            // Get Input
            Console.WriteLine("Welcome to the Bank of Brandon ATM");
            Console.WriteLine("Please Input your account Number");
            Console.WriteLine("If you are a new customer, type 'New'");
            string acctNumOrNew = Console.ReadLine();
            //Test Input
            switch (acctNumOrNew)
            {
                case "New":
                case "new":
                    MakeNewCustomer();
                    break;

                default:
                    Console.WriteLine("Validating...");
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Validating...");
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("System Down\n\n");
                    break;
                    // TODO make this actually test account number against sql table 'account'

            }
            WelcomeScreen();


        }


        public static MySqlConnection SQLAccountNumCheck()
        {

            //Use account number: 100046571
            //Connection String
            string connString = @"server=localhost;userid=root;password=password;database=test_atm";
            using var con = new MySqlConnection(connString);


            try // Statement wrapped in a try/Catch in case something goes wrong
            {

            //Opens the connection
            con.Open();
            Console.WriteLine("Connecting...");

                //SQL Command goes here 
                string sql = "SELECT * FROM CUSTOMERS WHERE account_number=@acct_num";
                MySqlCommand cmd = new MySqlCommand(sql, con);

                //Get parameter to pass to sql query
                Console.Write("\nEnter your account number: ");
                int acctNum = Convert.ToInt32(Console.ReadLine());
               
                
                    //pass to query
                    cmd.Parameters.AddWithValue("@acct_num", acctNum);

                    //Read out return
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Console.WriteLine("Acct_num ---- First Name --- Last Name --- Email -- Acct Opened --- Pin");
                        Console.WriteLine(rdr[0] + "---" + rdr[1] + "---" + rdr[2] + "---" + rdr[3] + "---" + rdr[4] + "---" + rdr[5]);

                    }
                    rdr.Close();

               

                


                    
            

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            //Close Connections
            con.Close();
            Console.WriteLine("Done\nConnection Closed");
            return con;

        }

        public static MySqlConnection CommitNewCustomer(Customer newCustomer)
        {
            //Connection String
            string connString = @"server=localhost;userid=root;password=password;database=test_atm";
            using var con = new MySqlConnection(connString);


            try // Statement wrapped in a try/Catch in case something goes wrong
            {

                //Opens the connection
                con.Open();
                Console.WriteLine("Connecting...");


                string addCust = "INSERT INTO customers (fName, lName, email, pin_num) VALUES (@fName, @lName, @email, @pin_num)";

                MySqlCommand cmd = new MySqlCommand(addCust, con);

                cmd.Parameters.AddWithValue("@fName", newCustomer.fName);
                cmd.Parameters.AddWithValue("@lName", newCustomer.lName);
                cmd.Parameters.AddWithValue("@email", newCustomer.email);
                cmd.Parameters.AddWithValue("@pin_num", newCustomer.pin);
                cmd.ExecuteNonQuery();

/* Not Sure if this needs to be here
 * 
                Console.Write("/n/nYour account number is: ");
                string returnAcct = "SELECT account_num FROM customers WHERE pin = @pin_num";
                MySqlCommand getAcct = new MySqlCommand(returnAcct, con);

                getAcct.Parameters.AddWithValue("@pin_num", newCustomer.pin);

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                   
                    Console.WriteLine(rdr[0]);

                }
                rdr.Close();
*/











            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            //Close Connections
            con.Close();
            Console.WriteLine("Done\nConnection Closed");
            return con;

        }



    }




    
}


