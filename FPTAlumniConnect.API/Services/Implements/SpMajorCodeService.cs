﻿using AutoMapper;
using FPTAlumniConnect.API.Services.Interfaces;
using FPTAlumniConnect.BusinessTier.Payload;
using FPTAlumniConnect.BusinessTier.Payload.Post;
using FPTAlumniConnect.BusinessTier.Payload.SpMajorCode;
using FPTAlumniConnect.DataTier.Models;
using FPTAlumniConnect.DataTier.Paginate;
using FPTAlumniConnect.DataTier.Repository.Interfaces;

namespace FPTAlumniConnect.API.Services.Implements
{
    public class SpMajorCodeService : BaseService<SpMajorCodeService>, ISpMajorCodeService
    {
        public SpMajorCodeService(IUnitOfWork<AlumniConnectContext> unitOfWork, ILogger<SpMajorCodeService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<int> CreateNewSpMajorCode(SpMajorCodeInfo request)
        {
            // Check if Major already has this name
            SpMajorCode existingMajorCode = await _unitOfWork.GetRepository<SpMajorCode>().SingleOrDefaultAsync(
                predicate: s => s.MajorId == request.MajorId && s.MajorName == request.MajorName);

            if (existingMajorCode != null)
            {
                throw new BadHttpRequestException("SpMajor already exists.");
            }

            SpMajorCode newSpMajorCode = _mapper.Map<SpMajorCode>(request);
            newSpMajorCode.CreatedAt = DateTime.Now;
            newSpMajorCode.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            await _unitOfWork.GetRepository<SpMajorCode>().InsertAsync(newSpMajorCode);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException("CreateFailed");

            return newSpMajorCode.SpMajorId;
        }

        public async Task<SpMajorCodeResponse> GetSpMajorCodeById(int id)
        {
            SpMajorCode spMajorCode = await _unitOfWork.GetRepository<SpMajorCode>().SingleOrDefaultAsync(
                predicate: x => x.SpMajorId.Equals(id)) ??
                throw new BadHttpRequestException("SpMajorCodeNotFound");

            SpMajorCodeResponse result = _mapper.Map<SpMajorCodeResponse>(spMajorCode);
            return result;
        }

        public async Task<bool> UpdateSpMajorCodeInfo(int id, SpMajorCodeInfo request)
        {
            // Check if Major already has this name
            SpMajorCode existingMajorCode = await _unitOfWork.GetRepository<SpMajorCode>().SingleOrDefaultAsync(
                predicate: s => s.MajorId == request.MajorId && s.MajorName == request.MajorName);

            if (existingMajorCode != null)
            {
                throw new BadHttpRequestException("SpMajor already exists.");
            }

            SpMajorCode spMajorCode = await _unitOfWork.GetRepository<SpMajorCode>().SingleOrDefaultAsync(
                predicate: x => x.SpMajorId.Equals(id)) ??
                throw new BadHttpRequestException("SpMajorCodeNotFound");

            spMajorCode.MajorId = request.MajorId;
            spMajorCode.MajorName = string.IsNullOrEmpty(request.MajorName) ? spMajorCode.MajorName : request.MajorName;
            spMajorCode.UpdatedAt = DateTime.Now;
            spMajorCode.UpdatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            _unitOfWork.GetRepository<SpMajorCode>().UpdateAsync(spMajorCode);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<SpMajorCodeResponse>> ViewAllSpMajorCodes(SpMajorCodeFilter filter, PagingModel pagingModel)
        {
            IPaginate<SpMajorCodeResponse> response = await _unitOfWork.GetRepository<SpMajorCode>().GetPagingListAsync(
                selector: x => _mapper.Map<SpMajorCodeResponse>(x),
                filter: filter,
                orderBy: x => x.OrderBy(x => x.CreatedAt),
                page: pagingModel.page,
                size: pagingModel.size
                );
            return response;
        }
    }
}
