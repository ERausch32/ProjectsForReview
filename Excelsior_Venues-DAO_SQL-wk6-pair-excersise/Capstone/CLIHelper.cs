using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// This class contains helper methods that should help get valid input from users.
    /// </summary>
    public static class CLIHelper
    {
        public static int GetInteger(string message)
        {
            string userInput;
            int intValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;
        }

        public static double GetDouble(string message)
        {
            string userInput;
            double doubleValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!double.TryParse(userInput, out doubleValue));

            return doubleValue;
        }

        public static bool GetBool(string message)
        {
            string userInput;
            bool boolValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
//  Added functionality to GetBool to work with Y & N.
                if (userInput.ToUpper() == "Y")
                {
                    userInput = "true";
                }
                if (userInput.ToUpper() == "N")
                {
                    userInput = "false";
                }

            }
            while (!bool.TryParse(userInput, out boolValue));

            return boolValue;
        }

        public static string GetString(string message)
        {
            string userInput;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (string.IsNullOrEmpty(userInput));

            return userInput;
        }

        public static DateTime GetDate(string message)
        {
            string userInput;
            int numberOfAttempts = 0;
            DateTime dateTimeValue;
            bool goodDate = false;
            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input. Please enter a future date as yyyy-mm-dd");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
                goodDate = DateTime.TryParse(userInput, out dateTimeValue);
                if (goodDate && dateTimeValue.Date < DateTime.Now.Date)
                {
                 goodDate = false;
                }
            }
            while (!goodDate);

            return dateTimeValue;
        }
        /// <summary>
        /// Given a month as integer, return a text abbreviation.
        /// </summary>
        public static string GetStringMonth(int month)
        {
            string monthString = "";
            switch (month)
            {
                case 1:
                    monthString = "Jan.";
                    break;
                case 2:
                    monthString = "Feb.";
                    break;
                case 3:
                    monthString = "Mar.";
                    break;
                case 4:
                    monthString = "Apr.";
                    break;
                case 5:
                    monthString = "May.";
                    break;
                case 6:
                    monthString = "Jun.";
                    break;
                case 7:
                    monthString = "Jul.";
                    break;
                case 8:
                    monthString = "Aug.";
                    break;
                case 9:
                    monthString = "Sep.";
                    break;
                case 10:
                    monthString = "Oct.";
                    break;
                case 11:
                    monthString = "Nov.";
                    break;
                case 12:
                    monthString = "Dec.";
                    break;
                case 0:
                    monthString = "";
                    break;
            }
            return monthString;
        }
    }
}
