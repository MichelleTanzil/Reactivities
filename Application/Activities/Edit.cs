using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;
using FluentValidation;
using Application.Errors;
using System.Net;

namespace Application.Activities
{
  public class Edit
  {
    public class Command : IRequest
    {
      public Guid Id { get; set; }
      public string Title { get; set; }
      public string Description { get; set; }
      public string Category { get; set; }
      public DateTime? Date { get; set; }
      public string City { get; set; }
      public string Venue { get; set; }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
      public CommandValidator()
      {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Venue).NotEmpty();
      }
    }
    public class Handler : IRequestHandler<Command>
    {
      private readonly DataContext _context;
      public Handler(DataContext context)
      {
        _context = context;
      }
      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        var activity = await _context.Activities.FindAsync(request.Id);
        if (activity == null)
          throw new RestException(HttpStatusCode.NotFound, new { activity = "Not Found" });

        activity.Title = request.Title ?? activity.Title;
        activity.Description = request.Description ?? activity.Description;
        activity.Category = request.Category ?? activity.Category;
        activity.Date = request.Date ?? activity.Date;
        activity.City = request.City ?? activity.City;
        activity.Venue = request.Venue ?? activity.Venue;

        var success = await _context.SaveChangesAsync();
        return success > 0 ? Unit.Value : throw new Exception("Problem saving changes");
      }
    }

  }
}
