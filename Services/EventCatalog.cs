using EventEase.Models;

namespace EventEase.Services;

public sealed class EventCatalog
{
    private readonly List<EventItem> _events =
    [
        new EventItem
        {
            Id = 1,
            Name = "Northstar Leadership Summit",
            Date = new DateTime(2026, 10, 8),
            Location = "The Glasshouse, New York",
            Category = "LEADERSHIP",
            Description = "A day of bold ideas and practical conversations for people shaping what comes next.",
            ImageUrl = "https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=1200&q=85"
        },
        new EventItem
        {
            Id = 2,
            Name = "Founders & Futures",
            Date = new DateTime(2026, 11, 14),
            Location = "Pier 27, San Francisco",
            Category = "NETWORKING",
            Description = "An open, lively evening for founders, makers, and the people who help great ideas grow.",
            ImageUrl = "https://images.unsplash.com/photo-1517457373958-b7bdd4587205?auto=format&fit=crop&w=1200&q=85"
        },
        new EventItem
        {
            Id = 3,
            Name = "Studio Social: Winter Edition",
            Date = new DateTime(2026, 12, 10),
            Location = "The Hoxton, Chicago",
            Category = "SOCIAL",
            Description = "Good food, bright conversation, and a year-end gathering made for reconnecting.",
            ImageUrl = "https://images.unsplash.com/photo-1519167758481-83f550bb49b3?auto=format&fit=crop&w=1200&q=85"
        },
        new EventItem
        {
            Id = 4,
            Name = "Designing Tomorrow",
            Date = new DateTime(2027, 1, 21),
            Location = "The Foundry, Boston",
            Category = "WORKSHOP",
            Description = "A hands-on day for teams turning ambitious ideas into thoughtful experiences.",
            ImageUrl = "https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=1200&q=85"
        },
        new EventItem
        {
            Id = 5,
            Name = "The Good Work Forum",
            Date = new DateTime(2027, 2, 12),
            Location = "The Glasshouse, New York",
            Category = "COMMUNITY",
            Description = "A forum for sharing the ideas and practices making work better for everyone.",
            ImageUrl = "https://images.unsplash.com/photo-1517457373958-b7bdd4587205?auto=format&fit=crop&w=1200&q=85"
        },
        new EventItem
        {
            Id = 6,
            Name = "Spring Makers Dinner",
            Date = new DateTime(2027, 3, 18),
            Location = "The Hoxton, Chicago",
            Category = "SOCIAL",
            Description = "An intimate dinner celebrating the people who make ambitious projects real.",
            ImageUrl = "https://images.unsplash.com/photo-1519167758481-83f550bb49b3?auto=format&fit=crop&w=1200&q=85"
        },
        new EventItem
        {
            Id = 7,
            Name = "People First Leadership Lab",
            Date = new DateTime(2027, 4, 7),
            Location = "Pier 27, San Francisco",
            Category = "LEADERSHIP",
            Description = "A practical leadership lab centered on trust, clarity, and stronger teams.",
            ImageUrl = "https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=1200&q=85"
        },
        new EventItem
        {
            Id = 8,
            Name = "Ideas in Bloom",
            Date = new DateTime(2027, 5, 20),
            Location = "The Foundry, Boston",
            Category = "NETWORKING",
            Description = "Meet curious minds and leave with fresh connections and a few new possibilities.",
            ImageUrl = "https://images.unsplash.com/photo-1517457373958-b7bdd4587205?auto=format&fit=crop&w=1200&q=85"
        }
    ];

    public IReadOnlyList<EventItem> Events => _events;

    public EventItem? Find(int id) => _events.FirstOrDefault(eventItem => eventItem.Id == id);

    public IReadOnlyList<EventItem> GetPage(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return Array.Empty<EventItem>();
        }

        var offset = (long)(pageNumber - 1) * pageSize;
        if (offset >= _events.Count)
        {
            return Array.Empty<EventItem>();
        }

        var start = (int)offset;
        return _events.GetRange(start, Math.Min(pageSize, _events.Count - start));
    }
}