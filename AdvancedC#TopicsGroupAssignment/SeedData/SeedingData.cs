using AdvancedC_TopicsGroupAssignment.Models;

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

            SeedDataFromCSV(context);
        }

        private static void SeedDataFromCSV(ContactInfoContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            // Define the relative paths to your CSV files from the current directory.
            string addressesCsvPath = Path.Combine(currentDirectory, "SeedData", "Addresses.csv");
            string businessesCsvPath = Path.Combine(currentDirectory, "SeedData", "Businesses.csv");
            string personCsvPath = Path.Combine(currentDirectory, "SeedData", "Person.csv");
            string businessPersonCsvPath = Path.Combine(currentDirectory, "SeedData", "BusinessPerson.csv");
            string personAddressesCsvPath = Path.Combine(currentDirectory, "SeedData", "PersonAddresses.csv");

            SeedBusinesses(context, businessesCsvPath);
            SeedAddresses(context, addressesCsvPath);
            SeedPerson(context, personCsvPath);
            SeedBusinessPersons(context, businessPersonCsvPath);
            SeedPersonAddresses(context, personAddressesCsvPath);
        }

        private static void SeedBusinesses(ContactInfoContext context, string fileName)
        {
            List<Business> businesses = new List<Business>();

            StreamReader reader = new StreamReader(fileName);
            string text = reader.ReadToEnd();
            string[] lines = text.Split("\r\n");

            foreach (string data in lines)
            {
                if (data == lines[0]) continue;

                string[] columns = data.Split(',');

                Business business = new Business()
                {
                    Name = columns[1],
                    PhoneNumber = columns[2],
                    Email = columns[3],
                };

                businesses.Add(business);
            }

            context.Businesses.AddRange(businesses);
            context.SaveChanges();
        }

        private static void SeedAddresses(ContactInfoContext context, string fileName)
        {
            List<Address> addresses = new List<Address>();

            StreamReader reader = new StreamReader(fileName);
            string text = reader.ReadToEnd();
            string[] lines = text.Split("\r\n");

            foreach (string data in lines)
            {
                if (data == lines[0]) continue;

                string[] columns = data.Split(',');

                bool hasUnit = int.TryParse(columns[3], out int unit);

                Address address = new Address()
                {
                    StreetName = columns[1],
                    StreetNumber = int.Parse(columns[2]),
                    UnitNumber = hasUnit ? unit : null,
                    PostalCode = columns[4],
                    BusinessId = int.Parse(columns[5])
                };

                addresses.Add(address);
            }

            context.Addresses.AddRange(addresses);
            context.SaveChanges();
        }

        private static void SeedPerson(ContactInfoContext context, string fileName)
        {
            List<Person> people = new List<Person>();

            StreamReader reader = new StreamReader(fileName);
            string text = reader.ReadToEnd();
            string[] lines = text.Split("\r\n");

            foreach (string data in lines)
            {
                if (data == lines[0]) continue;

                string[] columns = data.Split(',');

                Person person = new Person()
                {
                    FirstName = columns[1],
                    LastName = columns[2],
                    Email = columns[3],
                    PhoneNumber = columns[4]
                };

                people.Add(person);
            }

            context.Persons.AddRange(people);
            context.SaveChanges();
        }

        private static void SeedBusinessPersons(ContactInfoContext context, string fileName)
        {
            List<BusinessPerson> businessPeople = new List<BusinessPerson>();

            StreamReader reader = new StreamReader(fileName);
            string text = reader.ReadToEnd();
            string[] lines = text.Split("\r\n");

            foreach (string data in lines)
            {
                if (data == lines[0]) continue;

                string[] columns = data.Split(',');

                BusinessPerson businessPerson = new BusinessPerson()
                {
                    BusinessId = int.Parse(columns[1]),
                    PersonId = int.Parse(columns[2])
                };

                businessPeople.Add(businessPerson);
            }

            context.BusinessPersons.AddRange(businessPeople);
            context.SaveChanges();
        }

        private static void SeedPersonAddresses(ContactInfoContext context, string fileName)
        {
            List<PersonAddress> personAddresses = new List<PersonAddress>();

            StreamReader reader = new StreamReader(fileName);
            string text = reader.ReadToEnd();
            string[] lines = text.Split("\r\n");

            foreach (string data in lines)
            {
                if (data == lines[0]) continue;

                string[] columns = data.Split(',');

                PersonAddress personAddress = new PersonAddress()
                {
                    PersonId = int.Parse(columns[1]),
                    AddressId = int.Parse(columns[2])
                };

                personAddresses.Add(personAddress);
            }

            context.PersonAddresses.AddRange(personAddresses);
            context.SaveChanges();
        }
    }
}
