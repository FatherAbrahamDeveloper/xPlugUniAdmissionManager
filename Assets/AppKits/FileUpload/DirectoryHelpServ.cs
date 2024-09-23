namespace xPlugUniAdmissionManager.Assets.AppKits;

public class DirectoryHelpServ
{
    public string MessageInfo { get; set; } = string.Empty;
    private IWebHostEnvironment _environment;
    public DirectoryHelpServ(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public string GetFilePath(string fileName)
    {
        try
        {
            string path = Path.Combine(_environment.WebRootPath, "AppDocs", "TempUploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //var di = new DirectoryInfo(path);
            //foreach (FileInfo file in di.EnumerateFiles())
            //{
            //    if ((DateTime.Now - file.LastWriteTime).Hours > 1)
            //    {
            //        file.Delete();
            //    }
            //}

            return Path.Combine(path, fileName);
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            MessageInfo = "Process Error Unable to generate file name";
            return "";
        }
    }

    public string SaveFile(IFormFile formFile, string saveName)
    {
        try
        {
            if (formFile.Length < 5 || formFile.FileName.IsNullOrEmpty() || formFile.FileName.Length < 3)
            {
                MessageInfo = "Uploaded file is either empty, null or invalid";
                return "";
            }

            if (!formFile.IsExtensionAllowed(["jpg", "jpeg", "png"], out var msg))
            {
                MessageInfo = IsNullOrEmpty(msg) ? "Invalid File Format" : msg;
                return "";
            }

            var fileName = $"{saveName}{Path.GetExtension(formFile.FileName)}";
            var filePath = GetFilePath(fileName);
            if (IsNullOrEmpty(filePath) || filePath.Length < 5)
            {
                return "";
            }

            using FileStream stream = new(filePath, FileMode.Create);
            formFile.CopyTo(stream);

            return fileName;
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            MessageInfo = "Process Error Unable to generate file name";
            return "";
        }
    }
}
