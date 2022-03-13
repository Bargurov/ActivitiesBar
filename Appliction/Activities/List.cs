using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Persistence;
using Appliction.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Appliction.Interfaces;
using System.Threading;

namespace Appliction.Activities
{

    public class List
    {
        public class Query : IRequest<Result<PagedList<ActivityDto>>>
        {
            public ActivityParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUsernameAccess _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUsernameAccess userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Activities
                    .Where(d => d.Date >= request.Params.StartDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                        new { currentUsername = _userAccessor.getUsername() })
                    .AsQueryable();

                if (request.Params.IsGoing && !request.Params.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.UserName == _userAccessor.getUsername()));
                }

                if (request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.HostUsername == _userAccessor.getUsername());
                }

                return Result<PagedList<ActivityDto>>.Success(
                    await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber,
                        request.Params.PageSize)
                );
            }
        }
    }
} 