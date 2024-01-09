char menuChoice,
     continueToExit;
string[] staffNames = new string[] { };
int[] staffSalesCount = new int[] { };
const string DataFilePath = @"..\..\..\data.csv";

do
{
    do
    {
        // Display the menu
        DisplayMenu();

        // Get the user choice from the menu
        menuChoice = GetMenuChoice();

        // Determine the process to perform based on the user choice      
        switch (char.ToUpper(menuChoice))
        {
            case 'A':
                EnterStaffAndSalesCount(ref staffNames, ref staffSalesCount);
                break;
            case 'B':
                DisplayAllStaffSalesCountAndBonus(staffNames, staffSalesCount);
                break;
            case 'C':
                DisplayTotalSalesCount(staffSalesCount);
                break;
            case 'D':
                WriteToFile(staffNames, staffSalesCount);
                break;
            case 'E':
                ReadDataFromFile(ref staffNames, ref staffSalesCount);
                break;
            default:
                break;
        }
    } while (char.ToUpper(menuChoice) != 'X');

    // Check if there are unsaved data
    if (staffNames.Length != ReadFromFile('N', ref staffNames, ref staffSalesCount))
    {
        Console.Clear();
        Console.Write("Warning, there are unsaved records. Exit anyways?[y/n] ");
        while (!char.TryParse(Console.ReadLine(), out continueToExit) || char.ToUpper(continueToExit) != 'Y' && char.ToUpper(continueToExit) != 'N')
        {
            Console.Write("Invalid input! Warning, there are unsaved records. Exit anyways?[y/n] ");
        }
    }
    else
    {
        continueToExit = 'Y';
    }

} while (char.ToUpper(continueToExit) != 'Y');

Console.Clear();

static void DisplayMenu()
{
    Console.WriteLine("Pot of Gold Fun Center Staff Sales");
    Console.WriteLine("**********************************\n");
    Console.WriteLine("A) Enter Staff and Sales Count");
    Console.WriteLine("B) Display all Staff, Sales Count and Bonus");
    Console.WriteLine("C) Display Total Sales Count");
    Console.WriteLine("D) Data to file");
    Console.WriteLine("E) Data from file");
    Console.WriteLine("X) Exit");
}


static char GetMenuChoice()
{
    char menuChoice;
    while (!char.TryParse(Console.ReadLine(), out menuChoice) || char.ToUpper(menuChoice) != 'A' && char.ToUpper(menuChoice) != 'B' && char.ToUpper(menuChoice) != 'C' && char.ToUpper(menuChoice) != 'D' && char.ToUpper(menuChoice) != 'E' && char.ToUpper(menuChoice) != 'X')
    {
        Console.Clear();
        Console.WriteLine("***** Invalid choice *****\n");
        DisplayMenu();
    }
    return menuChoice;
}

static int GetSafeInt(string prompt)
{
    int safeInt = 0;
    Console.Write(prompt);
    while (!int.TryParse(Console.ReadLine(), out safeInt) || safeInt < 0)
    {
        Console.Write($"Invalid input! {prompt}");
    }
    return safeInt;
}

static string GetNonEmptyString(string prompt)
{
    string nonEmptyString;
    Console.Write(prompt);
    nonEmptyString = Console.ReadLine().Trim();
    while (string.IsNullOrEmpty(nonEmptyString) || nonEmptyString.IndexOf(',') > 0)
    {
        if (nonEmptyString.IndexOf(',') > 0)
        {
            Console.Write($"Invalid input! No comma allowed.  {prompt}");
        }
        else
        {
            Console.Write($"Invalid input! {prompt}");
        }
        nonEmptyString = Console.ReadLine().Trim();
    }

    return nonEmptyString;
}

static void EnterStaffAndSalesCount(ref string[] staffNames, ref int[] staffSalesCount)
{
    string prompt;

    Console.Clear();
    prompt = "\nEnter staff name >> ";
    staffNames = staffNames.Append(GetNonEmptyString(prompt)).ToArray();
    prompt = "Enter sales count >> ";
    staffSalesCount = staffSalesCount.Append(GetSafeInt(prompt)).ToArray();
    Console.WriteLine("\n Any key to continue...");
    Console.ReadKey();
    Console.Clear();
}

static void DisplayAllStaffSalesCountAndBonus(string[] staffNames, int[] staffSalesCount)
{
    double totalSalesCount = 0,
           bonus = 0;

    Console.Clear();
    Console.WriteLine("Pot of Gold Fun Center Staff Sales");
    Console.WriteLine("**********************************");
    Console.WriteLine($"{"Sales Count".PadLeft(20)}\n");
    Console.WriteLine($"{"Staff",-20} {"Sale Count",12} {"Bonus",12}\n");
    for (int i = 0; i < staffNames.Count(); i++)
    {
        GetSalesBonus(staffSalesCount[i], ref bonus);
        Console.WriteLine($"{staffNames[i],-20} {staffSalesCount[i],12} {bonus,12:C2}");
        totalSalesCount = totalSalesCount + staffSalesCount[i];
    }
    Console.WriteLine("==========".PadLeft(33));
    Console.WriteLine($"{"Total Count".PadLeft(15)} {totalSalesCount,17}");
    Console.WriteLine("\n Any key to continue...");
    Console.ReadKey();
    Console.Clear();
}


static void GetSalesBonus(int staffSalesCount, ref double bonus)
{
    if (staffSalesCount <= 9)
    {
        bonus = staffSalesCount * .10;
    }
    else if (staffSalesCount >= 10 && staffSalesCount <= 19)
    {
        bonus = staffSalesCount * .20;
    }
    else if (staffSalesCount >= 20 && staffSalesCount <= 29)
    {
        bonus = staffSalesCount * .40;
    }
    else
    {
        bonus = staffSalesCount * .50;
    }
}

static void DisplayTotalSalesCount(int[] staffSalesCount)
{
    Console.Clear();
    double totalSalesCount = 0;
    for (int i = 0; i < staffSalesCount.Length; i++)
    {
        totalSalesCount += staffSalesCount[i];
    }
    Console.WriteLine($"\nSales Count = {totalSalesCount}");
    Console.WriteLine("\n Any key to continue...");
    Console.ReadKey();
    Console.Clear();
}

static void ReadDataFromFile(ref string[] staffNames, ref int[] staffSalesCount)
{
    char isToOverwriteArray;
    int recordCountFromFile;
    string prompt;
    Console.Clear();
    Console.Write("Warning, this will delete any unsaved records. Continue?[y/n] ");
    while (!char.TryParse(Console.ReadLine(), out isToOverwriteArray) || char.ToUpper(isToOverwriteArray) != 'Y' && char.ToUpper(isToOverwriteArray) != 'N')
    {
        Console.Write("Invalid input! Warning, this will delete any unsaved records. Continue?[y/n] ");
    }
    recordCountFromFile = ReadFromFile(isToOverwriteArray, ref staffNames, ref staffSalesCount);

    if (recordCountFromFile < 1)
    {
        prompt = "record";
    }
    else
    {
        prompt = "records";
    }
    Console.WriteLine($"{recordCountFromFile} {prompt} read from file\n");
    Console.WriteLine("\n Any key to continue...");
    Console.ReadKey();
    Console.Clear();
}

static int ReadFromFile(char isToOverwriteArray, ref string[] staffNames, ref int[] staffSalesCount)
{
    int recordCountFromFile = 0;
    try
    {
        var line = File.ReadAllLines(DataFilePath);
        if (char.ToUpper(isToOverwriteArray) == 'Y')
        {
            staffNames = new string[line.Length];
            staffSalesCount = new int[line.Length];
            for (int i = 0; i < line.Length; i++)
            {

                staffNames[i] = line[i].Split(',')[0];
                staffSalesCount[i] = int.Parse(line[i].Split(',')[1]);
                recordCountFromFile++;
            }
        }
        else
        {
            for (int i = 0; i < line.Length; i++)
            {
                recordCountFromFile++;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading from {DataFilePath} with exception {ex.Message}");
    }
    return recordCountFromFile;
}

static void WriteToFile(string[] staffNames, int[] staffSalesCount)
{
    Console.Clear();
    try
    {
        using (StreamWriter writer = new StreamWriter(DataFilePath))
        {
            for (int i = 0; i < staffNames.Length; i++)
            {
                writer.WriteLine($"{staffNames[i]},{staffSalesCount[i]}");
            }
        }

        Console.WriteLine("Data saved to file");
        Console.WriteLine("\n Any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error writing to file {DataFilePath} with exception {ex.Message}");
    }
}