using MySqlConnector;

namespace MySQL_PasswordManager
{
    public static class MasterPassword
    {
        // Initialising some important variables.
        private static string _masterUsername = "user";
        private static string _masterPassword = "pass";
        private static readonly string conn = @"server=localhost;userid=root;password=y4qWs9Jst725peQ^;database=passwordmanagerdb";

        // Method to check if the user input username and password matches the set master username and password.
        public static bool ValidatePW(string? username, string? password)
        {
            if (username != _masterUsername || password != _masterPassword)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Method to record a breach attempt when the master username and password is input incorrectly 3 times.
        public static void BreachAttempt()
        {
            // Connecting to and opening the db.
            using var con = new MySqlConnection(conn);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;

            // Recording the current date and time.
            var now = DateTime.Now.ToString();
            string date = now[..10];
            string time = now[11..];

            // Passing the current date and time to the login_attempt table in the db in MySQL.
            cmd.CommandText = "INSERT INTO login_attempts (breach_date, breach_time) VALUES('" + date + "','" + time + "')";
            cmd.ExecuteNonQuery();
            con.Close(); // Closing the db once we're done.
        }

        public static void ViewBreachAttempt()
        {
            // Connecting to and opening the db.
            using var con = new MySqlConnection(conn);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;

            // Passing a command to MySQL to read every row. The more recent the breach, the closer to the top it is.
            string viewString = "SELECT * FROM login_attempts\nORDER BY breach_date DESC, breach_time DESC;";
            using var cmd1 = new MySqlCommand(viewString, con);
            using MySqlDataReader rdr = cmd1.ExecuteReader();

            // Displays every role in the table on the console.
            while (rdr.Read())
            {
                Console.WriteLine("Date: {0}   Time: {1}", rdr.GetString(0), rdr.GetString(1));
            }
            con.Close(); // Closing the file once we're done.
        }
    }
}
