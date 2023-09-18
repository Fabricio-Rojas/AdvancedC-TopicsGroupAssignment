using AdvancedC_TopicsGroupAssignment.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace AdvancedC_TopicsGroupAssignment.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ContactInfoContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Addresses.Any() || context.Businesses.Any() || context.Persons.Any())
            {
                return; // Database has been seeded
            }

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=ContactInfoDB;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SeedDataFromCSV(context, connection);

                connection.Close();
            }
        }

        private static void SetIdentityInsert(SqlConnection connection, SqlTransaction transaction, string tableName, bool enable)
        {
            string commandText = $"SET IDENTITY_INSERT {tableName} {(enable ? "ON" : "OFF")};";
            using (SqlCommand command = new SqlCommand(commandText, connection, transaction))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void SeedDataFromCSV(ContactInfoContext context, SqlConnection connection)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            // Define the relative paths to your CSV files from the current directory.
            string addressesCsvPath = Path.Combine(currentDirectory, "SeedData", "Addresses.csv");
            string businessesCsvPath = Path.Combine(currentDirectory, "SeedData", "Businesses.csv");
            string personCsvPath = Path.Combine(currentDirectory, "SeedData", "Person.csv");
            string businessPersonCsvPath = Path.Combine(currentDirectory, "SeedData", "BusinessPerson.csv");
            string personAddressesCsvPath = Path.Combine(currentDirectory, "SeedData", "PersonAddresses.csv");

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SetIdentityInsert(connection, transaction, "Businesses", true);
                    SeedBusinesses(context, businessesCsvPath);
                    SetIdentityInsert(connection, transaction, "Businesses", false);

                    SetIdentityInsert(connection, transaction, "Addresses", true);
                    SeedAddresses(context, addressesCsvPath);
                    SetIdentityInsert(connection, transaction, "Addresses", false);

                    SetIdentityInsert(connection, transaction, "Persons", true);
                    SeedPerson(context, personCsvPath);
                    SetIdentityInsert(connection, transaction, "Persons", false);

                    SetIdentityInsert(connection, transaction, "BusinessPersons", true);
                    SeedBusinessPersons(context, businessPersonCsvPath);
                    SetIdentityInsert(connection, transaction, "BusinessPersons", false);
                
                    SetIdentityInsert(connection, transaction, "PersonAddresses", true);
                    SeedPersonAddresses(context, personAddressesCsvPath);
                    SetIdentityInsert(connection, transaction, "PersonAddresses", false);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
        
        private static void SeedAddresses(ContactInfoContext context, string fileName)
        {
            var addresses = ReadCSV<Address, AddressMap>(fileName);
            context.Addresses.AddRange(addresses);
            context.SaveChanges();
        }

        private static void SeedBusinesses(ContactInfoContext context, string fileName)
        {
            var businesses = ReadCSV<Business, BusinessMap>(fileName);
            context.Businesses.AddRange(businesses);
            context.SaveChanges();
        }

        private static void SeedPerson(ContactInfoContext context, string fileName)
        {
            var people = ReadCSV<Person, PersonMap>(fileName);
            context.Persons.AddRange(people);
            context.SaveChanges();
        }

        private static void SeedBusinessPersons(ContactInfoContext context, string fileName)
        {
            var businessPersons = ReadCSV<BusinessPerson, BusinessPersonMap>(fileName);
            context.BusinessPersons.AddRange(businessPersons);
            context.SaveChanges();
        }

        private static void SeedPersonAddresses(ContactInfoContext context, string fileName)
        {
            var personAddresses = ReadCSV<PersonAddress, PersonAddressMap>(fileName);
            context.PersonAddresses.AddRange(personAddresses);
            context.SaveChanges();
        }


        private static List<T> ReadCSV<T, TMap>(string fileName)
            where T : class
            where TMap : ClassMap<T>
        {
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<T>().ToList();
            }
        }
    }
    public class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.StreetName).Name("StreetName");
            Map(m => m.StreetNumber).Name("StreetNumber");
            Map(m => m.UnitNumber).Name("UnitNumber");
            Map(m => m.PostalCode).Name("PostalCode");
            Map(m => m.BusinessId).Name("BusinessId");
        }
    }

    public class BusinessMap : ClassMap<Business>
    {
        public BusinessMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.Name).Name("Name");
            Map(m => m.PhoneNumber).Name("PhoneNumber");
            Map(m => m.Email).Name("Email");
        }
    }

    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.LastName).Name("LastName");
            Map(m => m.Email).Name("Email");
            Map(m => m.PhoneNumber).Name("PhoneNumber");
        }
    }

    public class BusinessPersonMap : ClassMap<BusinessPerson>
    {
        public BusinessPersonMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.BusinessId).Name("BusinessId");
            Map(m => m.PersonId).Name("PersonId");
        }
    }

    public class PersonAddressMap : ClassMap<PersonAddress>
    {
        public PersonAddressMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.PersonId).Name("PersonId");
            Map(m => m.AddressId).Name("AddressId");
        }
    }
}
