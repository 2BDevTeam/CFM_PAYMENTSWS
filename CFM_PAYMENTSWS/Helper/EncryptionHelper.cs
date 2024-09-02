using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using CFM_PAYMENTSWS.Persistence.Contexts;


namespace CFM_PAYMENTSWS.Helper
{
    public class EncryptionHelper
    {
        public string DecryptText(string encryptedText, string _keystamp)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetConnectionString("ConnStr");
            Debug.Print($"connectionString {connectionString}");


            var encKey = GetEncryptedKey(connectionString, _keystamp);

            if (string.IsNullOrEmpty(encKey))
                throw new Exception("Encrypted key not found.");

            var descryptKey = DecryptKey(connectionString, encKey);

            if (string.IsNullOrEmpty(descryptKey))
                throw new Exception("Decrypted key not found.");

            var decryptText = DecryptTextUsingKey(connectionString, encryptedText, descryptKey);
            //Debug.Print("FINAL" + decryptText);

            return decryptText;
        }

        private string GetEncryptedKey(string connectionString, string _keystamp)
        {
            using (var _wSCTX = new AppDbContext(GetDbContextOptions(connectionString)))
            {
                var encryptedKey = _wSCTX.Key
                    .Where(k => k.keystamp == _keystamp)
                    .Select(k => k.key)
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(encryptedKey))
                    throw new Exception("Encrypted key not found.");

                return encryptedKey;
            }
        }

        private string DecryptKey(string connectionString, string encKey)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT [dbo].[DText](@EncryptedText, @Passphrase)", connection))
                {
                    command.Parameters.AddWithValue("@EncryptedText", encKey);
                    command.Parameters.AddWithValue("@Passphrase", "Cfm1234567@");

                    object decryptedObject = command.ExecuteScalar();

                    if (decryptedObject != null && decryptedObject != DBNull.Value)
                    {
                        string decryptedText = decryptedObject.ToString();
                        Debug.Print("DESENCRYPTED KEY: " + decryptedText);
                        return decryptedText;
                    }
                    else
                    {
                        throw new Exception("Decrypted key not found.");
                    }
                }
            }
        }

        
        private string DecryptTextUsingKey(string connectionString, string encryptedText, string descryptKey)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT [dbo].[DText](@EncryptedText, @Passphrase)", connection))
                {
                    command.Parameters.AddWithValue("@EncryptedText", encryptedText);
                    command.Parameters.AddWithValue("@Passphrase", descryptKey);

                    object decryptedObject = command.ExecuteScalar();

                    if (decryptedObject != null && decryptedObject != DBNull.Value)
                    {
                        string decryptedText = decryptedObject.ToString();
                        Debug.Print("DECRYPTED TEXT: " + decryptedText);
                        return decryptedText;
                    }
                    // No need to throw an exception here as we're already returning null in this case.
                }
            }

            return null;
        }

        private DbContextOptions<AppDbContext> GetDbContextOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            /*
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = configuration.Build();
            var connString = config.GetConnectionString(connectionString);
            */
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }
    }
}