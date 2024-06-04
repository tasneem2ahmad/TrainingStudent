
namespace TrainingStudent.Helpers
{
    public static class DocumentSetting
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            var folderPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot/files", folderName);
            var fileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";
            var filepath = Path.Combine(folderPath, fileName);
            using var fs = new FileStream(filepath, FileMode.Create);
            file.CopyTo(fs);
            return fileName;
        }
        public static void DeleteFile(string fileName, string folderName)
        {
            var filepath=Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot/files", folderName,fileName);
            if (File.Exists(filepath))
                File.Delete(filepath);
        }
        public static string UpdateFile(IFormFile newFile, string folderName, string oldFileName)
        {
            // Delete the old file
            DeleteFile(oldFileName, folderName);

            // Upload the new file
            return UploadFile(newFile, folderName);
        }
    }
}
