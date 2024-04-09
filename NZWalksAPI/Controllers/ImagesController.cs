using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        //POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequest)
        {
            ValidateFileUpload(imageUploadRequest);
            if(ModelState.IsValid)
            {
                //convert Dto to Domain model
                var imageDomainModel = new Image
                {
                    File = imageUploadRequest.File,
                    FileExtension = Path.GetExtension(imageUploadRequest.File.FileName),
                    FileSizeInBytes = imageUploadRequest.File.Length,
                    FileName = imageUploadRequest.FileName,
                    FileDescription = imageUploadRequest.FileDescription,
                };
                //User repository to upload image
                await _imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequest)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtensions.Contains(Path.GetExtension(imageUploadRequest.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            if (imageUploadRequest.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10 MB, plase upload a smaller size file.");
            }
        }
    }
}
