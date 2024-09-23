namespace xPlugUniAdmissionManager.Assets.AppKits;

public static class FileManagerExtensionHelper
{
    public static bool IsFileSizeAllowed(this IFormFile file, int minSize, int maxSize, out string msg)
    {
        try
        {
            if (file == null)
            {
                msg = $"The file you are trying to upload is invalid";
                return false;
            }
            var filSize = file.Length / 1024;
            if (filSize < minSize)
            {
                msg = $"The size of the document you are trying to upload is too small and of low quality. Minimum required upload size is {minSize} kilobytes. Your document's size {filSize} kilobytes";
                return false;
            }

            if (filSize > maxSize)
            {
                msg = $"The size of the document you are trying to upload is too large. Maximum required upload size is {maxSize} kilobytes. Your document's size {filSize} kilobytes";
                return false;
            }

            msg = "";
            return true;
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            msg = "We are unable to complete your request due to document size error. Please try again later";
            return false;
        }
    }

    public static bool IsExtensionAllowed(this IFormFile file, string[] allowed, out string msg)
    {
        try
        {
            var fileName = file.FileName;
            if (file == null)
            {
                msg = $"The file you are trying to upload is invalid";
                return false;
            }
            var splitteds = fileName.Split(new[] { '.' }, StringSplitOptions.None);
            if (splitteds == null || !splitteds.Any() || splitteds.Length != 2)
            {
                msg = $"The file you uploaded is invalid";
                return false;
            }

            var ext = splitteds[1];
            if (string.IsNullOrEmpty(ext) || ext.Length < 2)
            {
                msg = $"The file you uploaded is invalid";
                return false;
            }

            if (!allowed.Contains(ext.ToLower().Trim()))
            {
                msg = $"You uploaded an invalid file format! Required format includes: {string.Join(",", allowed)}";
                return false;
            }
            msg = "";
            return true;
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            msg = $"Unable to validate your uploaded file! Please try again later";
            return false;
        }
    }
}
