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
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id {get;set;}
        }

    public class Handler : IRequestHandler<Command , Result<Unit>>
    {
            private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUsernameAccess _usernameAccess;

            public Handler(DataContext context ,IPhotoAccessor photoAccessor ,IUsernameAccess usernameAccess )
            {
                _context = context;
                _photoAccessor = photoAccessor;
                _usernameAccess = usernameAccess;
            }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(p=>p.Photos)
            .FirstOrDefaultAsync(x=>x.UserName ==_usernameAccess.getUsername());

            if(user == null) return null;

            var photo = user.Photos.FirstOrDefault(x=>x.Id == request.Id);

            if(photo == null) return null;

            if(photo.IsMain) return Result<Unit>.Failure("you cannot delete your main photo");

            var result = await _photoAccessor.DeletePhoto(photo.Id);

            if(result==null) return Result<Unit>.Failure("Problem deleteing photo from Cloudinary");

            user.Photos.Remove(photo);

            var success = await _context.SaveChangesAsync() > 0;

            if(success) return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Problem deleteing photo from API");

        }
    }
}
}