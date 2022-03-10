using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appliction.Core;
using Appliction.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Appliction.Profiles
{
    public class Details
    {
        public class Query :IRequest<Result<Profile>>
        {
            public string Username {get;set;}
        }
        public class Handler : IRequestHandler<Query, Result<Profile>>
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

            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .ProjectTo<Profile>(_mapper.ConfigurationProvider,new {currentUsername = _usernameAccess.getUsername()})
                    .SingleOrDefaultAsync(x=>x.UserName == request.Username);

                return Result<Profile>.Success(user);
            }
        }
    }
}