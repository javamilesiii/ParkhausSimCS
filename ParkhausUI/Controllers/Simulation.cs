//using ParkhausUI.Models;
//using ParkhausUI.Views;
//using Barrier = ParkhausUI.Models.Barrier;

//namespace ParkhausUI.Controllers
//{
//    public class Simulation
//    {
//        public PrintHandler PrintHandler { get; set; }
//        public void StartSimulation(CarParc carParc)
//        {
//            Console.WriteLine("Welcome to the Parkhaus Simulator\n-----------------------------------");

//            while (true)
//            {
//                Console.WriteLine(@"What do you want to do?

//1. Enter the parking garage

//2. Pay your ticket

//3. Exit the parking garage

//4. Show current status

//5. Exit the simulation");

//                if (int.TryParse(Console.ReadLine(), out int answer))
//                {
//                    switch (answer)
//                    {
//                        case 1:
//                            EnterParkingGarage(carParc);
//                            break;
//                        case 2:
//                            PayTicket(carParc);
//                            break;
//                        case 3:
//                            ExitParkingGarage(carParc);
//                            break;
//                        case 4:
//                            break;
//                        case 5:
//                            return;
//                        default:
//                            Console.WriteLine("Invalid option. Please try again.");
//                            break;
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Invalid input. Please enter a number.");
//                }
//            }
//        }

//        private void EnterParkingGarage(CarParc carParc)
//        {
//            if (carParc.GetFreeSpaces() == 0)
//            {
//                Console.WriteLine("Sorry, the parking garage is full!");
//                return;
//            }

//            carParc.EntranceBarrier.PassThroughBarrierAsync();
//            PrintHandler.PrintTicket(carParc);

//            Console.WriteLine($"Which floor do you want to enter? (1-{carParc.Floors.Length})");
//            if (int.TryParse(Console.ReadLine(), out int floor))
//            {
//                if (floor < 1 || floor > carParc.Floors.Length)
//                {
//                    Console.WriteLine("Invalid floor number.");
//                    return;
//                }

//                if (carParc.Floors[floor - 1].CalculateFreeSpaces() == 0)
//                {
//                    Console.WriteLine("Sorry, this floor is full! Please choose another floor.");
//                    return;
//                }

//                Console.WriteLine("\n\nWhich spot do you want to park?\n");
//                PrintHandler.PrintFloor(carParc.Floors[floor - 1]);

//                if (int.TryParse(Console.ReadLine(), out int spot))
//                {
//                    if (spot < 1 || spot > carParc.Floors[floor - 1].spots.Length)
//                    {
//                        Console.WriteLine("Invalid spot number.");
//                        return;
//                    }

//                    if (carParc.Floors[floor - 1].spots[spot - 1].occupied)
//                    {
//                        Console.WriteLine("This spot is already occupied! Please choose another spot.");
//                        return;
//                    }

//                    carParc.Floors[floor - 1].spots[spot - 1].occupied = true;
//                    Console.WriteLine("\n\nYou have successfully parked your car.");
//                }
//                else
//                {
//                    Console.WriteLine("Invalid spot number.");
//                }
//            }
//            else
//            {
//                Console.WriteLine("Invalid floor number.");
//            }
//        }

//        private void ExitParkingGarage(CarParc carParc)
//        {
//            Console.WriteLine("\n\nIn which floor is your car parked?\n");
//            if (!int.TryParse(Console.ReadLine(), out int floor) || floor < 1 || floor > carParc.Floors.Length)
//            {
//                Console.WriteLine("Invalid floor number.");
//                return;
//            }

//            Console.WriteLine("\n\nWhich spot is your car parked in?\n");
//            PrintHandler.PrintFloor(carParc.Floors[floor - 1]);

//            if (!int.TryParse(Console.ReadLine(), out int spot) || spot < 1 || spot > carParc.Floors[floor - 1].spots.Length)
//            {
//                Console.WriteLine("Invalid spot number.");
//                return;
//            }

//            if (!carParc.Floors[floor - 1].spots[spot - 1].occupied)
//            {
//                Console.WriteLine("The selected parking spot is not occupied. Please check your input.");
//                return;
//            }

//            Console.WriteLine(@"

//Which Exit do you want to take?
//1. Exit Barrier 1
//2. Exit Barrier 2
//");
//            if (!int.TryParse(Console.ReadLine(), out int exit) || (exit != 1 && exit != 2))
//            {
//                Console.WriteLine("Invalid exit number.");
//                return;
//            }

//            Console.WriteLine("\n\nPlease enter your ticket number:\n"); 
//            string ticketNumber = Console.ReadLine();
//            if (string.IsNullOrWhiteSpace(ticketNumber))
//            {
//                Console.WriteLine("Invalid ticket number.");
//                return;
//            }

//            try
//            {
//                Ticket ticket = carParc.TicketMachine.GetTicketById(ticketNumber);

//                Barrier exitBarrier = carParc.ExitBarrier;

//                if (carParc.TicketMachine.GetTicketById(ticketNumber).IsPaid)
//                {
//                    exitBarrier.PassThroughBarrierAsync();
//                    carParc.Floors[floor - 1].spots[spot - 1].occupied = false;
//                    carParc.TicketMachine.RemoveTicket(ticket);
//                    Console.WriteLine("Thank you for using our parking garage!");
//                }
//                else
//                {
//                    Console.WriteLine("Ticket not paid. Please pay your ticket first.");
//                }
//            }
//            catch (TicketNotFoundException)
//            {
//                Console.WriteLine("Invalid ticket number. Please try again.");
//            }
//        }

//        private void PayTicket(CarParc carParc)
//        {
//            Console.WriteLine("\n\nPlease enter your ticket number:\n");
//            string ticketNumber = Console.ReadLine();
//            if (string.IsNullOrWhiteSpace(ticketNumber))
//            {
//                Console.WriteLine("Invalid ticket number.");
//                return;
//            }

//            try
//            {
//                Ticket ticket = carParc.TicketMachine.GetTicketById(ticketNumber);
//                DateTime currentTime = DateTime.Now;

//                float price = carParc.Floors[0].cashDesk.CalculateParkingPrice(ticket, currentTime);

//                Console.WriteLine($"Parking fee: {price:F2} CHF");
//                Console.WriteLine("Do you want to pay? (1=Yes, 2=No)");

//                if (int.TryParse(Console.ReadLine(), out int payChoice))
//                {
//                    if (payChoice == 1)
//                    {
//                        carParc.Floors[0].cashDesk.PayTicket(ticketNumber);
//                        Console.WriteLine("Payment successful! You can now exit the parking garage.");
//                    }
//                    else
//                    {
//                        Console.WriteLine("Payment cancelled.");
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Invalid choice.");
//                }
//            }
//            catch (TicketNotFoundException)
//            {
//                Console.WriteLine("Invalid ticket number. Please try again.");
//            }
//        }
//    }
//}
