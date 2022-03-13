using System.Threading;
using System.Threading.Tasks;
using Appliction.Core;
using Appliction.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Appliction.Followers
{
    public class FollowToggle
    {
        public class Command :IRequest<Result<Unit>>
        {
            public string TargetUsername { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
        private readonly IUsernameAccess _usernameAccess;
            public Handler(DataContext context, IUsernameAccess usernameAccess)
            {
                _usernameAccess = usernameAccess;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var Observer =await _context.Users.FirstOrDefaultAsync(x=>
                    x.UserName == _usernameAccess.getUsername());

                var target = await _context.Users.FirstOrDefaultAsync(x=> 
                    x.UserName == request.TargetUsername);

                if(target == null) return null;

                var following =await _context.UserFollowings.FindAsync(Observer.Id,target.Id);

                if(following == null)
                {
                    following = new UserFollowing
                    {
                        Observer= Observer,
                        Target =target
                    };
                    _context.UserFollowings.Add(following);
                }
                else
                {
                    _context.UserFollowings.Remove(following);
                }
                var success = await  _context.SaveChangesAsync() > 0;

                if(success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to update following");
            }
        }
    }
}