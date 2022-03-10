using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appliction.Core;
using Appliction.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Appliction.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id {get;set;}
        }
        public class Handler : IRequestHandler<Query,Result<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUsernameAccess _usernameAccess;

            public Handler(DataContext context,IMapper mapper,IUsernameAccess usernameAccess)
            {
                _mapper = mapper;
                _usernameAccess = usernameAccess;
                _context = context;
            }
            public async Task <Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,new {currentUsername = _usernameAccess.getUsername()})
                .FirstOrDefaultAsync(x=>x.Id == request.Id);

                return Result<ActivityDto>.Success(activity); 
            }
        }
    }
}