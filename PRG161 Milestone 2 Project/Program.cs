namespace PRG161_Milestone_2_Project
{
    internal class Program
    {

        enum Menu // Musawenkosi
        {
            AddCustomerProcess = 1,
            AddTitleProcess,
            RentToCustomer,
            Exit
        }

        // Global variables for Customer and Registration \\

        public static int RegistrationYear;
        public static int Rentals;
        public static double TotalSpend;
        public static double Discount;

        // Processes \\

        static void AddCustomerProcess() // Primrose
        {
            Console.Write("Enter Customer Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Registration Year: ");
            int regYear = int.Parse(Console.ReadLine());
            customers.Add((name, regYear, 0, 0.0));

            Console.WriteLine();
            Console.WriteLine("Customer has been added!");

        }

        static void AddTitleProcess() // Archie
        {
            Console.Write("Enter Title Name: ");
            string titleName = Console.ReadLine();
            Console.Write("Enter Title Type (VHS/CD/DVD): ");
            string titleType = Console.ReadLine();

            double price;
            if (titleType.ToUpper() == "VHS")
            {
                price = 40.0;
            }
            else
            {
                price = 25.0;
            }
  
            titles.Add((titleName, titleType, price));

            Console.WriteLine();
            Console.WriteLine("Title has been added!");
        }

        static void RentToCustomer() // Musawenkosi
        {
            Console.Write("Enter customer name: ");
            string customerName = Console.ReadLine();

            int customerIndex = -1;
            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].name == customerName)
                {
                    customerIndex = i;
                    break;
                }
            }

            if (customerIndex == -1) // To check if customer was found
            {
                Console.WriteLine("Customer not found");

                return;
            }

            Console.Write("Enter Title(s) to rent: ");
            string addTitles = Console.ReadLine();
            List<string> titleNames = new List<string>(addTitles.Split(','));

            double totalCost = 0.00;
            for (int i = 0; i < titleNames.Count; i++)
            {
                string title = titleNames[i];
                int titleIndex = titles.FindIndex(t => t.titleName == title);
                if (titleIndex != -1)
                {
                    totalCost += titles[titleIndex].price; 
                    var customer = customers[customerIndex]; /* 'var' is an implicitly typed local variable (https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/implicitly-typed-local-variables) */

                    customer.rentals++;
                    customer.totalSpend += titles[titleIndex].price;
                    customers[customerIndex] = customer;
                }

                else
                {
                    Console.WriteLine($"{title} not found.");
                }
            }

            ApplyDiscount(customerIndex); // Calculation methods invoked in RentToCustomer proccess
            ApplyRewards(customerIndex);
            ApplyLostMediaReward(customerIndex);

            Console.WriteLine($"Total renting cost: {totalCost}");

        }

        // Calculations \\

        static void ApplyDiscount(int customerIndex) // Musawenkosi
        {

            int currentYear = DateTime.Now.Year;
            int yearsRegistered = currentYear - customers[customerIndex].regYear;

            double discountRate = 0;

            if (yearsRegistered >= 15)
            {
                discountRate = 0.35;
            }
            else if (yearsRegistered >= 10)
            {
                discountRate = 0.20;
            }
            else if (yearsRegistered >= 5)
            {
                discountRate = 0.10;
            }
            else
            {
                discountRate = 0.05;
            }

            double discountAmount = discountRate * customers[customerIndex].totalSpend;

            customers[customerIndex] = (customers[customerIndex].name, customers[customerIndex].regYear, customers[customerIndex].rentals, customers[customerIndex].totalSpend - discountAmount);

            Console.WriteLine("Discount has been applied!");
            Console.WriteLine(discountAmount);
        }

        static void ApplyRewards(int customerIndex) // Archie
        {
            int rentals = customers[customerIndex].rentals;
            int freeRentals = 0;

            if (rentals >= 75)
            {
                freeRentals = 8;
            }
            else if (rentals >= 50)
            {
                freeRentals = 4;
            }
            else if (rentals >= 25)
            {
                freeRentals = 2;
            }
            else if (rentals >= 10)
            {
                freeRentals = 1;
            }
            else
            {
                freeRentals = 0; // In case rentals are less than 10
            }

            Console.WriteLine("Free rentals aquired: " + freeRentals);
        }

        static void ApplyLostMediaReward(int customerIndex) // Primrose
        {
            int yearsRegistered = DateTime.Now.Year - customers[customerIndex].regYear;
            int rentals = customers[customerIndex].rentals;

            if (yearsRegistered >= 15 && rentals >= 75)
            {
                Console.WriteLine("Lost Media Reward: 5 Bronze-tier + 2 Silver-tier + 1 Gold-tier");
            }
            else if (yearsRegistered >= 10 && rentals >= 50)
            {
                Console.WriteLine("Lost Media Reward: 3 Bronze-tier + 1 Silver-tier");
            }
            else if (yearsRegistered >= 5 && rentals >= 25)
            {
                Console.WriteLine("Lost Media Reward: 1 Bronze-tier");
            }
        }

        static List<(string name, int regYear, int rentals, double totalSpend)> customers = new List<(string, int, int, double)>();
        static List<(string titleName, string titleType, double price)> titles = new List<(string, string, double)>(); // Global Lists using tuple datastructure to store different datatypes together.

        static void Main(string[] args)
        {

            Menu option = new Menu();

            do 
            {
                // Options
                Console.WriteLine();
                Console.WriteLine("1. Add customer.");
                Console.WriteLine("2. Add a title (VHS/CD/DVD).");
                Console.WriteLine("3. Rent to customer.");
                Console.WriteLine("4. Exit.");
                Console.WriteLine();

                Console.Write("Choose option: ");
                option = (Menu)int.Parse(Console.ReadLine());

                switch (option)
                {
                    case Menu.AddCustomerProcess:

                       AddCustomerProcess();

                        break;

                    case Menu.AddTitleProcess:

                        AddTitleProcess();

                        break;

                    case Menu.RentToCustomer:

                        RentToCustomer();

                        break;

                    case Menu.Exit:

                        Console.WriteLine("Thank you for choosing Rewind!");

                        Environment.Exit(0);

                        break;

                    default:

                        Console.WriteLine("Invalid Option. Please try again");

                        break;

                }


            } while (option != Menu.Exit);

            Console.ReadKey();
        }
    }
}
