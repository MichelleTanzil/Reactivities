using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain;

namespace Application.Activities
{
  public class Search
  {
    public class ActivitiesEnvelope
    {
      public List<ActivityDto> Activities { get; set; }
      public int ActivityCount { get; set; }
    }

    public class Query : IRequest<ActivitiesEnvelope>
    {
      public Query(string searchTerm, int? limit, int? offset)
      {
        SearchTerm = searchTerm;
        Limit = limit;
        Offset = offset;
      }
      public string SearchTerm { get; set; }
      public int? Limit { get; set; }
      public int? Offset { get; set; }
    }
    public class Handler : IRequestHandler<Query, ActivitiesEnvelope>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      public Handler(DataContext context, IMapper mapper)
      {
        _mapper = mapper;
        _context = context;
      }
      public async Task<ActivitiesEnvelope> Handle(Query request, CancellationToken cancellationToken)
      {
        var queryable = _context.Activities
        .OrderBy(x => x.Date)
        .AsQueryable();

        if (request.SearchTerm.Length > 0)
        {
          queryable = queryable.
          Where(x => x.Title
            .ToLower()
            .Contains(request.SearchTerm.ToLower())
          );
        }

        var activities = await queryable
          .Skip(request.Offset ?? 0)
          .Take(request.Limit ?? 3)
          .ToListAsync();

        return new ActivitiesEnvelope
        {
          Activities = _mapper.Map<List<Activity>, List<ActivityDto>>(activities),
          ActivityCount = queryable.Count()
        };
      }
    }
  }
}