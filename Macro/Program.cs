Console.WriteLine("Enter the path to the folder");
string? dirName = Console.ReadLine();
Console.WriteLine();

while (!Directory.Exists(dirName))
{
    Console.WriteLine("The path doesn't exists, try again.\n");
    dirName = Console.ReadLine();
    Console.WriteLine();
}

DeleteEmptyFolders(dirName);
Console.WriteLine();
ChangeFileAttributes(dirName);
Console.WriteLine();

Console.WriteLine("Press any key to close this window");
Console.ReadKey();


static void DeleteEmptyFolders(string dirName)
{
    List<string> notUsedFolders = new();

    string[] dirs = Directory.GetDirectories(dirName, "*", SearchOption.AllDirectories);

    //Search for empty folders
    for (int i = dirs.Length - 1; i >= 0; i--)
    {
        if (Directory.GetFileSystemEntries(dirs[i]).Length == 0)
            notUsedFolders.Add(dirs[i]);
    }

    //Delete empty folders
    if (!notUsedFolders.Any())
    {
        Console.WriteLine("No folders to delete");
    }
    else
    {
        foreach (string dir in notUsedFolders.ToArray())
        {
            try
            {
                DirectoryInfo dirInfo = new(dir);
                dirInfo.Delete(true);
                Console.WriteLine($"Directory \"{dir}\" deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to delete folder. Reason: " + ex.Message);
            }
        }
    }
}

static void ChangeFileAttributes(string dirName)
{
    List<string> fileToChange = new();

    string[] files = Directory.GetFiles(dirName, "*", SearchOption.AllDirectories);

    //Search for files with desired extensions
    for (int i = files.Length - 1; i >= 0; i--)
    {
        if ((Path.GetExtension(files[i]) == ".SLDASM" ||
            Path.GetExtension(files[i]) == ".SLDDRW" ||
            Path.GetExtension(files[i]) == ".SLDPRT") &
            ((File.GetAttributes(files[i]) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly))
            fileToChange.Add(files[i]);
    }

    if (!fileToChange.Any())
    {
        Console.WriteLine("No files to change");
    }
    else
    {
        foreach (string file in fileToChange.ToArray())
        {
            try
            {
                File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.ReadOnly);
                Console.WriteLine($"File \"{file}\" marked to ReadOnly");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to change file attribute. Reason: " + ex.Message);
            }
        }
    }
}
