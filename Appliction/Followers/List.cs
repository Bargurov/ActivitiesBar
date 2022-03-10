using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appliction.Core;
using Appliction.Interfaces;
using Appliction.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Appliction.Followers
{
    public class List
    {
        public class Query :IRequest<Result<List<Profiles.Profile>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUsernameAccess _usernameAccess;

            public Handler(DataContext context,IMapper mapper,IUsernameAccess usernameAccess )
            {
                _context = context;
                _mapper = mapper;
                _usernameAccess = usernameAccess;
            }

            public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<Profiles.Profile>();

                switch (request.Predicate)
                {
                    case "followers":
                    profiles = await _context.UserFollowers.Where(x=>x.Target.UserName == request.Username)
                        .Select(u=>u.Observer)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,new {currentUsername = _usernameAccess.getUsername()})
                        .ToListAsync();
                    break;

                    case "following":
                    profiles = await _context.UserFollowers.Where(x=>x.Observer.UserName == request.Username)
                        .Select(u=>u.Target)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,new {currentUsername = _usernameAccess.getUsername()})
                        .ToListAsync();
                    break;
                }
                return Result<List<Profiles.Profile>>.Success(profiles);
            }
        }
    }
}