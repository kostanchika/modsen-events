﻿using EventsAPI.DAL.Data;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.DAL.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationContext context) : base(context) { }
        new public async Task<Event?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _dbSet.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == id, ct);
        }
        public IEnumerable<Event> GetAllEventsWithFilters(
            int page,
            int pageSize,
            string? name,
            DateTime? dateFrom,
            DateTime? dateTo,
            string? location,
            EventCategories? category
        )
        {
            var query = _dbSet.AsQueryable();

            if (name != null)
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(e => e.EventDateTime >= dateFrom);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(e => e.EventDateTime <= dateTo);
            }

            if (location != null)
            {
                query = query.Where(e => e.Location.Contains(location));
            }

            if (category.HasValue && category.Value != EventCategories.Unspecified)
            {
                query = query.Where(e => e.Category == category);
            }

            return query.Include(e => e.Participants)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .AsEnumerable();
        }

        public async Task<int> GetTotalPagesAsync(
            int pageSize,
            string? name,
            DateTime? dateFrom,
            DateTime? dateTo,
            string? location,
            EventCategories? category,
            CancellationToken ct = default
        )
        {
            var query = _dbSet.AsQueryable();

            if (name != null)
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(e => e.EventDateTime >= dateFrom);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(e => e.EventDateTime <= dateTo);
            }

            if (location != null)
            {
                query = query.Where(e => e.Location.Contains(location));
            }

            if (category.HasValue && category.Value != EventCategories.Unspecified)
            {
                query = query.Where(e => e.Category == category);
            }

            int totalItems = await query.CountAsync(ct);
            return (int)Math.Ceiling((double)totalItems / pageSize);
        }
    }
}
