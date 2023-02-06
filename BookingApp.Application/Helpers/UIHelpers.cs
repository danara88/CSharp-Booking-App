using BookingApp.Application.Enums;

namespace BookingApp.Application.Helpers
{
    public static class UIHelpers
    {
        /// <summary>
        /// Triggers an error message
        /// </summary>
        /// <param name="message"></param>
        public static void TriggerErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Gets the selected option
        /// </summary>
        /// <returns></returns>
        public static AppOptionsEnum GetSelectedOption()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Select an option >> ");
            Console.ForegroundColor = ConsoleColor.Gray;

            var selectedOption = Console.ReadLine();
            if (string.IsNullOrEmpty(selectedOption))
            {
                return AppOptionsEnum.ErrorNull;
            }
            try
            {
                var availableOption = int.Parse(selectedOption);
                return (AppOptionsEnum)availableOption;
            }
            catch (FormatException)
            {
                return AppOptionsEnum.ErrorFormat;
            }
        }

        /// <summary>
        /// Prints main menu with options
        /// </summary>
        public static void PrintMenu()
        {
            var optionsValues = Enum.GetValues(typeof(AppOptionsEnum));
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine();
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Console.WriteLine("^^^^^^^^ BookingApp ^^^^^^^^^^^");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Available options: ");
            var i = 0;
            foreach (var option in optionsValues)
            {
                Console.WriteLine($"{i} - {option}");
                if(i == 4)
                {
                    break;
                }
                i++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Gets user input values
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="label"></param>
        /// <param name="validators"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetFormValue (string formName, string label, FormValidatorsEnum[] validators = null)
        {
            Console.WriteLine(label);
            var inputValue = Console.ReadLine();

            if(validators is not null && validators.Length != 0)
            {
                if(validators.Contains(FormValidatorsEnum.Required)) 
                { 
                    if(string.IsNullOrEmpty(inputValue))
                    {
                        throw new Exception($"ERROR: The field {formName} is empty.");
                    }
                }

                if (validators.Contains(FormValidatorsEnum.OnlyNumbers) && !string.IsNullOrEmpty(inputValue))
                {
                    long numericValue;
                    var isNumeric = long.TryParse(inputValue, out numericValue);
                    if(!isNumeric)
                    {
                        throw new Exception($"ERROR: The field {formName} is not numeric.");
                    }
                }
                if (validators.Contains(FormValidatorsEnum.HoursFormat) && !string.IsNullOrEmpty(inputValue))
                {
                    string[] splitValue = inputValue is not null ? inputValue.Split(":") : new string[] { };
                    if(splitValue.Length <= 1)
                    {
                        throw new Exception($"ERROR: The field {formName} is not hour format. Example: 15:30");
                    }
                }
            }

            return inputValue is not null ? inputValue : "";

        }
    }
}
