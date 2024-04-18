namespace Bookdisplayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;
            string BookDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), $"Books"));
            // Window Size Adjustment by: Node Defender - Stack Overflow
            Console.WindowHeight = 43;
            Console.WindowWidth = 75;
            Console.WriteLine("Test 1");
            Console.WriteLine("Max height: " + Console.LargestWindowHeight.ToString());
            Console.WriteLine("Max width: " + Console.LargestWindowWidth.ToString());
            Console.Clear();
            BookGrabber();
            PageReader();
        }
        public static void BookGrabber()
        {
            List<string> Books = new List<string>();
            string BookDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), $"Books"));
            foreach (string foldername in Directory.GetDirectories(BookDir))
            {
                Books.Add(foldername);
                Console.WriteLine(foldername.Replace(BookDir + "\\", ""));
            }
        }
        public static void PageReader()
        {
            string BookDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), $"Books"));
            Console.Write("What Book would you like to read from the above options?: ");
            string Book = Console.ReadLine();
            if (Directory.Exists(Path.Combine(BookDir, Book)))
            {
                Console.WriteLine($"Book Selected {Book}");
            }
            List<string> Pages = new List<string>();
            for (int i = 0; ; i++)
            {
                if (File.Exists(Path.Combine(BookDir, Book, $"{i}.txt")))
                {
                    Pages.Add(i.ToString());
                }
                else if (!File.Exists(Path.Combine(BookDir, Book, $"{i}.txt")))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("No Pages Available or hit cap");
                    break;
                }
            }
            string saveFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Save.txt");
            bool BookFinishedForNow = false;
            while (BookFinishedForNow == false)
            {
                if (File.Exists(saveFilePath))
                {
                    string[] saveLines = File.ReadAllLines(saveFilePath);
                    foreach (string line in saveLines)
                    {
                        if (line.StartsWith($"{Book}-Current-Page: "))
                        {
                            string currentPageStr = line.Split(':')[1].Trim();
                            if (int.TryParse(currentPageStr, out int currentPage))
                            {
                                if (currentPage >= 0 && currentPage <= Pages.Count)
                                {
                                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                                    Console.WriteLine($"Book: {Book}\nPage: {string.Join(", ", Pages.ElementAt(currentPage))}");
                                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), BookDir, Book, $"{currentPageStr}.txt");
                                    string fileContent = File.ReadAllText(filePath);
                                    Console.WriteLine(fileContent);
                                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                                    Console.WriteLine("<                                   X                                   >");
                                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                                    Console.Write("What Navigation would you like to do from the above options?: ");
                                    string Option = Console.ReadLine();
                                    Functions(Option, ref currentPage, ref BookFinishedForNow, Pages.Count);
                                    Save(ref saveLines, ref Book, ref saveFilePath, ref currentPage);
                                }
                                else
                                {
                                    Console.WriteLine("Error: Current page is out of range!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error: Invalid page number format in Save.txt!");
                            }
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error: Save file not found!");
                }
            }
        }
        public static void Functions(string Option, ref int currentPage, ref bool BookFinishedForNow, int totalPages)
        {
            if (Option == ">")
            {
                if (currentPage < totalPages - 1)
                {
                    currentPage++;
                }
                else
                {
                    Console.WriteLine("Error: You are already on the last page.");
                }
            }
            else if (Option == "<")
            {
                if (currentPage > 0)
                {
                    currentPage--;
                }
                else
                {
                    Console.WriteLine("Error: You are already on the first page.");
                }
            }
            else if (Option == "X")
            {
                BookFinishedForNow = true;
            }
            else
            {
                int y = 0;
                int x = 0;
                while (x == y)
                {
                    Console.Write("Not an option, the options are '>', '<' and 'X': ");
                    Option = Console.ReadLine();
                    if (Option == ">")
                    {
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            y++;
                        }
                        else
                        {
                            Console.WriteLine("Error: You are already on the last page.");
                        }
                    }
                    else if (Option == "<")
                    {
                        if (currentPage > 0)
                        {
                            currentPage--;
                            y++;
                        }
                        else
                        {
                            Console.WriteLine("Error: You are already on the first page.");
                        }
                    }
                    else if (Option == "X")
                    {
                        BookFinishedForNow = true;
                        y++;
                    }
                }
            }
        }
        public static void Save(ref string[] saveLines, ref string Book, ref string saveFilePath, ref int currentPage)
        {
            // Provided by GPT and modified from it to work better.
            string[] updatedSaveLines = new string[saveLines.Length];
            for (int i = 0; i < saveLines.Length; i++)
            {
                if (saveLines[i].StartsWith($"{Book}-Current-Page: "))
                {
                    updatedSaveLines[i] = $"{Book}-Current-Page: {currentPage}";
                }
                else
                {
                    updatedSaveLines[i] = saveLines[i];
                }
            }
            File.WriteAllLines(saveFilePath, updatedSaveLines);
        }
    }
}