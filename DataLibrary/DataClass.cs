using System.Data.SqlClient;

namespace DataLibrary
{
    public class DataClass
    {
        public void WriteDBAccount(string connStr, Operation operation)
        {
            string sql = "INSERT INTO Accounts(AccountNumber, Credit, Debit)" +
                " VALUES(@account, @credit, @debit)";

            // Operación de escritura en la tabla Accounts
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@account", operation.AccountNumber);
            command.Parameters.AddWithValue("@credit", operation.Credit);
            command.Parameters.AddWithValue("@debit", operation.Debit);
            conn.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}