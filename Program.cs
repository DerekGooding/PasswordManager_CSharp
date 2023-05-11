namespace MySQL_PasswordManager
{
    class Program
    {
        static void Main()
        {
            // Initialising important values.
            int tries = 3;
            bool correct = false;

            // Check if the database exists.
            if (!CheckDB.CheckForDB())
            {
                Console.WriteLine("Database does not exist.");
                return;
            }

            // Validating Master Password
            // User has 3 attempts to write correct master username and password.
            while (tries > 0)
            {
                Console.Write("Username: ");
                var masterInput1 = Console.ReadLine();
                Console.Write("Password: ");
                var masterInput2 = Console.ReadLine();

                if (!MasterPassword.ValidatePW(masterInput1, masterInput2))
                {
                    tries--;
                    Console.WriteLine("Incorrect username or password.");
                    Console.WriteLine("{0} attempt(s) remaining.\n", tries);
                }
                else
                {
                    Console.WriteLine("Welcome back, Nahdaa.");
                    correct = true;
                    break;
                }
            }

            // If username and password are still incorrect after 3 tries, program records it as a breach.
            if (!correct)
            {
                Console.WriteLine("Recording breach attempt.");
                MasterPassword.BreachAttempt();
                return;
            }


            // Menu for user to choose service.
            while (true)
            {
                Console.WriteLine("\nWould you like to:\n" +
                                    "- Add a password (add)\n" +
                                    "- Delete a password (delete)\n" +
                                    "- View a password (view)\n" +
                                    "- View breach attempts (breach)\n" +
                                    "- Quit (quit)");
                var userChoice = Console.ReadLine()?.ToLower();

                if (userChoice switch
                    {
                        "add" => HandleAdd(),
                        "delete" => HandleDelete(),
                        "view" => HandleView(),
                        "breach" => HandleBreach(),
                        "quit" => HandleQuit(),
                        _ => HandleInvalid()
                    }) return;
            }

        }

        //Handle options. Return true if Quit and exit the program.
        static bool HandleAdd()
        {
            while (true)
            {
                Console.Write("\nSite: ");
                var siteInput = Console.ReadLine();
                Console.Write("Username: ");
                var usernameInput = Console.ReadLine();
                Console.Write("Password: ");
                var passwordInput = Console.ReadLine();

                // Asking user for confirmation.
                Console.Write("\nSite: {0}   Username: {1}   Password: {2}\nIs this correct, or would you like to cancel? (Y/N/Cancel): ", siteInput, usernameInput, passwordInput);
                var userConfirmation = Console.ReadLine()?.ToLower();
                if (userConfirmation == "y")
                {
                    // Makes sure null values aren't passed into the method.
                    try
                    {
                        Password.AddPassword(siteInput, usernameInput, passwordInput);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Site, username or password is empty. Please try again.\n");
                        continue;
                    }
                    Console.WriteLine("Password added.");
                    break;

                }
                // If user cancels, takes them back to the menu.
                else if (userConfirmation == "cancel")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nPlease re-enter.");
                    continue;
                }
            }
            return false;
        }
        static bool HandleDelete()
        {
            while (true)
            {
                Console.Write("\nPlease enter which password you want to delete.\nYou can choose to enter the site, username or password: ");
                var deleteInput = Console.ReadLine();
                Console.Write("Is your input the site, username or password? ");
                var fieldInput = Console.ReadLine();

                try
                {
                    // Checking if the site, username, password set exists in the DB.
                    if (!Password.SearchDB(fieldInput, deleteInput))
                    {
                        Console.WriteLine("That {0} does not exist. Please try again.\n", fieldInput);
                        continue;
                    }
                }
                catch (Exception)
                {
                    // Catches if null values are passed or if the site/username/password field is entered incorrectly.
                    Console.WriteLine("Site, username or password is empty or input does not exist. Please try again.\n");
                    continue;
                }

                // Displays site, username and password so that user can check what they are deleting.
                Console.WriteLine();
                Password.ViewPassword(fieldInput, deleteInput);

                // Asking for user confirmation.
                Console.Write("Is this correct, or would you like to cancel? (Y/N/Cancel): ");
                var userConfirmation = Console.ReadLine()?.ToLower();
                if (userConfirmation == "y")
                {
                    Password.DeletePassword(fieldInput, deleteInput);
                    Console.WriteLine("Password deleted.");
                    break;

                }
                else if (userConfirmation == "cancel")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nPlease re-enter.");
                    continue;
                }
            }
            return false;
        }
        static bool HandleView()
        {
            while (true)
            {
                Console.Write("\nPlease enter which password you want to view.\nYou can choose to enter the site, username or password: ");
                var input = Console.ReadLine();
                Console.Write("Is your input the site, username or password? ");
                var field = Console.ReadLine();

                try
                {
                    // Checking if the site, username, password row set exists in the DB.
                    if (!Password.SearchDB(field, input))
                    {
                        Console.WriteLine("That {0} does not exist. Please try again.\n", field);
                        continue;
                    }
                }
                catch (Exception)
                {
                    // Catches if null values are passed or if the site/username/password field is entered incorrectly.
                    Console.WriteLine("Site, username or password is empty or input does not exist. Please try again.\n");
                    continue;
                }
                Console.WriteLine();
                Password.ViewPassword(field, input);
                break;
            }
            return false;
        }
        static bool HandleBreach()
        {
            Console.WriteLine();
            Console.WriteLine("Breach attempts: ");
            MasterPassword.ViewBreachAttempt();
            return false;
        }
        static bool HandleQuit()
        {
            Console.WriteLine();
            Console.WriteLine("Thank you for using Password Manager.");
            return true;
        }
        static bool HandleInvalid()
        {
            Console.WriteLine("Invalid input, please try again.\n");
            return false;
        }
    }
}


