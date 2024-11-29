﻿using AutoMapper;
using FPTAlumniConnect.API.Services.Interfaces;
using FPTAlumniConnect.BusinessTier.Payload.GroupChat;
using FPTAlumniConnect.BusinessTier.Payload;
using FPTAlumniConnect.BusinessTier.Utils;
using FPTAlumniConnect.DataTier.Models;
using FPTAlumniConnect.DataTier.Paginate;
using FPTAlumniConnect.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FPTAlumniConnect.BusinessTier.Payload.MessageGroupChat;

namespace FPTAlumniConnect.API.Services.Implements
{
    public class GroupChatService : BaseService<GroupChatService>, IGroupChatService
    {
        public GroupChatService(IUnitOfWork<AlumniConnectContext> unitOfWork, ILogger<GroupChatService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<int> CreateGroupChat(GroupChatInfo request)
        {
            var newGroupChat = _mapper.Map<GroupChat>(request);
            await _unitOfWork.GetRepository<GroupChat>().InsertAsync(newGroupChat);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException("CreateFailed");

            return newGroupChat.Id;
        }
        public async Task<GroupChatInfo> GetGroupChatById(int id)
        {
            GroupChat groupChat = await _unitOfWork.GetRepository<GroupChat>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)) ?? throw new BadHttpRequestException("GroupChatNotFound");

            return _mapper.Map<GroupChatInfo>(groupChat);
        }

        public async Task<bool> UpdateGroupChat(int id, GroupChatInfo request)
        {
            GroupChat groupChat = await _unitOfWork.GetRepository<GroupChat>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)) ?? throw new BadHttpRequestException("GroupChatNotFound");

            groupChat.RoomName = string.IsNullOrEmpty(request.RoomName) ? groupChat.RoomName : request.RoomName;
            groupChat.UpdatedAt = DateTime.Now;
            groupChat.UpdatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            _unitOfWork.GetRepository<GroupChat>().UpdateAsync(groupChat);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
        public async Task<IPaginate<GroupChatInfo>> ViewAllMessagesInGroupChat(GroupChatFilter filter, PagingModel pagingModel)
        {
            IPaginate<GroupChatInfo> response = await _unitOfWork.GetRepository<GroupChat>().GetPagingListAsync(
                selector: x => _mapper.Map<GroupChatInfo>(x),
                filter: filter,
                orderBy: x => x.OrderBy(x => x.Id),
                page: pagingModel.page,
                size: pagingModel.size
            );

            return response;
        }
    }
}
