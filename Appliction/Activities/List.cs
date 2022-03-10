using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Persistence;
using Appliction.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Appliction.Interfaces;

namespace Appliction.Activities
{
    public class List
    {
        public class Query :IRequest <Result<PagedList<ActivityDto>>>
        {
            public ActivityParams Params { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _Mapper ;
            private readonly IUsernameAccess _usernameAccess;

            public Handler(DataContext context,IMapper mapper,IUsernameAccess usernameAccess)
            {
                _Mapper = mapper;
                _usernameAccess = usernameAccess;
                _context =context;
            }

            public  async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Activities
                .Where(d=>d.Date>= request.Params.StartDate)
                .OrderBy(d=>d.Date)
                .ProjectTo<ActivityDto>(_Mapper.ConfigurationProvider,new {currentUsername =_usernameAccess.getUsername()})
                .AsQueryable();

                if(request.Params.IsGoing && !request.Params.IsHost)
                {
                    query = query.Where(x=>x.Attendees.Any(a=>a.UserName == _usernameAccess.getUsername()));
                }

                if(request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x=>x.HostUsername == _usernameAccess.getUsername());
                }

                return Result<PagedList<ActivityDto>>.Success(
                    await PagedList<ActivityDto>.CreateAsync(query,request.Params.PageNumber,request.Params.PageSize)
                );
            }
        }
    }
} 