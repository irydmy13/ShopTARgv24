using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;
using System.Xml;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARgv24Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                ShopTARgv24Context context,
                IHostEnvironment webHost
            )
        {
            _context = context;
        }
        public void FilesToApi(SpaceshipDto dto, Spaceship spaceship)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    //muutuja string uploadsFolder ja sinna laetakse failid
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "multipleFileUpload");
                    //muutuja string uniqueFileName ja siin genereeritakse uus Guid ja lisatakse see faili ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    //muutuja string filePath kombineeritakse ja lisatakse koos kausta unikaalse nimega
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipId = spaceship.Id
                        };

                        _context.FileToApis.AddAsync(path);
                    }
                }
            }
        }

        // Salvestab faile andmebaasi
        public void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                foreach (var file in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase()
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            KindergartenId = domain.Id
                        };

                        file.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.KindergartenFileToDatabase.Add(files);
                    }
                }
            }
        }

        // Eemaldab ühe pildi andmebaasist
        public async Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto)
        {
            var imageId = await _context.KindergartenFileToDatabase
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (imageId != null)
            {
                _context.KindergartenFileToDatabase.Remove(imageId);
                await _context.SaveChangesAsync();

                return imageId;
            }

            return null;
        }

        // Eemaldab kõik faile andmebaasist
        public async Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos)
        {
            foreach (var dto in dtos)
            {
                var imageId = await _context.KindergartenFileToDatabase
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (imageId != null)
                {
                    _context.KindergartenFileToDatabase.Remove(imageId);
                    await _context.SaveChangesAsync();
                }
            }
            return null;
        }
    }
}