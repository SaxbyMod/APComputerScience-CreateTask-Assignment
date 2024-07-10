namespace Bookdisplayer
{
    public static class BookReader
    {
        public static void Main()
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;
            string BookDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), $"Books"));
            // Window Size Adjustment by: Node Defender - Stack Overflow
            Console.WindowHeight = 43;
            Console.WindowWidth = 85;
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
            foreach (string folderPath in Directory.GetDirectories(BookDir))
            {
                string folderName = new DirectoryInfo(folderPath).Name;
                Books.Add(folderName);
                Console.WriteLine(folderName);
            }
            string saveName = "Save.txt";
            string saveNameLowered = saveName.ToLower();
            string saveFilePath = Path.Combine(Directory.GetCurrentDirectory());
            if (File.Exists(Path.Combine(saveFilePath, saveNameLowered)))
            {
                System.IO.File.Move(saveNameLowered, saveName);
            }
        }
        // Stack Overflow - javadch
        public static string Capitalize(this string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }
        // Provided by GPT
        public static string CapitalizeWithSpaces(this string sentence)
        {
            string[] words = sentence.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].Capitalize();
            }

            return string.Join(" ", words);
        }
        public static void PageReader()
        {
            string BookDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), $"Books"));
            Console.Write("What Book would you like to read from the above options?: ");
            string Book = Console.ReadLine();
            Book = CapitalizeWithSpaces(Book);
            Console.WriteLine(Book);
            if (!Directory.Exists(Path.Combine(BookDir, Book)))
            {
                Console.Write("The Options are above the prior line Which one of those?: ");
                Book = Console.ReadLine();
                Book = CapitalizeWithSpaces(Book);
                Console.WriteLine(Book);
            }
            List<string> Pages = new List<string>();
            int maxPages = 1000;
            for (int i = 0; i < maxPages; i++)
            {
                string filePath = Path.Combine(BookDir, Book, $"{i}.txt");
                if (File.Exists(filePath))
                {
                    Pages.Add(i.ToString());
                }
                else
                {
                    break;
                }
            }
            if (Pages.Count == 0)
            {
                Console.WriteLine("Error: No pages found for the selected book.");
                return;
            }
            string saveFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Save.txt");
            bool BookFinishedForNow = false;
            while (BookFinishedForNow == false)
            {
                if (File.Exists(saveFilePath))
                {
                    string[] saveLines = File.ReadAllLines(saveFilePath);
                    bool foundCurrentPage = false;

                    foreach (string line in saveLines)
                    {
                        if (line.StartsWith($"{Book}-Current-Page: "))
                        {
                            foundCurrentPage = true;
                            string currentPageStr = line.Split(':')[1].Trim();
                            if (int.TryParse(currentPageStr, out int currentPage))
                            {
                                if (currentPage >= 0 && currentPage <= Pages.Count)
                                {
                                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), BookDir, Book, $"{currentPageStr}.txt");

                                    var fileLines = File.ReadAllLines(filePath).ToList();

                                    Console.Clear();
                                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                                    Console.WriteLine($"Book: {Book}\nPage: {string.Join(", ", Pages.ElementAt(currentPage))}");
                                    if (fileLines.Count > 0 && fileLines[0].StartsWith("Chapter: "))
                                    {
                                        string chapterContent = fileLines[0].Substring("Chapter: ".Length);
                                        Console.WriteLine($"Chapter: {chapterContent}");
                                        fileLines.RemoveAt(0);
                                    }
                                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                                    Console.WriteLine(string.Join("\n", fileLines));
                                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                                    Console.WriteLine(" <                                        X                                        > ");
                                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                                    Console.Write("What Navigation would you like to do from the above options?: ");
                                    string Option = Console.ReadLine();
                                    Functions(Option, ref currentPage, ref BookFinishedForNow, Pages.Count);
                                    Save(ref saveLines, ref Book, ref saveFilePath, ref currentPage, ref BookDir);
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

                    if (!foundCurrentPage)
                    {
                        using (StreamWriter sw = File.AppendText(saveFilePath))
                        {
                            sw.WriteLine($"{Book}-Current-Page: 0");
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
            Option = Option.ToUpper();
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
                Main();
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
                        Main();
                    }
                }
            }
        }
        public static void Save(ref string[] saveLines, ref string Book, ref string SaveFilePath, ref int currentPage, ref string BookDir)
        {
            if (!Directory.Exists(BookDir))
            {
                Directory.CreateDirectory(BookDir);
            }
            bool bookFound = false;
            for (int i = 0; i < saveLines.Length; i++)
            {
                if (saveLines[i].StartsWith($"{Book}-Current-Page: "))
                {
                    saveLines[i] = $"{Book}-Current-Page: {currentPage}";
                    bookFound = true;
                    break;
                }
            }
            if (!bookFound)
            {
                using (StreamWriter sw = File.AppendText(SaveFilePath))
                {
                    sw.WriteLine($"{Book}-Current-Page: {currentPage}");
                }
            }
            File.WriteAllLines(SaveFilePath, saveLines);
        }
    }
}