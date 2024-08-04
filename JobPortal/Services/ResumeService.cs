using JobPortal.Models.Dto;
using JobPortal.Repositories.Interfaces;
using JobPortal.Helpers;
using JobPortal.Exceptions;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JobPortal.Enums;
using JobPortal.Models;
using JobPortal.Services.Interfaces;

public class ResumeService : IResumeService
{
    private readonly IResumeRepository _resumeRepository;
    private readonly IMapper _mapper;
    private readonly AzureBlobStorage _azureBlobStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ResumeService> _logger;

    private string GenerateRandomFileName(string originalFileName)
    {
        string randomHash = Guid.NewGuid().ToString();
        string fileExtension = Path.GetExtension(originalFileName);
        return $"{randomHash}{fileExtension}";
    }

    public ResumeService(
        IResumeRepository resumeRepository,
        IMapper mapper,
        AzureBlobStorage azureBlobStorage,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ResumeService> logger)
    {
        _resumeRepository = resumeRepository;
        _mapper = mapper;
        _azureBlobStorage = azureBlobStorage;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<ResumeDto> GetResumeByIdAsync(int resumeId)
    {
        var resume = await _resumeRepository.GetResumeByIdAsync(resumeId);
        return _mapper.Map<ResumeDto>(resume);
    }

    public async Task<IEnumerable<ResumeDto>> GetResumesByUserIdAsync(int userId)
    {
        var resumes = await _resumeRepository.GetResumesByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ResumeDto>>(resumes);
    }

    public async Task<ResumeDto> CreateResumeAsync(ResumeCreateDto resumeDto)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            _logger.LogError("HTTP context is not available.");
            throw new JobPortal.Exceptions.InvalidOperationException("HTTP context is not available.");
        }

        var user = httpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            _logger.LogError("User is not authenticated.");
            throw new JobPortal.Exceptions.InvalidOperationException("User is not authenticated.");
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            _logger.LogError("User ID claim not found.");
            throw new JobPortal.Exceptions.InvalidOperationException("User ID claim not found.");
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            _logger.LogError("User ID claim is not a valid integer.");
            throw new JobPortal.Exceptions.InvalidOperationException("User ID claim is not a valid integer.");
        }

        _logger.LogInformation("Uploading resume for user ID {UserId}", userId);
        var resumeUrl = await _azureBlobStorage.UploadResumeAsync(resumeDto.File.OpenReadStream(), resumeDto.File.FileName);

        var resume = new Resume
        {
            UserID = userId,
            FileName = resumeUrl.FileName,
            Url = resumeUrl.Uri,
            OriginalFileName = resumeDto.File.FileName,
            Status = ResumeStatus.Active
        };

        await _resumeRepository.AddResumeAsync(resume);
        _logger.LogInformation("Resume created successfully for user ID {UserId}", userId);
        return _mapper.Map<ResumeDto>(resume);
    }

    public async Task<ResumeDto> UpdateResumeAsync(int resumeId, ResumeUpdateDto resumeUpdateDto)
    {
        var resume = await _resumeRepository.GetResumeByIdAsync(resumeId);

        if (resume == null)
        {
            throw new NotFoundException($"Resume with ID {resumeId} not found.");
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new JobPortal.Exceptions.InvalidOperationException("HTTP context is not available.");
        }

        var user = httpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            throw new JobPortal.Exceptions.InvalidOperationException("User is not authenticated.");
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new JobPortal.Exceptions.InvalidOperationException("User ID claim not found.");
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new JobPortal.Exceptions.InvalidOperationException("User ID claim is not a valid integer.");
        }

        if (resume.UserID != userId)
        {
            throw new ForbiddenException("You are not authorized to update this resume.");
        }

        bool isStatusChangedOnly = resume.Status != resumeUpdateDto.Status &&
                                   resume.OriginalFileName == resumeUpdateDto.File.FileName;

        if (!isStatusChangedOnly)
        {
            _logger.LogInformation("Deleting existing resume file for user ID {UserId}", userId);
            await _azureBlobStorage.DeleteResumeAsync(resume.FileName);

            _logger.LogInformation("Uploading updated resume for user ID {UserId}", userId);
            var updatedUrl = await _azureBlobStorage.UploadResumeAsync(resumeUpdateDto.File.OpenReadStream(), resumeUpdateDto.File.FileName);

            resume.Url = updatedUrl.Uri;
            resume.FileName = updatedUrl.FileName;
        }

        resume.OriginalFileName = resumeUpdateDto.File.FileName;
        resume.Status = resumeUpdateDto.Status;

        await _resumeRepository.UpdateResumeAsync(resume);

        _logger.LogInformation("Resume updated successfully for user ID {UserId}", userId);
        return _mapper.Map<ResumeDto>(resume);
    }

    public async Task DeleteResumeAsync(int resumeId)
    {
        var resume = await _resumeRepository.GetResumeByIdAsync(resumeId);
        if (resume == null)
        {
            throw new NotFoundException($"Resume with ID {resumeId} not found.");
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new JobPortal.Exceptions.InvalidOperationException("HTTP context is not available.");
        }

        var user = httpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            throw new JobPortal.Exceptions.InvalidOperationException("User is not authenticated.");
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new JobPortal.Exceptions.InvalidOperationException("User ID claim not found.");
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new JobPortal.Exceptions.InvalidOperationException("User ID claim is not a valid integer.");
        }

        if (resume.UserID != userId)
        {
            throw new ForbiddenException("You are not authorized to delete this resume.");
        }

        _logger.LogInformation("Deleting resume for user ID {UserId}", userId);
        await _azureBlobStorage.DeleteResumeAsync(resume.FileName);
        await _resumeRepository.DeleteResumeAsync(resume);
        _logger.LogInformation("Resume deleted successfully for user ID {UserId}", userId);
    }
}
