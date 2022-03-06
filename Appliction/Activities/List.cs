using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Domain;
using Persistence;
using Appliction.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Appliction.Activities
{
    public class List
    {
        public class Query :IRequest <Result<List<ActivityDto>>>
        {

        }
        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _Mapper ;
            public Handler(DataContext context,IMapper mapper)
            {
                _Mapper = mapper;
                _context =context;
            }

            public  async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
                .ProjectTo<ActivityDto>(_Mapper.ConfigurationProvider)
                .ToListAsync();


                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}