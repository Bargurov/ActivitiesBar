using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appliction.Core;
using Appliction.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Appliction.Photos
{
    public class SetMain
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _dataContext;
        private readonly IUsernameAccess _usernameAccess;
            public Handler(DataContext dataContext,IUsernameAccess usernameAccess )
            {
            _usernameAccess = usernameAccess;
            _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.Include(p=>p.Photos)
                .FirstOrDefaultAsync(x=>x.UserName == _usernameAccess.getUsername());

                if(user == null) return null;
                
                var photo = user.Photos.FirstOrDefault(x=>x.Id == request.Id);

                if(photo == null) return null;

                var currentMain = user.Photos.FirstOrDefault(x=>x.IsMain);

                if(currentMain != null) currentMain.IsMain =false;

                photo.IsMain = true;

                var success = await _dataContext.SaveChangesAsync() > 0;

                if(success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Problem setting main photo");
            }
        }
    }
}