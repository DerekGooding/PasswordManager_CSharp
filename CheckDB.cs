using MySqlConnector;

namespace MySQL_PasswordManager
{
    public static class CheckDB
    {
        // Connection string set as private and read only as we don't want to access it or change it outside the class.
        private static readonly string cd = @"server=localhost;userid=root;password=y4qWs9Jst725peQ^;database=passwordmanagerdb";
        
        // Checking if the database exists. If it doesn't, returns false.
        public static bool CheckForDB()
        {
            try
            {
                using var con = new MySqlConnection(cd);
                con.Open();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
