using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appliction.Core;
using Appliction.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Appliction.Comments
{
    public class Create
    {
        public  class Command : IRequest<Result<CommentDto>>
        {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x=>x.Body).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, Result<CommentDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUsernameAccess _usernameAccess;
            public Handler(DataContext context,IMapper mapper, IUsernameAccess usernameAccess)
            {
                _usernameAccess = usernameAccess;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.ActivityId);

                if(activity== null) return null;

                var user = await _context.Users
                    .Include(p=>p.Photos)
                    .SingleOrDefaultAsync(x=>x.UserName == _usernameAccess.getUsername());

                var comment = new Comment
                {
                    Author = user,
                    Activity= activity,
                    Body = request.Body
                };

                activity.Comments.Add(comment);
                var success = await _context.SaveChangesAsync() > 0 ;
                if(success) return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));

                return Result<CommentDto>.Failure("Failed to add a comment");
            }
        }
    }
}